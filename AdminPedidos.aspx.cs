using Semestral_Fruteriaa.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Semestral_Fruteriaa
{
    public partial class AdminPedidos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProtegerAdmin();
                Cargar();
            }
        }

        private void ProtegerAdmin()
        {
            if (Session["USER_ID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["USER_ID"]);
            if (!EsAdmin(userId))
                Response.Redirect("~/Default.aspx");
        }

        private bool EsAdmin(int userId)
        {
            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand("SELECT IsAdmin FROM Usuarios WHERE Id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                conn.Open();
                var v = cmd.ExecuteScalar();
                return v != null && Convert.ToBoolean(v);
            }
        }

        private void Cargar()
        {
            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(@"SELECT Id, UsuarioId, Total, Estado, FechaCreacion
                                              FROM Pedidos
                                              ORDER BY FechaCreacion DESC;", conn))
            {
                conn.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                gvPedidosAdmin.DataSource = dt;
                gvPedidosAdmin.DataBind();
            }
        }

        protected void gvPedidosAdmin_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "GuardarEstado") return;

            int pedidoId = Convert.ToInt32(e.CommandArgument);

            var btn = (Control)e.CommandSource;
            var row = (GridViewRow)btn.NamingContainer;
            var ddl = (DropDownList)row.FindControl("ddlEstado");
            string nuevoEstado = ddl.SelectedValue;

            using (var conn = Db.GetConn())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        // 1) actualizar pedido
                        using (var cmd = new SqlCommand(@"UPDATE Pedidos SET Estado=@e WHERE Id=@p;", conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@e", nuevoEstado);
                            cmd.Parameters.AddWithValue("@p", pedidoId);
                            cmd.ExecuteNonQuery();
                        }

                        // 2) agregar tracking
                        string comentario;

                        switch (nuevoEstado)
                        {
                            case "Procesando":
                                comentario = "Estamos preparando tu pedido.";
                                break;

                            case "Enviado":
                                comentario = "Tu pedido fue enviado.";
                                break;

                            case "Entregado":
                                comentario = "Pedido entregado. ¡Gracias por comprar!";
                                break;

                            case "Cancelado":
                                comentario = "Pedido cancelado por el administrador.";
                                break;

                            default:
                                comentario = "Estado actualizado.";
                                break;
                        }


                        using (var cmd = new SqlCommand(@"INSERT INTO PedidoEstados(PedidoId, Estado, Comentario)
                                                          VALUES(@p,@e,@c);", conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@p", pedidoId);
                            cmd.Parameters.AddWithValue("@e", nuevoEstado);
                            cmd.Parameters.AddWithValue("@c", comentario);
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();

                        lblMsg.CssClass = "text-success fw-semibold";
                        lblMsg.Text = "✅ Estado actualizado y tracking agregado.";
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }

            Cargar();
        }
    }
}
