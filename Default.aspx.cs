using Semestral_Fruteriaa.DAL;
using Semestral_Fruteriaa.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Semestral_Fruteriaa
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblInfo.Text = "";
                CargarProductos();
            }   
        }

        protected void ddlOrden_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarProductos(); // ordenar desde SQL
        }

        private void CargarProductos()
        {
            string order = ddlOrden.SelectedValue;

            string orderBy;
            switch (order)
            {
                case "asc":
                    orderBy = "Precio ASC";
                    break;
                case "desc":
                    orderBy = "Precio DESC";
                    break;
                default:
                    orderBy = "Rating DESC";
                    break;
            }

            string sql = $@"
                SELECT Id, Nombre, Categoria, Precio, Rating, ImagenUrl
                FROM Productos
                WHERE Activo = 1
                ORDER BY {orderBy};";

            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                rptProductos.DataSource = dt;
                rptProductos.DataBind();
            }
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Agregar")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                // 1) Obtener el producto desde SQL
                Producto prod = ObtenerProductoPorId(id);
                if (prod == null)
                {
                    lblInfo.Text = "❌ Producto no encontrado.";
                    return;
                }

                // 2) Carrito en Session (lista real)
                var carrito = Session["CART"] as List<CarritoItem> ?? new List<CarritoItem>();

                // 3) Sumar cantidad si ya existe
                var item = carrito.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    item.Cantidad += 1;
                }
                else
                {
                    carrito.Add(new CarritoItem
                    {
                        Id = prod.Id,
                        Nombre = prod.Nombre,
                        Categoria = prod.Categoria,
                        ImagenUrl = prod.ImagenUrl,
                        Precio = prod.Precio,
                        Cantidad = 1
                    });
                }

                // 4) Guardar y actualizar contador
                Session["CART"] = carrito;
                Session["CART_COUNT"] = carrito.Sum(x => x.Cantidad);

                lblInfo.Text = $"✅ Agregaste: {prod.Nombre} al carrito.";
            }
        }

        private Producto ObtenerProductoPorId(int id)
        {
            const string sql = @"
                SELECT TOP 1 Id, Nombre, Categoria, Precio, Rating, ImagenUrl
                FROM Productos
                WHERE Activo = 1 AND Id = @Id;";

            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new Producto
                        {
                            Id = Convert.ToInt32(rd["Id"]),
                            Nombre = rd["Nombre"].ToString(),
                            Categoria = rd["Categoria"].ToString(),
                            Precio = Convert.ToDecimal(rd["Precio"]),
                            Rating = Convert.ToDecimal(rd["Rating"]),
                            ImagenUrl = rd["ImagenUrl"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        // Modelos
        private class Producto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public decimal Precio { get; set; }
            public decimal Rating { get; set; }
            public string ImagenUrl { get; set; }
        }

       
    }
}
