using DBVM_API.Constant;
using HIS_DB_Lib;
using MySql.Data.MySqlClient;

namespace DBVM_API.Services
{
    public class UpdateDataBaseService
    {
        private const string ConnectionString = "Server=localhost;Port=3306;Database=dbvm;User ID=user;Password=66437068;SslMode=None;";

        /// <summary>
        /// 依 GUID 更新藥袋條碼
        /// </summary>
        public static bool UpdateBarcodeByGuid(OrderClass orderClass)
        {
            string sql = @"UPDATE order_list
                           SET `藥袋條碼` = @barcode,
                               `藥袋類型` = @bagType
                           WHERE `GUID` = @guid;
                         ";

            using (var conn = new MySqlConnection(ConnectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@guid", orderClass.GUID);
                cmd.Parameters.AddWithValue("@barcode", orderClass.藥袋條碼);

                var bagType = LogicUtility.GetMedBagType(orderClass);
                if (!string.IsNullOrEmpty(bagType))
                    orderClass.藥袋類型 = bagType;

                cmd.Parameters.AddWithValue("@bagType", orderClass.藥袋類型);

                conn.Open();
                int affectedRows = cmd.ExecuteNonQuery();

                // 有更新到資料才回 true
                return affectedRows > 0;
            }
        }
    }
}
