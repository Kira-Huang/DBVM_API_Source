using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Text.Json.Serialization;

namespace DBVM_API.Models
{
    public class SmallDrugRequest
    {
        /// <summary>
        /// 開始日期  格式 YYYYMMDDHHMMSS
        /// </summary>
        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// 結束日期  格式 YYYYMMDDHHMMSS
        /// </summary>
        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        /// <summary>
        /// Y = 取得已讀取 / N = 取得未讀取 / 不傳 = 取得全部
        /// </summary>
        [JsonProperty("complete", NullValueHandling = NullValueHandling.Ignore)]
        public string Complete { get; set; }
     }
}
