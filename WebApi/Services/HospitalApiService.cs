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
        public readonly string API_Server = "http://192.168.5.200:4450";
        public HospitalApiService()
        {
            _client = new HttpClient(_httpClientHandler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            // 院方API Key
            _client.DefaultRequestHeaders.Add("X-API-KEY", "9684FF26-7032-4574-AFE1-F0D5C4320E3B");
        }


        /// <summary>
        /// 根據時間區間與註記條件取得小藥袋配方機資料
        /// </summary>
        /// <param name="request">根據時間區間與註記條件取得小藥袋配方機資料</param>
        /// <returns></returns>
        public Task<(bool Success, List<SmallDrugResponse> Data, string ErrorMessage)> GetSmallDrug(SmallDrugRequest request)
        {
            var url = $"{_baseUrl}/smalldrug";
            var response = PostAndDeserializeAsync<List<SmallDrugResponse>>(url, request);
            return response;
        }

        /// <summary>
        /// 根據條碼取得小藥袋配方機資料
        /// </summary>
        /// <param name="barcode">BarCode</param>
        /// <returns></returns>
        public Task<(bool Success, SmallDrugResponse Data, string ErrorMessage)> GetSmallDrugByBarcode(BarCodeRequest barcode)
        {
            var url = $"{_baseUrl}/smalldrug/barcode";
            var response = PostAndDeserializeAsync<SmallDrugResponse>(url, barcode);
            return response;
        }

        /// <summary>
        /// 取得出院帶藥/日間帶藥配方機資料
        /// </summary>
        /// <param name="request">根據時間區間與註記條件取得出院帶藥/日間帶藥配方機資料</param>
        /// <returns></returns>
        public Task<(bool Success, List<TakeDrugResponse> Data, string ErrorMessage)> GetTakeDrug(TakeDrugRequest request)
        {
            var url = $"{_baseUrl}/takedrug";
            var response = PostAndDeserializeAsync<List<TakeDrugResponse>>(url, request);
            return response;
        }

        /// <summary>
        /// 根據條碼取得出院帶藥/日間帶藥配方機資料
        /// </summary>
        /// <param name="request">根據時間區間與註記條件取得出院帶藥/日間帶藥配方機資料</param>
        /// <returns></returns>
        public Task<(bool Success, List<TakeDrugResponse> Data, string ErrorMessage)> GetTakeDrugByBarCode(BardCodeTakeDrugRequest request)
        {
            var url = $"{_baseUrl}/takedrug/barcode";
            var response = PostAndDeserializeAsync<List<TakeDrugResponse>>(url, request);
            return response;
        }

        /// <summary>
        /// 回寫註記欄位
        /// </summary>
        /// <param name="request">根據主鍵編號回寫註記欄位</param>
        /// <returns></returns>
        public Task<(bool Success, RemarkStatusResponse Data, string ErrorMessage)> RemarkStatus(RemarkStatusRequest request)
        {
            var url = $"{_baseUrl}/remarkstatus";
            var response = PutAndDeserializeAsync<RemarkStatusResponse>(url, request);
            return response;
        }

        /// <summary>
        /// 潘朵拉資料轉換
        /// </summary>
        /// <param name="request">同步院內庫存與紀錄</param>
        /// <returns></returns>
        public Task<(bool Success, PandoraResponse Data, string ErrorMessage)> Pandora(PandoraRequest request)
        {
            var url = $"{_baseUrl}/pandora";
            var response = PostAndDeserializeAsync<PandoraResponse>(url, request);
            return response;
        }

        /// <summary>
        /// UD藥車加強點收調劑項目
        /// </summary>
        /// <param name="time">調劑時間</param>
        /// <param name="pharmacy">藥局位置</param>
        /// <returns></returns>
        public Task<(bool Success, List<UDCartResponse> Data, string ErrorMessage)> GetUDCartInfo(enum_調劑時間 time, enum_藥局位置 pharmacy)
        {
            var url = $"{_baseUrl}/UDCart/{time.GetDescription()}/{pharmacy.GetDescription()}";
            var response = GetAndDeserializeAsync<List<UDCartResponse>>(url);
            return response;
        }

        /// <summary>
        /// 每日補公清單 取得管制藥每日補公清單
        /// </summary>
        /// <param name="pharmacy">藥局位置</param>
        /// <returns></returns>
        public Task<(bool Success, List<ControlDrugResponse> Data, string ErrorMessage)> GetControlDrugInfoList(enum_藥局位置 pharmacy)
        {
            var url = $"{_baseUrl}/controlledDrug/{pharmacy.GetDescription()}";
            var response = GetAndDeserializeAsync<List<ControlDrugResponse>>(url);
            return response;
        }

        /// <summary>
        /// 管制藥借還 根據條碼取得管制藥借還資料
        /// </summary>
        /// <param name="barcode">BarCode</param>
        /// <returns></returns>
        public Task<(bool Success, List<ControlDrugLendReturnResponse> Data, string ErrorMessage)> GetControlDrugInfoList(string barcode)
        {
            var url = $"{_baseUrl}/controlledDrugLendReturn/{barcode}";
            var response = GetAndDeserializeAsync<List<ControlDrugLendReturnResponse>>(url);
            return response;
        }

        /// <summary>
        /// 常日配方機  取得常日配方機資料
        /// </summary>
        /// <param name="stations">欲接收護理站，使用逗號隔開。例：W105,W102</param>
        /// <returns></returns>
        public Task<(bool Success, List<NormalDrugResponse> Data, string ErrorMessage)> GetNormalDrugInfo(string stations)
        {
            var url = $"{_baseUrl}/normalDrugMachine/{stations}";
            var response = GetAndDeserializeAsync<List<NormalDrugResponse>>(url);
            return response;
        }

        /// <summary>
        /// 智慧藥櫃品項  取得對應藥局智慧藥櫃品項
        /// </summary>
        /// <param name="position">藥局位置</param>
        /// <returns></returns>
        public Task<(bool Success, List<SmartMedCabinetResponse> Data, string ErrorMessage)> GetSmartMedCabinet(enum_藥局位置 position)
        {
            var url = $"{_baseUrl}/normalDrugMachine/{position.GetDescription()}";
            var response = GetAndDeserializeAsync<List<SmartMedCabinetResponse>>(url);
            return response;
        }

        /// <summary>
        /// 取得卡片使用者清單
        /// </summary>
        /// <returns></returns>
        public Task<(bool Success, List<CardUserResponse> Data, string ErrorMessage)> GetCardUsers()
        {
            var url = $"{_baseUrl}/pharmacist";
            var response = GetAndDeserializeAsync<List<CardUserResponse>>(url);
            return response;
        }

        /// <summary>
        /// 藥師登入
        /// </summary>
        /// <param name="request">登入項目</param>
        /// <returns></returns>
        public Task<(bool Success, PharmacistLoginResponse Data, string ErrorMessage)> PharmacistLogin(PharmacistLoginRequest request)
        {
            var url = $"{_baseUrl}/pharmacistLogin";
            var response = PostAndDeserializeAsync<PharmacistLoginResponse>(url, request);
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
