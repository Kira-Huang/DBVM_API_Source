using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class SmartMedCabinetResponse
    {
        /// <summary>
        /// 藥名
        /// </summary>
        [JsonProperty("UDDMDPNAME")]
        public string UDDMDPNAME { get; set; }

        /// <summary>
        /// 儲存類型
        /// </summary>
        [JsonProperty("DRUGTYPE")]
        public string DRUGTYPE { get; set; }

        /// <summary>
        /// 儲位1
        /// </summary>
        [JsonProperty("STORE_POS_1")]
        public string STORE_POS_1 { get; set; }

        /// <summary>
        /// 儲位2
        /// </summary>
        [JsonProperty("STORE_POS_2")]
        public string STORE_POS_2 { get; set; }

        /// <summary>
        /// 最近更新時間
        /// </summary>
        [JsonProperty("UPDATETIME")]
        public string UPDATETIME { get; set; }

        /// <summary>
        /// 最近更新卡號
        /// </summary>
        [JsonProperty("UPDATEID")]
        public string UPDATEID { get; set; }

        /// <summary>
        /// 藥碼
        /// </summary>
        [JsonProperty("UDDRUGCODE")]
        public string UDDRUGCODE { get; set; }

        /// <summary>
        /// 最近更新人
        /// </summary>
        [JsonProperty("UPDATENAME")]
        public string UPDATENAME { get; set; }

        /// <summary>
        /// 藥局位置
        /// </summary>
        [JsonProperty("POSITION")]
        public string POSITION { get; set; }
    }
}
