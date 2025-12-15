using Semestral_Fruteriaa.DAL;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace Semestral_Fruteriaa
{
    public partial class Register : Page
    {
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string email = txtEmail.Text.Trim().ToLower();
            string pass = txtPass.Text;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                lblMsg.Text = "Completa todos los campos.";
                return;
            }

            string hash = HashPassword(pass);

            try
            {
                using (var conn = Db.GetConn())
                using (var cmd = new SqlCommand(@"INSERT INTO Usuarios(Nombre, Email, PasswordHash)
                                                  VALUES(@n,@e,@p)", conn))
                {
                    cmd.Parameters.AddWithValue("@n", nombre);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@p", hash);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                Response.Redirect("~/Login.aspx");
            }
            catch (SqlException ex)
            {
                // 2627/2601 = duplicado (email UNIQUE)
                if (ex.Number == 2627 || ex.Number == 2601)
                    lblMsg.Text = "Ese email ya está registrado.";
                else
                    lblMsg.Text = "Error al registrar: " + ex.Message;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
