using Basic;
using HIS_DB_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ubiety.Dns.Core;

namespace DB2VM_API
{
    public class medPictureClass
    {
        [JsonPropertyName("imageBase64")]
        public string pic_base64 { get; set; }
        [JsonPropertyName("code")]
        public string 藥碼 { get; set; }
        [JsonPropertyName("fullName")]
        public string 藥名 { get; set; }

        /// <summary>
        /// 取得藥品圖片檔
        /// </summary>
        /// <param name="code">藥碼</param>
        /// <returns>藥檔圖片物件</returns>
        static public medPictureClass get_pic(string code)
        {
            // 藥碼取得藥品圖片(中榮藥碼圖片連結)  得到的是JPEG 二進制資料 非Base64
            string url = $"https://www3.vghtc.gov.tw:8443/pharmacyHandbook/API/getImage.jsp?path=pic&code={code}";
            medPictureClass medPicClass = new medPictureClass();
            string json_out = WEBApiGet(url);

            medPicClass.藥碼 = code;
            medPicClass.pic_base64 = json_out;
            return medPicClass;
        }

        /// <summary>
        /// 擷取圖檔連結 (包成同步)
        /// </summary>
        /// <param name="url">圖檔連結</param>
        /// <returns>Base64字串</returns>
        static string WEBApiGet(string url)
        {
            return Task.Run(async () => await WEBApiGetAsync(url)).Result;
        }

        /// <summary>
        /// 擷取圖檔連結 (非同步)
        /// </summary>
        /// <param name="url">圖檔連結</param>
        /// <returns>Base64字串</returns>
        static async Task<string> WEBApiGetAsync(string url)
        {
            string responseBody = "";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                responseBody = Convert.ToBase64String(imageBytes);
                return responseBody;
            }
            catch
            {
                return responseBody;
            }
        }
    }
}
