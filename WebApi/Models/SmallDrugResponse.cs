using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class SmallDrugResponse
    {
        /// <summary>
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
        /// 數量
        /// </summary>
        [JsonProperty("QUANTITY")]
        public string QUANTITY { get; set; }

        /// <summary>
        /// 領藥號
        /// </summary>
        [JsonProperty("DISPNO")]
        public string DISPNO { get; set; }

        /// <summary>
        /// 單位
        /// </summary>
        [JsonProperty("UDOGIVUNIT")]
        public string UDOGIVUNIT { get; set; }

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
        /// 床號
        /// </summary>
        [JsonProperty("HBEDNO")]
        public string HBEDNO { get; set; }

        /// <summary>
        /// 劑量
        /// </summary>
        [JsonProperty("UDOGIVDOSE")]
        public string UDOGIVDOSE { get; set; }

        /// <summary>
        /// 就診號
        /// </summary>
        [JsonProperty("ENCNTNO")]
        public string ENCNTNO { get; set; }

        /// <summary>
        /// 病歷號
        /// </summary>
        [JsonProperty("HHISTNUM")]
        public string HHISTNUM { get; set; }

        /// <summary>
        /// 藥碼
        /// </summary>
        [JsonProperty("UDDDRGCODE")]
        public string UDDDRGCODE { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        [JsonProperty("UDDDGNPRODUCT")]
        public string UDDDGNPRODUCT { get; set; }

        /// <summary>
        /// 劑量單位
        /// </summary>
        [JsonProperty("UDDDSPUNIT")]
        public string UDDDSPUNIT { get; set; }

        /// <summary>
        /// 頻次
        /// </summary>
        [JsonProperty("UDOGIVFREQN")]
        public string UDOGIVFREQN { get; set; }

        /// <summary>
        /// 副作用
        /// </summary>
        [JsonProperty("SIDEEFFECT")]
        public string SIDEEFFECT { get; set; }

        /// <summary>
        /// 途徑
        /// </summary>
        [JsonProperty("UDDROUTE")]
        public string UDDROUTE { get; set; }

        /// <summary>
        /// 指導內容
        /// </summary>
        [JsonProperty("INDICATION")]
        public string INDICATION { get; set; }

        /// <summary>
        /// 印表機號
        /// </summary>
        [JsonProperty("PRINTER")]
        public string PRINTER { get; set; }

        /// <summary>
        /// 藥袋條碼
        /// </summary>
        [JsonProperty("UDBC")]
        public string UDBC { get; set; }

        /// <summary>
        /// 醫囑類別
        /// </summary>
        [JsonProperty("UDOFUNCT")]
        public string UDOFUNCT { get; set; }

        /// <summary>
        /// 藥品學名
        /// </summary>
        [JsonProperty("UDDDGNMATERIAL")]
        public string UDDDGNMATERIAL { get; set; }

        /// <summary>
        /// 科別
        /// </summary>
        [JsonProperty("SECT")]
        public string SECT { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [JsonProperty("HBIRTHDT")]
        public string HBIRTHDT { get; set; }

        /// <summary>
        /// 住院日
        /// </summary>
        [JsonProperty("INDATE")]
        public string INDATE { get; set; }

        /// <summary>
        /// 主診斷
        /// </summary>
        [JsonProperty("DIAGNOSIS")]
        public string DIAGNOSIS { get; set; }
    }
}
