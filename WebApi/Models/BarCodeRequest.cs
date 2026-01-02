using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class BarCodeRequest
    {
        /// <summary>
        /// Barcode
        /// </summary>
        [JsonProperty("barcode")]
        public string BarCode { get; set; }
    }

    public class BardCodeTakeDrugRequest
    {
        /// <summary>
        /// 藥袋條碼，支援 一維 / 二維條碼
        /// </summary>
        [JsonProperty("barcode")]
        public string BarCode { get; set; }

        /// <summary>
        /// discharge = 出院帶藥 /daytime = 日間帶藥
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// 藥局位置 PHR = 中央藥局 /PHRO = 門診藥局 / PHRE = 急診藥局 / PHR6 = 二醫藥局 /PHRN = 北院區藥局
        /// </summary>
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }
    }
}
