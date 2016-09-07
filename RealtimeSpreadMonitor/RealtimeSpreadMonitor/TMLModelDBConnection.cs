using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MySql.Data.MySqlClient;

namespace RealtimeSpreadMonitor
{
    public class TMLModelDBConnection
    {
        //public MySqlConnection conn;

        //connection to database here
        public void connectDB(String dbServerName)
        {
            try
            {
                String connString =
                    String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false ", dbServerName,
                    TradingSystemConstants.DB_USERNAME, TradingSystemConstants.DB_PASSWORD, TradingSystemConstants.DB_DATABASENAME);
                
                //conn = new MySqlConnection(connString);
                //conn.Open();

                

            }
            catch (System.Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void closeDB()
        {
            try
            {
                //conn.Close();

                //conn.Dispose();
            }
            catch (System.Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }
    }
}
