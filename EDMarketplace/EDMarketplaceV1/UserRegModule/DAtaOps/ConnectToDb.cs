using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegModule.DAtaOps
{
    public sealed class ConnectToDb1
    {
        private static ConnectToDb1 instance = null;
        private static readonly object padlock = new object();
        public MySqlConnection conn = null;
        ConnectToDb1()
        {
            string connStr = "server=localhost;user id=root;password=Welcome123#;persistsecurityinfo=True;database=edmarketplace";
            conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Console.WriteLine("Opened DB");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static ConnectToDb1 Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ConnectToDb1();
                    }
                    return instance;
                }
            }
        }
    }
}
