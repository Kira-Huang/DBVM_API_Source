using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class PandoraRequest
    {
        /// <summary>
        /// 藥局位置  
        /// PHR = 中央藥局  
        /// PHRO = 門診藥局  
        /// PHRE = 急診藥局  
        /// PHR6 = 二醫藥局  
        /// PHRN = 北院區藥局
        /// </summary>
        [JsonProperty("PHARMACY")]
        public string PHARMACY { get; set; }

        /// <summary>
        /// 操作代碼
        /// </summary>
        [JsonProperty("FUNCNO")]
        public string FUNCNO { get; set; }

        /// <summary>
        /// 醫囑序號
        /// </summary>
        [JsonProperty("ORDSEQ")]
        public string ORDSEQ { get; set; }

        /// <summary>
        /// 藥碼
        /// </summary>
        [JsonProperty("BILLNUM")]
        public string BILLNUM { get; set; }

        /// <summary>
        /// 病歷號
        /// </summary>
        [JsonProperty("PATIENTID")]
        public string PATIENTID { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [JsonProperty("QUANTITY")]
        public string QUANTITY { get; set; }

        /// <summary>
        /// 藥師卡號
        /// </summary>
        [JsonProperty("SIGON")]
        public string SIGON { get; set; }

        /// <summary>
        /// 註解內容
        /// </summary>
        [JsonProperty("MEMO")]
        public string MEMO { get; set; }
    }

    public class PandoraResponse
    {
        /// <summary>
        /// 回傳狀態說明
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
