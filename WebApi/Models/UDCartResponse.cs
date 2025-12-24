using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class UDCartResponse
    {
        /// <summary>
        /// 醫囑序號
        /// </summary>
        [JsonProperty("ORDSEQ")]
        public string ORDSEQ { get; set; }

        /// <summary>
        /// 藥品註記
        /// </summary>
        [JsonProperty("PREFIX")]
        public string PREFIX { get; set; }

        /// <summary>
        /// 藥碼
        /// </summary>
        [JsonProperty("UDODRGCODE")]
        public string UDODRGCODE { get; set; }

        /// <summary>
        /// 藥名
        /// </summary>
        [JsonProperty("DRUGNAME")]
        public string DRUGNAME { get; set; }

        /// <summary>
        /// 醫囑狀態
        /// </summary>
        [JsonProperty("STATUS")]
        public string STATUS { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [JsonProperty("CREATEDT")]
        public string CREATEDT { get; set; }

        /// <summary>
        /// 護理站
        /// </summary>
        [JsonProperty("STATIONNO")]
        public string STATIONNO { get; set; }

        /// <summary>
        /// 床號
        /// </summary>
        [JsonProperty("BEDNO")]
        public string BEDNO { get; set; }

        /// <summary>
        /// 一日量
        /// </summary>
        [JsonProperty("DAY1QTY")]
        public string DAY1QTY { get; set; }

        /// <summary>
        /// 二日量
        /// </summary>
        [JsonProperty("DAY2QTY")]
        public string DAY2QTY { get; set; }

        /// <summary>
        /// 三日量
        /// </summary>
        [JsonProperty("DAY3QTY")]
        public string DAY3QTY { get; set; }

        /// <summary>
        /// 四日量
        /// </summary>
        [JsonProperty("DAY4QTY")]
        public string DAY4QTY { get; set; }

        /// <summary>
        /// 一日調劑
        /// </summary>
        [JsonProperty("UD1")]
        public string UD1 { get; set; }

        /// <summary>
        /// 二日調劑
        /// </summary>
        [JsonProperty("UD2")]
        public string UD2 { get; set; }

        /// <summary>
        /// 三日調劑
        /// </summary>
        [JsonProperty("UD3")]
        public string UD3 { get; set; }

        /// <summary>
        /// 四日調劑
        /// </summary>
        [JsonProperty("UD4")]
        public string UD4 { get; set; }

        /// <summary>
        /// 核對一
        /// </summary>
        [JsonProperty("CHECKED1")]
        public string CHECKED1 { get; set; }

        /// <summary>
        /// 核對二
        /// </summary>
        [JsonProperty("CHECKED2")]
        public string CHECKED2 { get; set; }

        /// <summary>
        /// 核對三
        /// </summary>
        [JsonProperty("CHECKED3")]
        public string CHECKED3 { get; set; }

        /// <summary>
        /// 核對四
        /// </summary>
        [JsonProperty("CHECKED4")]
        public string CHECKED4 { get; set; }

        /// <summary>
        /// 取出註記
        /// </summary>
        [JsonProperty("DCCHECK")]
        public string DCCHECK { get; set; }

        /// <summary>
        /// 醫囑生效時間
        /// </summary>
        [JsonProperty("BEGINDATE")]
        public string BEGINDATE { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        [JsonProperty("MODTIP")]
        public string MODTIP { get; set; }

        /// <summary>
        /// 異動別
        /// </summary>
        [JsonProperty("FUNC")]
        public string FUNC { get; set; }

        /// <summary>
        /// 醫囑狀態
        /// </summary>
        [JsonProperty("ORDSTATUS")]
        public string ORDSTATUS { get; set; }

        /// <summary>
        /// 病歷號
        /// </summary>
        [JsonProperty("HHISTNUM")]
        public string HHISTNUM { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [JsonProperty("BILLDATE")]
        public string BILLDATE { get; set; }

        /// <summary>
        /// 資料批次  
        /// 1: 上午  
        /// 2: 下午  
        /// U: 醫囑異動
        /// </summary>
        [JsonProperty("MDATASEQ")]
        public string MDATASEQ { get; set; }

        /// <summary>
        /// 資料批次  
        /// 1: 上午  
        /// 2: 下午  
        /// U: 醫囑異動
        /// </summary>
        [JsonProperty("DDATASEQ")]
        public string DDATASEQ { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        [JsonProperty("SERNO")]
        public string SERNO { get; set; }

        /// <summary>
        /// 劑量
        /// </summary>
        [JsonProperty("DOSAGE")]
        public string DOSAGE { get; set; }

        /// <summary>
        /// 單位
        /// </summary>
        [JsonProperty("DOSAGEUNIT")]
        public string DOSAGEUNIT { get; set; }

        /// <summary>
        /// 途徑
        /// </summary>
        [JsonProperty("USINGWAY")]
        public string USINGWAY { get; set; }

        /// <summary>
        /// 頻次
        /// </summary>
        [JsonProperty("FREQUENCY")]
        public string FREQUENCY { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("HNAMEC")]
        public string HNAMEC { get; set; }

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
        /// 總數量
        /// </summary>
        [JsonProperty("TATAL_QTY")]
        public string TATAL_QTY { get; set; }

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
