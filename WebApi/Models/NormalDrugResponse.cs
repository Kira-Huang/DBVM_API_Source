using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class NormalDrugResponse
    {
        /// <summary>
        /// 醫囑序號
        /// </summary>
        [JsonProperty("ORDSEQ")]
        public string ORDSEQ { get; set; }

        /// <summary>
        /// 藥名
        /// </summary>
        [JsonProperty("ORDPROCED")]
        public string ORDPROCED { get; set; }

        /// <summary>
        /// 生效時間
        /// </summary>
        [JsonProperty("ORDBGNDTTM")]
        public string ORDBGNDTTM { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        [JsonProperty("ORDENDDTTM")]
        public string ORDENDDTTM { get; set; }

        /// <summary>
        /// 藥碼
        /// </summary>
        [JsonProperty("UDODRGCODE")]
        public string UDODRGCODE { get; set; }

        /// <summary>
        /// 劑量
        /// </summary>
        [JsonProperty("UDOGIVDOSE")]
        public string UDOGIVDOSE { get; set; }

        /// <summary>
        /// 單位
        /// </summary>
        [JsonProperty("UDOGIVUNIT")]
        public string UDOGIVUNIT { get; set; }

        /// <summary>
        /// 頻次
        /// </summary>
        [JsonProperty("UDOGIVFREQN")]
        public string UDOGIVFREQN { get; set; }

        /// <summary>
        /// 途徑
        /// </summary>
        [JsonProperty("UDOGIVROUTE")]
        public string UDOGIVROUTE { get; set; }

        /// <summary>
        /// 病歷號
        /// </summary>
        [JsonProperty("HHISTNUM")]
        public string HHISTNUM { get; set; }

        /// <summary>
        /// 就診號
        /// </summary>
        [JsonProperty("ENCNTNO")]
        public string ENCNTNO { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        [JsonProperty("HNAMEC")]
        public string HNAMEC { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [JsonProperty("HBIRTHDT")]
        public string HBIRTHDT { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        [JsonProperty("HSEX")]
        public string HSEX { get; set; }

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
        /// 數量
        /// </summary>
        [JsonProperty("QNAUTITY")]
        public int QNAUTITY { get; set; }
    }
}
