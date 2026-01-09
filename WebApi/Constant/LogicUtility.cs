using HIS_DB_Lib;
using System;
using System.Text.RegularExpressions;

namespace DBVM_API.Constant
{
    public class LogicUtility
    {
        /// <summary>
        /// 取得交易量 (負值)
        /// </summary>
        /// <param name="value">藥品數量</param>
        /// <returns></returns>
        public static string GetTradingVolume(string value)
        {
            return string.Concat("-", value);
        }

        /// <summary>
        /// 取得開方日期字串
        /// </summary>
        /// <param name="time">時間</param>
        /// <returns>開方日期字串 (格式 yyyyMMdd HH:mm:ss)</returns>
        public static string GetPrescriptionDate(DateTime time)
        {
            return time.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// 取得開方日期字串 (格式 yyyyMMdd HH:mm:ss)
        /// </summary>
        /// <param name="time">時間字串</param>
        /// <returns>開方日期字串 (格式 yyyyMMdd HH:mm:ss)</returns>
        public static string GetPrescriptionDate(string time)
        {             
            if (DateTime.TryParse(time, out DateTime dt))
            {
                return dt.ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {

                return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 取得簡易開方日期字串 (格式 yyyyMMddHHmmss)
        /// </summary>
        /// <param name="time">開方日期字串</param>
        /// <returns></returns>
        public static string GetSimplePrescriptionDate(string time)
        {
            string result = time.Replace("-", "").Replace("/", "").Replace(":", "").Replace(" ", "").Trim();
            return result;
        }

        /// <summary>
        /// 取得主要Pri_Key
        /// </summary>
        /// <param name="orderClass">處方物件</param>
        /// <returns>組好的Pri_Key</returns>
        public static string GetPrimaryKey(OrderClass orderClass)
        {
            string 時間 = GetSimplePrescriptionDate(orderClass.開方日期);

            //====== PRI_KEY ======
            string key = $"{orderClass.頻次}{orderClass.天數}{orderClass.單次劑量}{orderClass.劑量單位}";
            return $"{時間}-{orderClass.病歷號}-{orderClass.藥品碼}{orderClass.交易量}-{key}";
        }

        /// <summary>
        /// 取得單次劑量及劑量單位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DoseInfo ParseDose(string input)
        {
            DoseInfo doseInfo = new DoseInfo();

            if (string.IsNullOrWhiteSpace(input))
                return null;

            var regex = new Regex(@"^\s*(\d+(\.\d+)?)\s*([A-Za-z]+)\s*$");
            var match = regex.Match(input);

            if (!match.Success)
                return null;

            doseInfo.SingleDose = match.Groups[1].Value;
            doseInfo.DoseUnit = match.Groups[3].Value.ToUpper();

            return doseInfo;
        }
    }
}
