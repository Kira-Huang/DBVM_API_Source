using MySql.Data.MySqlClient;

namespace DBVM_API.Services
{
    public class UpdateDataBaseService
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=dbvm;User ID=user;Password=66437068;SslMode=None;";

        /// <summary>
        /// 依 GUID 更新藥袋條碼
        /// </summary>
        public static bool UpdateBarcodeByGuid(string guid, string barcode)
        {
            string sql = @"UPDATE order_list
                           SET 藥袋條碼 = @barcode
                           WHERE GUID = @guid;
                         ";

            using (var conn = new MySqlConnection(ConnectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@GUID", guid);
                cmd.Parameters.AddWithValue("@藥袋條碼", barcode);

                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();

                // 有更新到資料才回 true
                return affectedRows > 0;
            }
        }
    }
}
