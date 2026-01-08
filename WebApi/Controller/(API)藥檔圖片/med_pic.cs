using Basic;
using DBVM_API.Services;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SQLUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DB2VM_API.Controller._API_藥檔圖片
{
    [Route("api/[controller]")]
    [ApiController]
    public class med_pic : ControllerBase
    {
        static private string API_Server = "http://127.0.0.1:4433";
        static private MySqlSslMode SSLMode = MySqlSslMode.None;

        private readonly HospitalApiService _hospitalApi;
        public med_pic(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
            API_Server = hospitalApi.API_Server;
        }

        [HttpGet]
        public string get()
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            returnData returnData = new returnData();
            try
            {
        
                List<medClass> medClasses = medClass.get_med_cloud(API_Server);       
                List<medPicClass> medPicClasses = new List<medPicClass>();
                
                ConcurrentBag<medPicClass> localList = new ConcurrentBag<medPicClass>();

                Parallel.ForEach(medClasses, new ParallelOptions { MaxDegreeOfParallelism = 4 }, medClass =>
                {
                    string 藥碼 = medClass.藥品碼;
                    string 藥名 = medClass.藥品名稱;
                    medPictureClass medPictureClass = medPictureClass.get_pic(藥碼);
                    if (medPictureClass != null && medPictureClass.pic_base64.StringIsEmpty() == false)
                    {
                        medPicClass medPicClass = new medPicClass
                        {
                            藥碼 = 藥碼,
                            藥名 = 藥名,
                            副檔名 = "jpg",
                            pic_base64 = medPictureClass.pic_base64,
                        };
                        localList.Add(medPicClass);
                    }
                });
                lock (medPicClasses)
                {
                    medPicClasses.AddRange(localList);
                }
             
                //medPicClass.add(API_Server, medPicClasses);
                for (int i = 0; i < medPicClasses.Count; i++)
                {
                    medPicClass.add(API_Server, medPicClasses[i]);
                }

                returnData.Code = 200;
                returnData.Result = $"新增<{medPicClasses.Count}>筆圖片";
                returnData.TimeTaken = $"{myTimerBasic}";
                //returnData.Data = medPicClasses;
                return returnData.JsonSerializationt(true);
            }
            catch(Exception ex)
            {
                returnData.Code = -200;
                returnData.Result = ex.Message;
                return returnData.JsonSerializationt(true);
            }
        }

        [HttpGet("test")]
        public async Task<IActionResult> test([FromQuery] string code)
        {            
            medPictureClass medPictureClass = medPictureClass.get_pic(code);

            //string url = $"https://www3.vghtc.gov.tw:8443/pharmacyHandbook/API/getImage.jsp?path=pic&code={code}";

            //ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.GetAsync(url);
            //response.EnsureSuccessStatusCode();
            //byte[] jpegBytes = await response.Content.ReadAsByteArrayAsync();
            //string base64 = Convert.ToBase64String(jpegBytes);
            //byte[] imageBytes = Convert.FromBase64String(base64);

            byte[] imageBytes = Convert.FromBase64String(medPictureClass.pic_base64);

            return File(imageBytes, "image/jpeg");
        }
    }
}
