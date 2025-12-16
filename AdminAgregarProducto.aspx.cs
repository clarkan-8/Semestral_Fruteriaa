using Semestral_Fruteriaa.DAL;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace Semestral_Fruteriaa
{
    public partial class AdminAgregarProducto : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ProtegerAdmin();
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
            {
                Response.Redirect("~/Default.aspx");
                return;
            }
        }

        private bool EsAdmin(int userId)
        {
            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand("SELECT IsAdmin FROM Usuarios WHERE Id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                conn.Open();
                var val = cmd.ExecuteScalar();
                return val != null && Convert.ToBoolean(val);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ProtegerAdmin();

            string nombre = txtNombre.Text.Trim();
            string categoria = txtCategoria.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(categoria))
            {
                lblMsg.CssClass = "text-danger";
                lblMsg.Text = "Completa Nombre y Categoría.";
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
            {
                lblMsg.CssClass = "text-danger";
                lblMsg.Text = "Precio inválido. Usa formato 1.25";
                return;
            }

            decimal rating = 4.5m;
            if (!string.IsNullOrWhiteSpace(txtRating.Text) &&
                !decimal.TryParse(txtRating.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out rating))
            {
                lblMsg.CssClass = "text-danger";
                lblMsg.Text = "Rating inválido. Usa formato 4.5";
                return;
            }

            bool activo = chkActivo.Checked;

            // 1) Guardar imagen
            string imagenUrl = "";
            if (fuImagen.HasFile)
            {
                string ext = Path.GetExtension(fuImagen.FileName).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".webp")
                {
                    lblMsg.CssClass = "text-danger";
                    lblMsg.Text = "Formato de imagen no permitido. Usa jpg/png/webp.";
                    return;
                }

                string folderVirtual = "~/Images/Productos/";
                string folderFisico = Server.MapPath(folderVirtual);

                if (!Directory.Exists(folderFisico))
                    Directory.CreateDirectory(folderFisico);

                string fileName = $"{Guid.NewGuid():N}{ext}";
                string fullPath = Path.Combine(folderFisico, fileName);

                fuImagen.SaveAs(fullPath);

                imagenUrl = folderVirtual + fileName; // guardamos con ~/
            }
            else
            {
                // opcional: imagen por defecto
                imagenUrl = "https://picsum.photos/seed/fruta/600/400";
            }

            // 2) Insertar en SQL
            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(@"
                INSERT INTO Productos(Nombre, Categoria, Precio, Rating, ImagenUrl, Activo)
                VALUES(@n, @c, @p, @r, @img, @a);", conn))
            {
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.Parameters.AddWithValue("@c", categoria);
                cmd.Parameters.AddWithValue("@p", precio);
                cmd.Parameters.AddWithValue("@r", rating);
                cmd.Parameters.AddWithValue("@img", imagenUrl);
                cmd.Parameters.AddWithValue("@a", activo);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.CssClass = "text-success fw-semibold";
            lblMsg.Text = "✅ Producto guardado correctamente.";

            // limpiar
            txtNombre.Text = "";
            txtCategoria.Text = "";
            txtPrecio.Text = "";
            txtRating.Text = "";
            chkActivo.Checked = true;
        }
    }
}
