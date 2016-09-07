using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeSpreadMonitor
{
    public class TMLAzureDBConnection
    {
        //public SqlConnection conn;
        public SqlConnectionStringBuilder connString1Builder;

        public TMLAzureDBConnection()
        {
            setupSqlConnectionString();
        }

        //connection to database here
        public void setupSqlConnectionString()
        {
            try
            {
                connString1Builder = new SqlConnectionStringBuilder();
                connString1Builder.DataSource = "tcp:h9ggwlagd1.database.windows.net,1433";
                connString1Builder.InitialCatalog = "TMLDB";
                connString1Builder.Encrypt = true;
                connString1Builder.TrustServerCertificate = false;
                connString1Builder.UserID = "realtimeuser@h9ggwlagd1";
                connString1Builder.Password = "NJoyce111174~";
                connString1Builder.MultipleActiveResultSets = true;
                connString1Builder.Pooling = true;
                connString1Builder.MaxPoolSize = 50;
                connString1Builder.ConnectTimeout = 30;
            }
            catch (System.Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public int ConnectDBSqlDataAdapter(string sqlQuery, DataSet dataSet)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connString1Builder.ToString()))
                {
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection))
                    {
                        int rows = adapter.Fill(dataSet);

                        connection.Close();

                        return rows;
                    }

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }

        


        public delegate void RunReaderAfterSQLConn(SqlDataReader reader,
            Object workingReaderObject1,
            Object workingReaderObject2,
            Object workingReaderObject3);

        public void ConnectDBAndExecuteReader(string sqlQuery,
            RunReaderAfterSQLConn runReaderAfterSQLConn,
            Object workingReaderObject1,
            Object workingReaderObject2,
            Object workingReaderObject3)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(connString1Builder.ToString()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {

                        command.CommandTimeout = 0;

                        //Console.WriteLine("Executed Thread.. " + Thread.CurrentThread.ManagedThreadId);
                        //command.CommandTimeout = 80;
                        command.ExecuteNonQuery();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            runReaderAfterSQLConn(reader,
                                workingReaderObject1,
                                workingReaderObject2,
                                workingReaderObject3);
                            //        {
                            //            //while (reader.Read())
                            //            //{
                            //            //    Console.WriteLine(reader.GetValue(0));
                            //            //}
                            //            reader = command.ExecuteReader();
                            //            return reader;

                            reader.Close();
                        }


                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //return null;
        } 


        public void ConnectDBAndExecuteQuerySync(string sqlQuery)
        //SqlConnectionStringBuilder connString1Builderx)//, SqlConnection connection)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString1Builder.ToString()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {

                        command.CommandTimeout = 0;

                        //Console.WriteLine("Executed Thread.. " + Thread.CurrentThread.ManagedThreadId);
                        //command.CommandTimeout = 80;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //public void ConnectDBAndExecuteQuerySync(string sqlQuery)
        ////SqlConnectionStringBuilder connString1Builderx)//, SqlConnection connection)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connString1Builder.ToString()))
        //        {
        //            connection.Open();

        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                //Console.WriteLine("Executed Thread.. " + Thread.CurrentThread.ManagedThreadId);
        //                //command.CommandTimeout = 80;
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}

        public SqlCommand ConnectDBAndReturnCommandForPrepare(string sqlQuery)
        //SqlConnectionStringBuilder connString1Builderx)//, SqlConnection connection)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString1Builder.ToString()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        //Console.WriteLine("Executed Thread.. " + Thread.CurrentThread.ManagedThreadId);
                        //command.CommandTimeout = 80;
                        return command;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public void ExecutePreparedSqlCommand(SqlCommand command)
        //SqlConnectionStringBuilder connString1Builderx)//, SqlConnection connection)
        {
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public Object ConnectDBAndExecuteScalarSync(string sqlQuery)
        //SqlConnectionStringBuilder connString1Builderx)//, SqlConnection connection)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString1Builder.ToString()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        //Console.WriteLine("Executed Thread.. " + Thread.CurrentThread.ManagedThreadId);
                        //command.CommandTimeout = 80;
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }


        public static async Task ConnectDBAndExecuteQueryAsyncWithTransaction(
            List<string> queryStringToUpdate,
            SqlConnectionStringBuilder connString1BuilderInternal)//, SqlConnection connection)
        {
            try
            {
                //http://executeautomation.com/blog/using-async-and-await-for-asynchronous-operation-also-with-multi-threading/
                //http://stackoverflow.com/questions/200986/c-sql-how-to-execute-a-batch-of-storedprocedure
                //http://stackoverflow.com/questions/17008902/sending-several-sql-commands-in-a-single-transaction

                //string sqlQuery = "insert into tblMasterLookup Values (" + ThreadNumber + ",'Test','2.0','Myapplication',GetDate()) waitfor delay '00:00:30'";

                //string connectionString = @"Server=.\SQLEXPRESS;Database=AUTODATA;Password=abc123;User ID=sa";
                using (SqlConnection connection = new SqlConnection(connString1BuilderInternal.ToString()))
                {
                    connection.Open();

                    //using (SqlTransaction trans = connection.BeginTransaction())
                    {

                        using (SqlCommand command = new SqlCommand("", connection))
                        {
                            command.CommandType = System.Data.CommandType.Text;
                            //IAsyncResult result = command.BeginExecuteNonQuery();
                            //Console.WriteLine("Command complete. Affected {0} rows.",
                            //command.EndExecuteNonQuery(result));

                            //DateTime currenttime = DateTime.Now;
                            //Console.WriteLine("Executed Thread.. " + Thread.CurrentThread.ManagedThreadId);
                            //command.CommandTimeout = 80;

                            foreach (var commandString in queryStringToUpdate)
                            {
                                command.CommandText = commandString;
                                await command.ExecuteNonQueryAsync();
                                //command.ExecuteNonQuery();

                            }

                            //long elapsedTicks = DateTime.Now.Ticks - currenttime.Ticks;
                            //TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

                            //TSErrorCatch.debugWriteOut("TEST 2 seconds " + elapsedSpan.TotalSeconds);
                        }

                        //trans.Commit();

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static async Task ConnectDBAndExecuteQueryAsync(string sqlQuery,
            SqlConnectionStringBuilder connString1BuilderInternal)//, SqlConnection connection)
        {
            try
            {
                //http://executeautomation.com/blog/using-async-and-await-for-asynchronous-operation-also-with-multi-threading/
                //http://stackoverflow.com/questions/200986/c-sql-how-to-execute-a-batch-of-storedprocedure
                //http://stackoverflow.com/questions/17008902/sending-several-sql-commands-in-a-single-transaction

                //string sqlQuery = "insert into tblMasterLookup Values (" + ThreadNumber + ",'Test','2.0','Myapplication',GetDate()) waitfor delay '00:00:30'";

                //string connectionString = @"Server=.\SQLEXPRESS;Database=AUTODATA;Password=abc123;User ID=sa";
                using (SqlConnection connection = new SqlConnection(connString1BuilderInternal.ToString()))
                {
                    connection.Open();



                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        //command.CommandType = CommandType.StoredProcedure;

                        //DateTime currenttime = DateTime.Now;
                        //Console.WriteLine("Executed Thread.. " + Thread.CurrentThread.ManagedThreadId);
                        //command.CommandTimeout = 80;
                        await command.ExecuteNonQueryAsync();

                        //long elapsedTicks = DateTime.Now.Ticks - currenttime.Ticks;
                        //TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

                        //TSErrorCatch.debugWriteOut("TEST 2 seconds " + elapsedSpan.TotalSeconds);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //public void closeDB()
        //{
        //    try
        //    {
        //        conn.Close();

        //        conn.Dispose();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}
    }
}
