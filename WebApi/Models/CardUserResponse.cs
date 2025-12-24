using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class CardUserResponse
    {
        /// <summary>
        /// 卡片編號
        /// </summary>
        [JsonProperty("SN")]
        public string SN { get; set; }

        /// <summary>
        /// 卡號
        /// </summary>
        [JsonProperty("CARDNO")]
        public string CARDNO { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("NAME")]
        public string NAME { get; set; }

        /// <summary>
        /// 卡號識別碼
        /// </summary>
        [JsonProperty("NUM")]
        public string NUM { get; set; }
    }
}
