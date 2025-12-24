using Newtonsoft.Json;

namespace DBVM_API.Models
{

    public class TakeDrugResponse
    {
        // <summary>
        /// 主鍵編號，用來回傳回寫欄位依據
        /// </summary>
        [JsonProperty("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [JsonProperty("CREATETIME")]
        public string CREATETIME { get; set; }

        /// <summary>
        /// 讀取時間
        /// </summary>
        [JsonProperty("READTIME")]
        public string READTIME { get; set; }

        /// <summary>
        /// 實配量
        /// </summary>
        [JsonProperty("UDQNTY2")]
        public string UDQNTY2 { get; set; }

        /// <summary>
        /// 實配量
        /// </summary>
        [JsonProperty("UDQNTY")]
        public string UDQNTY { get; set; }

        /// <summary>
        /// 領藥號
        /// </summary>
        [JsonProperty("DISPNO")]
        public string DISPNO { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        [JsonProperty("BEDNO")]
        public string BEDNO { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        [JsonProperty("HNAMEC")]
        public string HNAMEC { get; set; }

        /// <summary>
        /// 醫囑序號
        /// </summary>
        [JsonProperty("ORDSEQ")]
        public string ORDSEQ { get; set; }

        /// <summary>
        /// 護理站
        /// </summary>
        [JsonProperty("HNURSTA")]
        public string HNURSTA { get; set; }

        /// <summary>
        /// 應配量
        /// </summary>
        [JsonProperty("UDDOSAGE")]
        public string UDDOSAGE { get; set; }

        /// <summary>
        /// 就診號
        /// </summary>
        [JsonProperty("ENCNTNO")]
        public string ENCNTNO { get; set; }

        /// <summary>
        /// 病歷號
        /// </summary>
        [JsonProperty("HHISNUM")]
        public string HHISNUM { get; set; }

        /// <summary>
        /// 藥碼
        /// </summary>
        [JsonProperty("UDDRGNO")]
        public string UDDRGNO { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        [JsonProperty("UDDDGNPRODUCT")]
        public string UDDDGNPRODUCT { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [JsonProperty("UDDURAT")]
        public string UDDURAT { get; set; }

        /// <summary>
        /// 副作用
        /// </summary>
        [JsonProperty("SIDEEFFECT")]
        public string SIDEEFFECT { get; set; }

        /// <summary>
        /// 途徑
        /// </summary>
        [JsonProperty("UDROUTE")]
        public string UDROUTE { get; set; }

        /// <summary>
        /// 頻次
        /// </summary>
        [JsonProperty("UDFREQN")]
        public string UDFREQN { get; set; }

        /// <summary>
        /// 指導內容
        /// </summary>
        [JsonProperty("INDICATION")]
        public string INDICATION { get; set; }

        /// <summary>
        /// 醫囑開立時間
        /// </summary>
        [JsonProperty("ORDDTTM")]
        public string ORDDTTM { get; set; }

        /// <summary>
        /// 醫囑備註
        /// </summary>
        [JsonProperty("UDOINSTRUCTION")]
        public string UDOINSTRUCTION { get; set; }

        /// <summary>
        /// 藥品學名
        /// </summary>
        [JsonProperty("UDDDGNMATERIAL")]
        public string UDDDGNMATERIAL { get; set; }

        /// <summary>
        /// 主診斷
        /// </summary>
        [JsonProperty("DIAGNOSIS")]
        public string DIAGNOSIS { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [JsonProperty("HBIRTHDT")]
        public string HBIRTHDT { get; set; }
    }
}
