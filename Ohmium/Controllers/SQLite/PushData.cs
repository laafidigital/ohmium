using Microsoft.Data.Sqlite;
using Ohmium.Models;
using Ohmium.Models.EFModels.LotusModels;
using System;
using System.Threading.Tasks;

namespace Ohmium.Controllers.SQLite
{
    public class PushData
    {
        public async Task<string> DeleteLog()
        {
            string msg = "";
            string connectionString = "Data Source=wwwroot\\content\\data\\SQCache.db;Cache=Shared";
            string tableName = "deviceDataLog";
            string errorlog = "";

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                DateTime dtm = DateTime.UtcNow.AddDays(-10);
                // Create a SQL DELETE command using a lambda expression
                string deleteQuery = $"DELETE FROM {tableName} where timestamp < {dtm} ";

                using (SqliteCommand command = new SqliteCommand(deleteQuery, connection))
                {
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        //return msg;
                    }
                    catch (Exception ex)
                    {
                        //return "failed";
                        errorlog += "Delete datalog failed with exception - " + ex.Message + "|" + ex.InnerException;
                    }
                }
                connection.Close();
            }
            return errorlog;
        }
        public async Task<string> DeleteSQLiteLiveData()
        {
            string msg = "";
            string connectionString = "Data Source=wwwroot\\content\\data\\SQCache.db;Cache=Shared";
            string tableName1 = "mtsstackdata";
            string tableName2 = "mtsdevicedata";
            string tableName3 = "deviceDataLog";
            string errorlog = "";
           
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Create a SQL DELETE command using a lambda expression
                string deleteQuery = $"DELETE FROM {tableName1}";

                using (SqliteCommand command = new SqliteCommand(deleteQuery, connection))
                {
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        //return msg;
                    }
                    catch (Exception ex)
                    {
                        //return "failed";
                        errorlog += "Delete stackdata failed with exception - " + ex.Message + "|" + ex.InnerException;
                    }
                }
                connection.Close();
            }
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Create a SQL DELETE command using a lambda expression
                string deleteQuery = $"DELETE FROM {tableName2}";

                using (SqliteCommand command = new SqliteCommand(deleteQuery, connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        //return msg;
                    }
                    catch (Exception ex)
                    {
                        //return "failed";
                        errorlog += "Delete devicedata failed with exception - " + ex.Message + "|" + ex.InnerException;
                    }
                }
                connection.Close();
            }
            //using (SqliteConnection connection = new SqliteConnection(connectionString))
            //{
            //    connection.Open();
                
            //    // Create a SQL DELETE command using a lambda expression
            //    string deleteQuery = $"DELETE FROM {tableName3}";

            //    using (SqliteCommand command = new SqliteCommand(deleteQuery, connection))
            //    {
            //        try
            //        {
            //            await command.ExecuteNonQueryAsync();
            //            //return msg;
            //        }
            //        catch (Exception ex)
            //        {
            //            //return "failed";
            //            errorlog += "Delete devicedatalog failed with exception - " + ex.Message + "|" + ex.InnerException;
            //        }
            //    }
            //    connection.Close();
            //}

            return errorlog;
        }

    }
}
