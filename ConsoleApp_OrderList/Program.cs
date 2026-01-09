using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp_OrderList
{
    class Program
    {
        static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(20)
        };
        static string _baseUrl = "http://192.168.5.200:4434";

        static async Task Main(string[] args)
        {
            Mutex mutex = new Mutex(true, "Orderlist_Mutex", out bool created);
            if (!created)
            {
                Console.WriteLine("已有程序執行，結束本次執行");
                return;
            }

            DateTime today = DateTime.Now;

            string startDate = today.ToString("yyyyMMdd") + "000001";
            string endDate = today.ToString("yyyyMMdd") + "235959";

            string url = $"{_baseUrl}" + $"/dbvm/Orders" +
                         $"?startDate={startDate}&endDate={endDate}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"API: {url} 呼叫成功");
                Console.WriteLine(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API: {url}: {ex.Message}");
            }
        }
    }
}
