using System.Configuration;
using System.Data.SqlClient;

namespace Semestral_Fruteriaa.DAL
{
    public static class Db
    {
        public static SqlConnection GetConn()
        {
            string cs = ConfigurationManager.ConnectionStrings["FruteriaDB"].ConnectionString;
            return new SqlConnection(cs);
        }
    }
}
