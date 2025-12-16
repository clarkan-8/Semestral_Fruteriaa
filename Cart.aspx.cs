using Semestral_Fruteriaa.DAL;
using Semestral_Fruteriaa.Models;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Semestral_Fruteriaa
{
    public partial class WebForm1 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarCarrito();
        }

        private void CargarCarrito()
        {
            var carrito = ObtenerCarrito();

            pnlVacio.Visible = carrito.Count == 0;
            pnlCarrito.Visible = carrito.Count > 0;

            gvCarrito.DataSource = carrito;
            gvCarrito.DataBind();

            lblTotal.Text = carrito.Sum(x => x.Subtotal).ToString("0.00");

            // actualizar contador del navbar
            Session["CART_COUNT"] = carrito.Sum(x => x.Cantidad);
        }

        private List<CarritoItem> ObtenerCarrito()
        {
            if (Session["CART"] == null)
                Session["CART"] = new List<CarritoItem>();

            return (List<CarritoItem>)Session["CART"];
        }



        protected void gvCarrito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(x => x.Id == id);
            if (item == null) return;

            if (e.CommandName == "Mas")
            {
                item.Cantidad += 1;
            }
            else if (e.CommandName == "Menos")
            {
                item.Cantidad -= 1;

                // Si llega a 0, se elimina
                if (item.Cantidad <= 0)
                    carrito.Remove(item);
            }
            else if (e.CommandName == "Actualizar")
            {
                // Tomar el TextBox de la misma fila
                var btn = (Control)e.CommandSource;
                var row = (GridViewRow)btn.NamingContainer;

                var txt = (TextBox)row.FindControl("txtCantidad");

                int nuevaCantidad;
                if (!int.TryParse(txt.Text.Trim(), out nuevaCantidad))
                    nuevaCantidad = item.Cantidad;

                if (nuevaCantidad <= 0)
                    carrito.Remove(item);
                else
                    item.Cantidad = nuevaCantidad;
            }
            else if (e.CommandName == "Eliminar")
            {
                carrito.Remove(item);
            }

            Session["CART"] = carrito;
            Session["CART_COUNT"] = carrito.Sum(x => x.Cantidad);

            CargarCarrito();
        }


        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (Session["USER_ID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var carrito = ObtenerCarrito();
            if (carrito.Count == 0)
                return;

            int userId = Convert.ToInt32(Session["USER_ID"]);
            decimal total = carrito.Sum(x => x.Subtotal);

            using (var conn = Db.GetConn())
            {
                conn.Open();
                SqlTransaction tx = conn.BeginTransaction();
                bool commitOk = false;

                try
                {
                    int pedidoId;

                    // 1️⃣ Crear pedido
                    using (var cmd = new SqlCommand(@"
                INSERT INTO Pedidos(UsuarioId, Total, Estado)
                OUTPUT INSERTED.Id
                VALUES(@u, @t, 'Procesando')", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@u", userId);
                        cmd.Parameters.AddWithValue("@t", total);
                        pedidoId = (int)cmd.ExecuteScalar();
                    }

                    // 2️⃣ Insertar items
                    foreach (var it in carrito)
                    {
                        using (var cmd = new SqlCommand(@"
                    INSERT INTO PedidoItems(PedidoId, ProductoId, Nombre, Precio, Cantidad)
                    VALUES(@p, @pid, @n, @pr, @c)", conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@p", pedidoId);
                            cmd.Parameters.AddWithValue("@pid", it.Id);
                            cmd.Parameters.AddWithValue("@n", it.Nombre);
                            cmd.Parameters.AddWithValue("@pr", it.Precio);
                            cmd.Parameters.AddWithValue("@c", it.Cantidad);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 3️⃣ Estado inicial
                    using (var cmd = new SqlCommand(@"
                INSERT INTO PedidoEstados(PedidoId, Estado, Comentario)
                VALUES(@p, 'Procesando', 'Estamos preparando tu pedido.')", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@p", pedidoId);
                        cmd.ExecuteNonQuery();
                    }

                    // 4️⃣ Commit
                    tx.Commit();
                    commitOk = true;

                    // 5️⃣ Limpiar carrito
                    Session["CART"] = new List<Semestral_Fruteriaa.Models.CarritoItem>();
                    Session["CART_COUNT"] = 0;

                    Response.Redirect("~/Pedidos.aspx");
                }
                catch
                {
                    if (!commitOk)
                        tx.Rollback();

                    throw;
                }
            }
        }




    }
}
