using Basic;
using DBVM_API;
using DBVM_API.Models;
using DBVM_API.Services;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using SQLUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace DBVM
{
    /// <summary>
    /// 根據條碼取得小藥袋配方機資料 
    /// </summary>
    [Route("dbvm/[controller]")]
    [ApiController]
    public class TakeDrugController : Controller
    {
        static string MySQL_server = $"{ConfigurationManager.AppSettings["MySQL_server"]}";
        static string MySQL_database = $"{ConfigurationManager.AppSettings["MySQL_database"]}";
        static string MySQL_userid = $"{ConfigurationManager.AppSettings["MySQL_user"]}";
        static string MySQL_password = $"{ConfigurationManager.AppSettings["MySQL_password"]}";
        static string MySQL_port = $"{ConfigurationManager.AppSettings["MySQL_port"]}";

        private SQLControl sQLControl_醫囑資料 = new SQLControl(MySQL_server, MySQL_database, "order_list", MySQL_userid, MySQL_password, (uint)MySQL_port.StringToInt32(), MySql.Data.MySqlClient.MySqlSslMode.None);
        private string API_Server = "http://127.0.0.1:44318";


        private readonly HospitalApiService _hospitalApi;

        public TakeDrugController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
            API_Server = hospitalApi.API_Server;
        }

        [HttpPost("barcode")]
        public async Task<IActionResult> GetOrder([FromBody] BardCodeTakeDrugRequest request)
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

            var response = await _hospitalApi.GetTakeDrugByBarCode(request);

            MyTimerBasic t2 = new MyTimerBasic();
            HIS呼叫時間 = t2.ToString();

            //===============================
            // 2. 無資料處理
            //===============================
            if (response.Data == null)
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
                foreach (var data in response.Data)
                {
                    OrderClass orderClass = new OrderClass();

                    //====== 藥袋類型 ======
                    if (request.Category.TryToEnumFromDescription(out enum_藥袋類別 category))
                        orderClass.藥袋類型 = category.GetDescription();
                    else
                        continue;

                    //====== 基本欄位 ======
                    orderClass.藥袋條碼 = request.BarCode;
                    orderClass.產出時間 = data.CREATETIME;
                    orderClass.住院序號 = data.ORDSEQ;
                    orderClass.就醫序號 = data.ENCNTNO;
                    orderClass.藥品碼 = data.UDDRGNO;
                    orderClass.藥品名稱 = data.UDDDGNMATERIAL;
                    orderClass.病人姓名 = data.HNAMEC;
                    orderClass.病歷號 = data.HHISNUM;
                    orderClass.領藥號 = data.DISPNO;
                    orderClass.單次劑量 = data.UDDOSAGE;
                    orderClass.頻次 = data.UDFREQN;
                    orderClass.途徑 = data.UDROUTE;
                    orderClass.床號 = data.BEDNO;

                    // ===== 無對應（僅註解保留） =====

                    // data.UDDURAT           // 數量 → OrderClass 無對應屬性
                    // data.UDQNTY            // 實配量(第一欄位) → OrderClass 無對應屬性
                    // data.UDQNTY2           // 實配量(第二欄位) → OrderClass 無對應屬性
                    // data.INDICATION        // 指導內容 → OrderClass 無對應屬性
                    // data.UDDDGNPRODUCT     // 商品名 → OrderClass 無對應屬性
                    // data.DIAGNOSIS         // 主診斷 → OrderClass 無對應屬性
                    // data.HBIRTHDT          // 生日 → OrderClass 無對應屬性
                    // data.DIAGNOSIS         // 主診斷 → OrderClass 無對應屬性
                    // data.SIDEEFFECT        // 過敏史 → OrderClass 無對應屬性
                    // data.ORDDTTM           // 醫囑開立時間 → OrderClass 無對應屬性
                    // data.UDOINSTRUCTION    // 醫囑備註 → OrderClass 無對應屬性

                    // ===== OrderClass 但 JSON 未提供 =====

                    // orderClass.藥局代碼        // JSON 無藥局代碼
                    // orderClass.就醫類別        // JSON 無就醫類別
                    // orderClass.批序            // JSON 無批序
                    // orderClass.天數            // JSON 無天數
                    // orderClass.科別            // JSON 無科別名稱
                    // orderClass.劑量單位        // JSON 無劑量單位
                    // orderClass.費用別          // JSON 無費用別
                    // orderClass.醫師代碼        // JSON 無醫師代碼
                    // orderClass.結方日期        // JSON 無結方日期
                    // orderClass.核對時間        // JSON 無核對時間
                    // orderClass.發藥時間        // JSON 無發藥時間
                    // orderClass.領藥時間        // JSON 無領藥時間
                    // orderClass.備註          // JSON 無備註欄位

                    ////====== PRI_KEY ======                
                    orderClass.PRI_KEY = data.ID;

                    orderClasses.Add(orderClass);
                }

                //===============================
                // 4. 寫入資料庫
                //===============================            
                MyTimerBasic t_db = new MyTimerBasic();
                var returnData_order = OrderClass.update_order_list_new(API_Server, orderClasses);
                DB寫入時間 = t_db.ToString();

                returnData_order.Value = request.BarCode;   // 回傳院方藥袋條碼
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
