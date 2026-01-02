using Basic;
using DBVM_API;
using DBVM_API.Models;
using DBVM_API.Services;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SQLUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace DBVM
{
    /// <summary>
    /// 根據條碼取得小藥袋配方機資料 
    /// </summary>
    [Route("dbvm/[controller]")]
    [ApiController]
    public class SmallDrugController : Controller
    {
        static string MySQL_server = $"{ConfigurationManager.AppSettings["MySQL_server"]}";
        static string MySQL_database = $"{ConfigurationManager.AppSettings["MySQL_database"]}";
        static string MySQL_userid = $"{ConfigurationManager.AppSettings["MySQL_user"]}";
        static string MySQL_password = $"{ConfigurationManager.AppSettings["MySQL_password"]}";
        static string MySQL_port = $"{ConfigurationManager.AppSettings["MySQL_port"]}";

        private SQLControl sQLControl_醫囑資料 = new SQLControl(MySQL_server, MySQL_database, "order_list", MySQL_userid, MySQL_password, (uint)MySQL_port.StringToInt32(), MySql.Data.MySqlClient.MySqlSslMode.None);
        private string API_Server = "https://localhost:44318";

        private readonly HospitalApiService _hospitalApi;

        public SmallDrugController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
        }

        [HttpPost("barcode")]
        public async Task<IActionResult> GetOrder([FromBody] BarCodeRequest request)
        {
            MyTimerBasic timerTotal = new MyTimerBasic();
            string HIS呼叫時間 = "";
            string HISData時間 = "";
            string DB寫入時間 = "";

            //===============================
            // 1. 呼叫 HIS API
            //===============================
            MyTimerBasic t1 = new MyTimerBasic();
            HIS呼叫時間 = t1.ToString();

            // 管制櫃各類型的藥袋都刷刷看  有資料就開藥盒

            enum_藥袋類別 bagType;

            // 小藥袋
            var response = await _hospitalApi.GetSmallDrugByBarcode(request);

            // 日間帶藥
            BardCodeTakeDrugRequest dayRequest = new BardCodeTakeDrugRequest()
            {
                BarCode = request.BarCode,
                Category = enum_藥袋類別.日間帶藥.GetDescription()
            };
            var dayResponse = await _hospitalApi.GetTakeDrugByBarCode(dayRequest);

            // 出院帶藥
            BardCodeTakeDrugRequest dischargeRequest = new BardCodeTakeDrugRequest()
            {
                BarCode = request.BarCode,
                Category = enum_藥袋類別.出院帶藥.GetDescription()
            };
            var dischargeResponse = await _hospitalApi.GetTakeDrugByBarCode(dischargeRequest);

            string test = @"
                    {
                        ""ID"": ""05C812B2-C7FC-4A4C-B5AD-21E811A653DE"",
                        ""UDOINSTRUCTION"": null,
                        ""UDDDGNPRODUCT"": ""Neomycin oint 0.5% 28Gm"",
                        ""HNURSTA"": ""ED1"",
                        ""HNAMEC"": ""<病人姓名>"",
                        ""HHISNUM"": ""<病歷號>"",
                        ""ORDSEQ"": ""<醫囑序號>"",
                        ""ENCNTNO"": ""<就診號>"",
                        ""CREATETIME"": ""2024-08-01 04:18:52.0"",
                        ""DISPNO"": ""ER-1141"",
                        ""READTIME"": null,
                        ""SIDEEFFECT"": ""局部刺激。"",
                        ""INDICATION"": ""抗生素(消炎)藥膏"",
                        ""UDQNTY2"": ""-7 TUB"",
                        ""UDQNTY"": ""-7"",
                        ""UDDRGNO"": ""AN150"",
                        ""BEDNO"": ""<位置>"",
                        ""UDDOSAGE"": ""0 TUB"",
                        ""UDDDGNMATERIAL"": ""NEOMYCIN OINT"",
                        ""UDMDPNAM"": ""Neomycin oint 0.5% 28Gm"",
                        ""UDROUTE"": ""TOP"",
                        ""UDFREQN"": ""BID"",
                        ""UDDMDPNAME"": ""Neomycin oint 0.5% 28Gm"",
                        ""UDDURAT"": ""7"",
                        ""ORDDTTM"": ""2024-08-01 03:38:35.0"",
                        ""SECT"": ""CV"",
                        ""HBIRTHDT"": ""19910101"",
                        ""INDATE"": ""20250501"",
                        ""DIAGNOSIS"": ""xxx""
                    }";
            response.Data = JsonConvert.DeserializeObject<SmallDrugResponse>(test);

            MyTimerBasic t2 = new MyTimerBasic();
            HIS呼叫時間 = t2.ToString();

            //===============================
            // 2. 無資料處理
            //===============================
            if (response.Data == null && dayResponse.Data == null && dischargeResponse.Data == null)
            {
                returnData rd = new returnData()
                {
                    Code = -200,
                    TimeTaken = timerTotal.ToString(),
                    Result = "無此藥袋資料!"
                };
                return Content(rd.JsonSerializationt(true), "application/json; charset=utf-8");
            }

            //===============================
            // 3. 資料處理
            //===============================
            
            try
            {
                List<OrderClass> orderClasses = new List<OrderClass>();
                
                bool isExist = false;
                OrderClass orderClass = new OrderClass();
                orderClasses = OrderClass.get_by_barcode(API_Server, request.BarCode);

                var data = response.Data;
                if (orderClasses != null)
                    isExist = true;
                else
                {
                    orderClasses = new List<OrderClass>();
                    //====== 藥袋類型 ======
                    orderClass.藥袋類型 = enum_藥袋類別.小藥袋.GetDescription();

                    //====== 基本欄位 ======
                    orderClass.產出時間 = data.CREATETIME;
                    orderClass.藥袋條碼 = (string.IsNullOrEmpty(data.UDBC)) ? request.BarCode : data.UDBC;
                    orderClass.住院序號 = data.ORDSEQ;
                    orderClass.就醫序號 = data.ENCNTNO;
                    orderClass.藥品碼 = data.UDDDRGCODE;
                    orderClass.藥品名稱 = data.UDDDGNMATERIAL;
                    orderClass.病人姓名 = data.HNAMEC;
                    orderClass.病歷號 = data.HHISTNUM;
                    orderClass.領藥號 = data.DISPNO;
                    orderClass.科別 = data.SECT;
                    orderClass.單次劑量 = data.UDOGIVDOSE;
                    orderClass.劑量單位 = data.UDDDSPUNIT;
                    orderClass.途徑 = data.UDDROUTE;
                    orderClass.床號 = data.HBEDNO;
                    orderClass.交易量 = data.QUANTITY;

                    // ===== 無對應（僅註解保留） =====
                    // data.READTIME          // 讀取時間 → OrderClass 無對應屬性
                    // data.QUANTITY          // 數量 → OrderClass 無對應屬性
                    // data.PRINTER          // 印表機號 → OrderClass 無對應屬性
                    // data.UDOGIVUNIT        // 單位 → OrderClass 無明確對應（非劑量單位）
                    // data.UDOFUNCT          // 醫囑類別 → OrderClass 無對應屬性
                    // data.UDDDGNPRODUCT     // 藥品商品名 → OrderClass 無對應屬性
                    // data.HBIRTHDT          // 生日 → OrderClass 無對應屬性
                    // data.HNURSTAT          // 護理站 → OrderClass 無對應屬性
                    // data.INDATE            // 住院日 → OrderClass 無對應屬性
                    // data.DIAGNOSIS         // 主診斷 → OrderClass 無對應屬性
                    // data.SIDEEFFECT        // 過敏史 → OrderClass 無對應屬性
                    // data.INDICATION        // 適應症 → OrderClass 無對應屬性

                    // ===== OrderClass 但 JSON 未提供 =====
                    // orderClass.EXT_TIME     // JSON 無 EXT_TIME 欄位
                    // orderClass.交易量        // JSON 無交易量欄位
                    // orderClass.實際調劑量     // JSON 無實際調劑量欄位
                    // orderClass.病房          // JSON 無病房欄位
                    // orderClass.醫師代碼      // JSON 無醫師代碼欄位
                    // orderClass.頻次          // JSON 無頻次欄位
                    // orderClass.天數          // JSON 無天數欄位
                    // orderClass.費用別        // JSON 無費用別欄位
                    // orderClass.批序          // JSON 無批序欄位
                    // orderClass.開方日期      // JSON 無開方日期
                    // orderClass.結方日期      // JSON 無結方日期
                    // orderClass.核對時間      // JSON 無核對時間
                    // orderClass.發藥時間      // JSON 無發藥時間
                    // orderClass.領藥時間      // JSON 無領藥時間
                    // orderClass.備註          // JSON 無備註欄位

                    ////====== PRI_KEY ======                
                    orderClass.PRI_KEY = data.ID;

                    orderClasses.Add(orderClass);
                }

                //===============================
                // 4. 寫入資料庫
                //===============================            
                MyTimerBasic t_db = new MyTimerBasic();
                returnData returnData_order = new returnData();
                if (!isExist)
                    returnData_order = OrderClass.update_order_list_new(API_Server, orderClasses);
                else 
                    returnData_order.Data = orderClasses;

                //var returnData_order = OrderClass.update_order_list(API_Server, orderClasses);
                DB寫入時間 = t_db.ToString();

                returnData_order.Value = data.UDBC;   // 回傳院方藥袋條碼
                returnData_order.TimeTaken += $"{timerTotal}";
                returnData_order.Result += $"，HIS呼叫時間:{HIS呼叫時間}，取得HIS資料:{HISData時間}，DB寫入時間:{DB寫入時間}";

                string json = returnData_order.JsonSerializationt(true);
                Logger.Log(json);
                return Ok(returnData_order);

            }
            catch (Exception ex)
            {
                return Content($"HIS系統資料解析異常 (Row)：{ex.Message}", "text/plain; charset=utf-8");
            }

        }
    }
}
