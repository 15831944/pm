using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NanoFabric.Infrastrue.Ibatis.DataBaseHandle
{
    public class BigDataManagement
    {
        private static string connectString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        /// <summary>
        /// modify by Colin
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        /// <param name="isClearData"></param>
        public static void BulkCopy(string tableName, DataTable dt, bool isClearData = false)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(connectString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.BatchSize = dt.Rows.Count;
                            bulkCopy.DestinationTableName = tableName;
                            try
                            {
                                if (isClearData)
                                {
                                    string sql = "TRUNCATE TABLE " + tableName;
                                    SqlCommand cmd = new SqlCommand(sql, connection, transaction);
                                    cmd.ExecuteNonQuery();
                                }
                                foreach (DataColumn column in dt.Columns)
                                {
                                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                                }
                                bulkCopy.WriteToServer(dt);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                LogHelper.WriteLog(typeof(BigDataManagement), ex);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }
            }
        }

    }
}
