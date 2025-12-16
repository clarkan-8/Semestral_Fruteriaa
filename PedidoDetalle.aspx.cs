using Semestral_Fruteriaa.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Semestral_Fruteriaa
{
    public partial class PedidoDetalle : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USER_ID"] == null)
                {
                    pnlLogin.Visible = true;
                    pnlContenido.Visible = false;
                    return;
                }

                int pedidoId = Convert.ToInt32(Request.QueryString["id"] ?? "0");
                if (pedidoId <= 0)
                {
                    Response.Redirect("~/Pedidos.aspx");
                    return;
                }

                CargarPedido(pedidoId);
                CargarItems(pedidoId);
                CargarTracking(pedidoId);
            }
        }

        private void CargarPedido(int pedidoId)
        {
            int userId = Convert.ToInt32(Session["USER_ID"]);

            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(@"
                SELECT TOP 1 Id, Total, Estado
                FROM Pedidos
                WHERE Id = @p AND UsuarioId = @u;", conn))
            {
                cmd.Parameters.AddWithValue("@p", pedidoId);
                cmd.Parameters.AddWithValue("@u", userId);

                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read())
                    {
                        Response.Redirect("~/Pedidos.aspx");
                        return;
                    }

                    lblPedidoId.Text = rd["Id"].ToString();
                    lblTotal.Text = Convert.ToDecimal(rd["Total"]).ToString("0.00");
                    lblEstado.Text = rd["Estado"].ToString();
                }
            }
        }

        private void CargarItems(int pedidoId)
        {
            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(@"
                SELECT Nombre, Precio, Cantidad, Subtotal
                FROM PedidoItems
                WHERE PedidoId = @p;", conn))
            {
                cmd.Parameters.AddWithValue("@p", pedidoId);

                conn.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                gvItems.DataSource = dt;
                gvItems.DataBind();
            }
        }

        private void CargarTracking(int pedidoId)
        {
            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(@"
                SELECT Estado, Comentario, Fecha
                FROM PedidoEstados
                WHERE PedidoId = @p
                ORDER BY Fecha DESC;", conn))
            {
                cmd.Parameters.AddWithValue("@p", pedidoId);

                conn.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                rptTracking.DataSource = dt;
                rptTracking.DataBind();
            }
        }
    }
}
