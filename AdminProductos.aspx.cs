using Semestral_Fruteriaa.DAL;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Semestral_Fruteriaa
{
    public partial class AdminProductos : Page
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
            using (var cmd = new SqlCommand(@"SELECT Id, Nombre, Categoria, Precio, Rating, ImagenUrl, Activo
                                              FROM Productos ORDER BY FechaCreacion DESC;", conn))
            {
                conn.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                gvProductos.DataSource = dt;
                gvProductos.DataBind();
            }
        }

        protected void gvProductos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProductos.EditIndex = e.NewEditIndex;
            Cargar();
        }

        protected void gvProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProductos.EditIndex = -1;
            Cargar();
        }

        protected void gvProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvProductos.DataKeys[e.RowIndex].Value);

            string nombre = ((TextBox)gvProductos.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();
            string categoria = ((TextBox)gvProductos.Rows[e.RowIndex].Cells[3].Controls[0]).Text.Trim();
            string precioStr = ((TextBox)gvProductos.Rows[e.RowIndex].Cells[4].Controls[0]).Text.Trim();
            string ratingStr = ((TextBox)gvProductos.Rows[e.RowIndex].Cells[5].Controls[0]).Text.Trim();

            if (!decimal.TryParse(precioStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
                precio = 0;

            if (!decimal.TryParse(ratingStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rating))
                rating = 4.5m;

            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(@"UPDATE Productos
                                              SET Nombre=@n, Categoria=@c, Precio=@p, Rating=@r
                                              WHERE Id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@c", categoria);
                cmd.Parameters.AddWithValue("@p", precio);
                cmd.Parameters.AddWithValue("@r", rating);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvProductos.EditIndex = -1;
            lblMsg.CssClass = "text-success fw-semibold";
            lblMsg.Text = "✅ Producto actualizado.";
            Cargar();
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleActivo")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (var conn = Db.GetConn())
                using (var cmd = new SqlCommand(@"UPDATE Productos
                                                  SET Activo = CASE WHEN Activo=1 THEN 0 ELSE 1 END
                                                  WHERE Id=@id;", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMsg.CssClass = "text-success fw-semibold";
                lblMsg.Text = "✅ Estado actualizado.";
                Cargar();
            }

            if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (var conn = Db.GetConn())
                using (var cmd = new SqlCommand(@"DELETE FROM Productos WHERE Id=@id;", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMsg.CssClass = "text-success fw-semibold";
                lblMsg.Text = "✅ Producto eliminado.";
                Cargar();
            }
        }
    }
}
