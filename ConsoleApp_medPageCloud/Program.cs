using System;
using Basic;
using Oracle.ManagedDataAccess.Client;

namespace ConsoleApp_medPageCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = string.Empty;
            string json = string.Empty;

            //Console.WriteLine("藥檔更新開始");
            //url = "http://192.168.5.200:4434/api/BBCM";
            //json = Basic.Net.WEBApiGet(url);
            //Console.WriteLine("藥檔更新結束");
            Console.WriteLine("藥品圖片更新開始");
            url = "http://192.168.5.200:4434/api/med_pic";
            json = Basic.Net.WEBApiGet(url);
            Console.WriteLine("藥品圖片更新結束");
        }
    }
}
