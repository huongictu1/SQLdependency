using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealChart.Common
{
    public static class PostgressConnect
    {
        private static string Host = "localhost";
        private static string User = "postgres";
        private static string DBname = "postgres";
        private static string Password = "Admin@123";
        private static string Port = "5432";

        public static DataTable GetDataTable()
        {
            try
            {
                string connString = String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer", Host, User, DBname, Port, Password);
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand("select * from tbl_demo", conn))
                    {
                        var reader = command.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }           
        }
    }
}
