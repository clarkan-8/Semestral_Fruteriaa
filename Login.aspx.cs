using Semestral_Fruteriaa.DAL;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace Semestral_Fruteriaa
{
    public partial class Login : Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim().ToLower();
            string pass = txtPass.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                lblMsg.Text = "Completa email y contraseña.";
                return;
            }

            string hash = HashPassword(pass);

            using (var conn = Db.GetConn())
            using (var cmd = new SqlCommand(@"SELECT Id, Nombre FROM Usuarios
                                              WHERE Email=@e AND PasswordHash=@p", conn))
            {
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@p", hash);

                conn.Open();
                var rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    // guardar sesión
                    Session["USER_ID"] = rd["Id"];
                    Session["USER_NAME"] = rd["Nombre"].ToString();

                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    lblMsg.Text = "Credenciales incorrectas.";
                }
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
