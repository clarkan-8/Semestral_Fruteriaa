using Semestral_Fruteriaa.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Semestral_Fruteriaa
{
    public partial class Pedidos : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["USER_ID"] == null)
                {
                    pnlLogin.Visible = true;
                    return;
                }

                CargarPedidos();
            }
        }

        private void CargarPedidos()
        {
            int userId = Convert.ToInt32(Session["USER_ID"]);

            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(@"
                SELECT Id, Total, Estado, FechaCreacion
                FROM Pedidos
                WHERE UsuarioId = @u
                ORDER BY FechaCreacion DESC;", conn))
            {
                cmd.Parameters.AddWithValue("@u", userId);

                conn.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                pnlVacio.Visible = dt.Rows.Count == 0;

                gvPedidos.DataSource = dt;
                gvPedidos.DataBind();
            }
        }
    }
}
