using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBM.Data.DB2.Core;
using System.Data;
using System.Configuration;
using Basic;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using HIS_DB_Lib;
using System.Linq.Expressions;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DB2VM_API.Controller
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class BBCM : ControllerBase
    {
        static public string API_Server = "http://127.0.0.1:4433";


        [HttpGet]
        public string Get(string? stockcode)
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            returnData returnData = new returnData();
            try
            {
                List<medicineClass> medicineClasses = medicineClass.get_med(stockcode);
                List<medClass> medClasses = new List<medClass>();
                foreach (var medicineClass in medicineClasses)
                {
                    medClass medClass = new medClass
                    {
                        藥品碼 = medicineClass.藥品碼,
                        藥品名稱 = medicineClass.藥品名稱,
                        藥品學名 = medicineClass.藥品學名,
                        中文名稱 = medicineClass.中文名稱,
                        最小包裝單位 = medicineClass.最小包裝單位,
                        包裝單位 = medicineClass.最小包裝單位,
                        警訊藥品 = medicineClass.警訊藥品.ToString(),
                        管制級別 = medicineClass.管制級別.ToString(),
                        開檔狀態 = medicineClass.開檔狀態,
                        料號 = medicineClass.料號.Trim(),
                        ATC = medicineClass.healthInsurance.ATC,
                        中西藥 = "西藥"
                    };
                    if (medClass.管制級別 == "0") medClass.管制級別 = "N";
                    if (medClass.開檔狀態 == "N") medClass.開檔狀態 = "停用中";
                    if (medClass.開檔狀態 == "Y") medClass.開檔狀態 = "開檔中";
                    if (medClass.料號.StringIsEmpty() == false && medClass.藥品碼.StringIsEmpty() == false) medClasses.Add(medClass);

                }
                Dictionary<string, List<medClass>> medDict = medClass.CoverToDictionaryByCode(medClasses);
                List<medClass> temp_medClass = new List<medClass>();
                foreach (string key in medDict.Keys)
                {
                    List<medClass> med = medDict[key];
                    if (med.Count > 1)
                    {
                        List<medClass> buff_medClass = med.Where(temp => temp.開檔狀態 == "開檔中").ToList();
                        if (buff_medClass.Count == 0)
                        {
                            temp_medClass.Add(med[0]);
                        }
                        else
                        {
                            temp_medClass.Add(buff_medClass[0]);
                        }

                    }
                    else if (med.Count == 1)
                    {
                        temp_medClass.Add(med[0]);
                    }

                }
                medClass.add_med_clouds(API_Server, temp_medClass);


                returnData.Code = 200;
                returnData.Result = $"取得藥品資料共{medClasses.Count}筆";
                returnData.TimeTaken = $"{myTimerBasic}";
                returnData.Data = medClasses;
                return returnData.JsonSerializationt(true);
            }
            catch (Exception ex)
            {
                returnData.Code = -200;
                returnData.Result = ex.Message;
                return returnData.JsonSerializationt(true);
            }
        }

        public static string ConvertToBig5(string input)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // 將字串轉換為位元組陣列
            byte[] bytes = Encoding.Default.GetBytes(input);
            // 將位元組由預設編碼轉換為 BIG5 編碼
            byte[] big5Bytes = Encoding.Convert(Encoding.Default, Encoding.GetEncoding("BIG5"), bytes);
            // 取得轉換後的字串

            return Encoding.GetEncoding("BIG5").GetString(big5Bytes);
        }
    }
}
