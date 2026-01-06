using System;
using Basic;
using Oracle.ManagedDataAccess.Client;

namespace ConsoleApp_medPageCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("藥檔更新開始");
            string url = "http://127.0.0.1:4433/api/BBCM";
            string json = Basic.Net.WEBApiGet(url);
            Console.WriteLine("藥檔更新結束");
            Console.WriteLine("藥品圖片更新開始");
            url = "http://127.0.0.1:4433/api/med_pic";
            json = Basic.Net.WEBApiGet(url);
            Console.WriteLine("藥品圖片更新結束");
        }
    }
}
