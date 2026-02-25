using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class MedicationRequest
    {
        /// <summary>
        /// 住院序號
        /// </summary>
        [JsonProperty("inhospitalNum")]
        public int InhospitalNum { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        [JsonProperty("medType")]
        public string MedType { get; set; }

        /// <summary>
        /// 領藥號
        /// </summary>
        [JsonProperty("bagNum")]
        public int BagNum { get; set; }

        /// <summary>
        /// 掃描條碼內容
        /// </summary>
        [JsonProperty("barcode")]
        public string Barcode { get; set; }
    }
}
