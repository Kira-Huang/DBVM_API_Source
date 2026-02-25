using Newtonsoft.Json;
using System.Collections.Generic;

namespace DBVM_API.Models
{
    public class MedicationResponse
    {
        /// <summary>
        /// 資源類型，固定為 "Bundle"
        /// </summary>
        [JsonProperty("resourceType")]
        public string ResourceType { get; set; }

        /// <summary>
        /// 類型，固定為 "searchset"
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 結果總筆數
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// MedicationRequest 資源陣列
        /// </summary>
        [JsonProperty("entry")]
        public List<MedicationEntry> Entry { get; set; }
    }

    public class MedicationEntry
    {
        /// <summary>
        /// 資源類型，固定為 "Bundle"
        /// </summary>
        [JsonProperty("resourceType")]
        public string ResourceType { get; set; }

        /// <summary>
        /// 流水序號
        /// </summary>
        [JsonProperty("serialNo")]
        public int SerialNo { get; set; }

        /// <summary>
        /// 住院序號
        /// </summary>
        [JsonProperty("inhospitalNum")]
        public int InhospitalNum { get; set; }

        /// <summary>
        /// 醫囑序號
        /// </summary>
        [JsonProperty("serialNum")]
        public int SerialNum { get; set; }

        /// <summary>
        /// 就診日期，格式為 "YYYYMMDD"
        /// </summary>
        [JsonProperty("visitDate")]
        public string VisitDate { get; set; }

        /// <summary>
        /// 處方序號
        /// </summary>
        [JsonProperty("presNum")]
        public int PresNum { get; set; }

        /// <summary>
        /// 藥品代碼
        /// </summary>
        [JsonProperty("drugId")]
        public string DrugId { get; set; }

        /// <summary>
        /// 藥品標記
        /// </summary>
        [JsonProperty("drugFlag")]
        public string DrugFlag { get; set; }

        /// <summary>
        /// 用法說明
        /// </summary>
        [JsonProperty("sig")]
        public string Sig { get; set; }

        /// <summary>
        /// 首次數量
        /// </summary>
        [JsonProperty("firstQty")]
        public int FirstQty { get; set; }

        /// <summary>
        /// 藥品類型
        /// </summary>
        [JsonProperty("medType")]
        public string MedType { get; set; }

        /// <summary>
        /// 領藥號
        /// </summary>
        [JsonProperty("bagNum")]
        public int BagNum { get; set; }

        /// <summary>
        /// 領藥列印標記(Y/N)
        /// </summary>
        [JsonProperty("bagPrintFlag")]
        public string BagPrintFlag { get; set; }
    }
}
