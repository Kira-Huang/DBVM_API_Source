using DBVM_API.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Common;

namespace DBVM_API.Services
{
    public class HospitalApiService
    {
        private readonly HttpClient _client;
        private readonly HttpClientHandler _httpClientHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };

        public HospitalApiService()
        {
            _client = new HttpClient(_httpClientHandler)
            {
                BaseAddress = new Uri("http://billingweb.vghb12.vhyl.gov.tw/PharmacySpring/company/"),
                Timeout = TimeSpan.FromSeconds(10)
            };

            // 院方API Key
            _client.DefaultRequestHeaders.Add("X-API-KEY", "9684FF26-7032-4574-AFE1-F0D5C4320E3B");
        }

        /// <summary>
        /// 根據條碼取得小藥袋配方機資料
        /// </summary>
        /// <param name="barcode">BarCode</param>
        /// <returns></returns>
        public async Task<SmallDrugResponse> GetSmallDrugByBarcode(BarCodeRequest barcode)
        {
            var json = JsonConvert.SerializeObject(barcode);
            using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            using var response = await _client.PostAsync("smalldrug/barcode", content);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<SmallDrugResponse>(jsonResponse);
        }
    }
}
