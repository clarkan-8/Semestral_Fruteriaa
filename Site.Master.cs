using System;
using Semestral_Fruteriaa.DAL;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Semestral_Fruteriaa
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USER_ID"] != null)
            {
                int userId = Convert.ToInt32(Session["USER_ID"]);
                phAdmin.Visible = EsAdmin(userId);
            }
            else
            {
                phAdmin.Visible = false;
            }
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
    }
}