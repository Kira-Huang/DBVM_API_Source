using Newtonsoft.Json;

namespace DBVM_API.Models
{
    public class RemarkStatusRequest
    {
        /// <summary>
        /// 使用 - , 分隔的字串
        /// </summary>
        [JsonProperty("ID")]
        public string ID { get; set; }
    }

    public class RemarkStatusResponse
    {
        /// <summary>
        /// 回傳狀態說明  ex: 註記讀取資料成功 → 共 <> 筆
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
