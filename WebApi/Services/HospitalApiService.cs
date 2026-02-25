using DBVM_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Common;

namespace DBVM_API.Services
{
    public class HospitalApiService
    {
        private readonly string _baseUrl = "http://billingweb.vghb12.vhyl.gov.tw/PharmacySpring/company";
        private readonly HttpClient _client;
        private readonly HttpClientHandler _httpClientHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };
        public readonly string API_Server = "http://192.168.5.200:4433";
        public HospitalApiService()
        {
            _client = new HttpClient(_httpClientHandler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            // 院方API Key
            _client.DefaultRequestHeaders.Add("X-KEY", "XQIH4E1XjdsX0zNdCPH7QXs8EKeQhn7w");
        }

        /// <summary>
        /// 取得醫囑藥物品項清單，以特定病人與藥號條件查詢。
        /// </summary>
        /// <param name="request">住院序號,類別,領藥號,BarCode</param>
        /// <returns></returns>
        public Task<(bool Success, MedicationResponse Data, string ErrorMessage)> GetFHIRMedicaionRequests(MedicationRequest request)
        {
            var url = $"{_baseUrl}/System/WS/FHIR_MedicationRequests.asmx/GetMedicationRequests";
            var response = PostAndDeserializeAsync<MedicationResponse>(url, request);
            return response;
        }

        #region --- HTTP 工具方法 ---

        /// <summary>
        /// HTTP Get 並反序列化
        /// </summary>
        /// <typeparam name="T">物件</typeparam>
        /// <param name="url">API URL</param>
        /// <returns>結果、資料、錯誤訊息</returns>
        private async Task<(bool Success, T Data, string ErrorMessage)> GetAndDeserializeAsync<T>(string url)
        {
            var result = await GetAsync(url);
            if (!result.Success)
                return (false, default, result.ErrorMessage);

            var data = JsonConvert.DeserializeObject<T>(result.JsonResponse);
            return (true, data, null);
        }

        /// <summary>
        /// HTTP Get 方法
        /// </summary>
        /// <param name="url">API URL</param>
        /// <returns>結果、JSON回覆、錯誤訊息</returns>
        private async Task<(bool Success, string JsonResponse, string ErrorMessage)> GetAsync(string url)
        {
            try
            {
                using var response = await _client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return (false, null, "API 回傳 404 Not Found");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                return (true, jsonResponse, null);
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                return (false, null, "連線逾時，請稍後再試");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"HTTP 錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"系統錯誤: {ex.Message}");
            }
        }

        /// <summary>
        /// HTTP Post 並反序列化
        /// </summary>
        /// <typeparam name="T">泛型物件</typeparam>
        /// <param name="url">API URL</param>
        /// <param name="payload">物件</param>
        /// <returns>結果、資料、錯誤訊息</returns>
        private async Task<(bool Success, T Data, string ErrorMessage)> PostAndDeserializeAsync<T>(string url, object payload)
        {
            var result = await PostAsync(url, payload);
            if (!result.Success) 
                return (false, default, result.ErrorMessage);

            var data = JsonConvert.DeserializeObject<T>(result.JsonResponse);
            return (true, data, null);
        }

        /// <summary>
        /// Http Post 方法
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="payload">物件</param>
        /// <returns>結果、JSON回覆、錯誤訊息</returns>
        private async Task<(bool Success, string JsonResponse, string ErrorMessage)> PostAsync(string url, object payload)
        {
            try
            {
                var json = JsonConvert.SerializeObject(payload);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var response = await _client.PostAsync(url, content);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return (false, null, "API 回傳 404 Not Found");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return (true, jsonResponse, null);
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                return (false, null, "連線逾時，請稍後再試");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"HTTP 錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"系統錯誤: {ex.Message}");
            }
        }

        /// <summary>
        /// HTTP Put 並反序列化
        /// </summary>
        /// <typeparam name="T">泛型物件</typeparam>
        /// <param name="url">API URL</param>
        /// <param name="payload">物件</param>
        /// <returns>結果、資料、錯誤訊息</returns>
        private async Task<(bool Success, T Data, string ErrorMessage)> PutAndDeserializeAsync<T>(string url, object payload)
        {
            var result = await PutAsync(url, payload);
            if (!result.Success)
                return (false, default, result.ErrorMessage);

            var data = JsonConvert.DeserializeObject<T>(result.JsonResponse);
            return (true, data, null);
        }

        /// <summary>
        /// Http Put 方法
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="payload">物件</param>
        /// <returns>結果、JSON回覆、錯誤訊息</returns>
        private async Task<(bool Success, string JsonResponse, string ErrorMessage)> PutAsync(string url, object payload)
        {
            try
            {
                var json = JsonConvert.SerializeObject(payload);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var response = await _client.PutAsync(url, content);

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return (false, null, "API 回傳 404 Not Found");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
                return (true, jsonResponse, null);
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                return (false, null, "連線逾時，請稍後再試");
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"HTTP 錯誤: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"系統錯誤: {ex.Message}");
            }
        }

        #endregion

        #region --- 舊版方法 (保留備用) ---

        ///// <summary>
        ///// 根據條碼取得小藥袋配方機資料
        ///// </summary>
        ///// <param name="barcode">BarCode</param>
        ///// <returns></returns>
        //public async Task<SmallDrugResponse> GetSmallDrugByBarcode(BarCodeRequest barcode)
        //{
        //    var url = $"{_baseUrl}/smalldrug/barcode";

        //    var json = JsonConvert.SerializeObject(barcode);
        //    using var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //    using var response = await _client.PostAsync(url, content);

        //    if (response.StatusCode == HttpStatusCode.NotFound)
        //        return null;

        //    var jsonResponse = await response.Content.ReadAsStringAsync();
        //    response.EnsureSuccessStatusCode();

        //    return JsonConvert.DeserializeObject<SmallDrugResponse>(jsonResponse);
        //}

        ///// <summary>
        ///// 根據時間區間與註記條件取得小藥袋配方機資料
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<SmallDrugResponse>> GetSmallDrug(SmallDrugRequest request)
        //{
        //    var url = $"{_baseUrl}/smalldrug";

        //    var json = JsonConvert.SerializeObject(request);
        //    using var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    using var response = await _client.PostAsync(url, content);

        //    if (response.StatusCode == HttpStatusCode.NotFound)
        //        return null;

        //    var jsonResponse = await response.Content.ReadAsStringAsync();
        //    response.EnsureSuccessStatusCode();

        //    return JsonConvert.DeserializeObject<List<SmallDrugResponse>>(jsonResponse);
        //}

        #endregion
    }
}
