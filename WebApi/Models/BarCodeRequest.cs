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
}
