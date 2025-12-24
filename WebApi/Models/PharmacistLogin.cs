using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class PharmacistLoginRequest
    {
        /// <summary>
        /// 卡號（僅在 Auth 登入時使用，NFC 登入不會有此欄位）
        /// </summary>
        [JsonProperty("card", NullValueHandling = NullValueHandling.Ignore)]
        public string Card { get; set; }

        /// <summary>
        /// 密碼（僅在 Auth 登入時使用，NFC 登入不會有此欄位）
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary>
        /// 卡片識別碼（僅在 NFC 登入時使用，Auth 登入不會有此欄位）
        /// </summary>
        [JsonProperty("nfc", NullValueHandling = NullValueHandling.Ignore)]
        public string NFC { get; set; }

        /// <summary>
        /// 登入類型  
        /// Auth: 卡號密碼登入  
        /// NFC: 識別證登入
        /// </summary>
        [JsonProperty("loginType")]
        public string LoginType { get; set; }
    }

    public class PharmacistLoginResponse
    {
        /// <summary>
        /// 卡片識別碼
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
