using HIS_DB_Lib;
using System;

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
            return time.ToString("yyyyMMdd HH:mm:ss");
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
                return dt.ToString("yyyyMMdd HH:mm:ss");
            }
            else
            {

                return DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            }
        }

    }
}
