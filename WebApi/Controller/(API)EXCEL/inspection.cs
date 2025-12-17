using Basic;
using Google.Protobuf.WellKnownTypes;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyOffice;
using MySql.Data.MySqlClient;
using SQLUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DBVM_API.Controller._API_EXCEL
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class inspection : ControllerBase
    {
        [HttpPost("excel_upload_extra")]
        public async Task<string> excel_upload_extra([FromForm] IFormFile file)
        {
            var formFile = Request.Form.Files.FirstOrDefault();

            if (formFile == null)
            {
                throw new Exception("文件不能為空");
            }
            string extension = Path.GetExtension(formFile.FileName); // 获取文件的扩展名
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            List<medClass> medClasses = medClass.get_med_cloud("http://127.0.0.1:4433");
            List<medClass> medClasses_buf = new List<medClass>();

            string json = "";
            List<inspectionClass.content> contents = new List<inspectionClass.content>();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                System.Data.DataTable dt = ExcelClass.NPOI_LoadFile(memoryStream.ToArray(), extension);
                List<object[]> list_value = dt.DataTableToRowList();

                for (int i = 0; i < list_value.Count; i++)
                {
                    inspectionClass.content content = new inspectionClass.content();
                    content.藥品碼 = list_value[i][(int)enum_驗收單匯入北市聯.料品代碼].ObjectToString();
                    medClasses_buf = (from temp in medClasses
                                      where (temp.藥品碼 == content.藥品碼 || temp.料號 == content.藥品碼)
                                      select temp).ToList();
                    medClasses_buf = medClasses_buf.Where(temp => temp.開檔狀態.Contains("關檔中") == false).ToList();

                    if (medClasses_buf.Count > 0)
                    {
                        content.藥品名稱 = medClasses_buf[0].藥品名稱;
                        content.藥品碼 = medClasses_buf[0].藥品碼;
                        content.料號 = medClasses_buf[0].料號;

                    }
                    else
                    {
                        content.藥品名稱 = list_value[i][(int)enum_驗收單匯入北市聯.料品名稱].ObjectToString();
                        content.藥品碼 = "無";
                        content.料號 = "無";
                    }
                    //content.藥品名稱 = list_value[i][(int)enum_驗收單匯入北市聯.名稱].ObjectToString();
                    string 交貨期限 = list_value[i][(int)enum_驗收單匯入北市聯.交貨期限].ObjectToString();
                    string 交貨期限_ = string.Empty;
                    if (交貨期限.Length == 7)
                    {
                        string year = (交貨期限.Substring(0, 3).StringToInt32() + 1911).ToString();
                        string month = 交貨期限.Substring(3, 2);
                        string day = 交貨期限.Substring(5, 2);
                        交貨期限_ = $"{year}-{month}-{day} 00:00:00";
                    }
                    string 訂單時間 = list_value[i][(int)enum_驗收單匯入北市聯.訂單傳送日].ObjectToString();
                    string 訂單時間_ = string.Empty;
                    if (訂單時間.Length == 12)
                    {
                        string year = (訂單時間.Substring(0, 3).StringToInt32() + 1911).ToString();
                        string month = 訂單時間.Substring(3, 2);
                        string day = 訂單時間.Substring(5, 2);
                        string HH = 訂單時間.Substring(8, 2);
                        string mm = 訂單時間.Substring(10, 2);

                        訂單時間_ = $"{year}-{month}-{day} {HH}:{mm}:00";
                    }
                    content.廠牌 = list_value[i][(int)enum_驗收單匯入北市聯.廠商名稱].ObjectToString();
                    content.請購單號 = list_value[i][(int)enum_驗收單匯入北市聯.採購單號].ObjectToString();
                    if (交貨期限_.StringIsEmpty() == false) content.交貨時間 = 交貨期限_;
                    if (訂單時間_.StringIsEmpty() == false) content.訂單時間 = 訂單時間_;
                    content.應收數量 = list_value[i][(int)enum_驗收單匯入北市聯.採購數量].ObjectToString();
                    contents.Add(content);
                }

            }

            returnData returnData = inspectionClass.content_add("http://127.0.0.1:4433", contents);
            if (returnData == null)
            {
                returnData = new returnData();
                returnData.Result = $"上傳失敗";
                returnData.Code = -200;
                return returnData.JsonSerializationt(true);
            }
            return returnData.JsonSerializationt(true);
            
        }

        public enum enum_驗收單匯入北市聯
        {
            院區,
            單位,
            採購單號,
            採購項次,
            採購入庫別,
            料品代碼,
            料品名稱,
            廠商名稱,
            採購單位,
            採購數量,
            訂單傳送日,
            收貨數量,
            欠貨數量,
            收穫狀態,
            最後收穫日,
            交貨期限,
            逾期天數,
            合約案號,
            請購人員,
            請購單號,
            請購項次,
            請購數量,
            採購包裝,
        }
    }
}
