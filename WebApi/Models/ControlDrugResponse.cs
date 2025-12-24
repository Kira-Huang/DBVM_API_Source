using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class ControlDrugResponse
    {
        /// <summary>
        /// 藥局位置
        /// </summary>
        [JsonProperty("DEPT_ID")]
        public string DEPT_ID { get; set; }

        /// <summary>
        /// 藥碼
        /// </summary>
        [JsonProperty("DRUG_CODE")]
        public string DRUG_CODE { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [JsonProperty("QUANTITY")]
        public int QUANTITY { get; set; }

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
        /// 核對人編號
        /// </summary>
        [JsonProperty("PROCID")]
        public string PROCID { get; set; }

        /// <summary>
        /// 核對人姓名
        /// </summary>
        [JsonProperty("PROCNAME")]
        public string PROCNAME { get; set; }
    }
}
