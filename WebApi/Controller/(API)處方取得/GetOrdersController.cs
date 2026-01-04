using Basic;
using DBVM_API;
using DBVM_API.Models;
using DBVM_API.Services;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBVM
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class GetOrdersController : Controller
    {
        private string API_Server = "https://localhost:44318";

        private readonly HospitalApiService _hospitalApi;
        public GetOrdersController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
        }

        /// <summary>
        /// 取得處方資料
        /// </summary>
        /// <param name="startDate">開始日期  格式 YYYYMMDDHHMMSS</param>
        /// <param name="endDate">結束日期  格式 YYYYMMDDHHMMSS</param>
        /// <param name="complete">Y = 取得已讀取 / N = 取得未讀取 / 不傳 = 取得全部</param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetOrders(string startDate, string endDate, string complete = null)
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
            // 小藥袋
            SmallDrugRequest request = new SmallDrugRequest()
            {
                StartDate = startDate,
                EndDate = endDate,
                Complete = complete
            };
            var smallTask = _hospitalApi.GetSmallDrug(request);

            // 日間帶藥
            TakeDrugRequest dayRequest = new TakeDrugRequest()
            {
                StartDate = startDate,
                EndDate = endDate,
                Category = enum_藥袋類別.日間帶藥.GetDescription(),
                Complete = complete
            };
            var dayTask = _hospitalApi.GetTakeDrug(dayRequest);

            // 出院帶藥
            TakeDrugRequest dischargeRequest = new TakeDrugRequest()
            {
                StartDate = startDate,
                EndDate = endDate,
                Category = enum_藥袋類別.出院帶藥.GetDescription(),
                Complete = complete
            };
            var dischargeTask = _hospitalApi.GetTakeDrug(dischargeRequest);

            // 等待所有任務完成
            await Task.WhenAll(smallTask, dayTask, dischargeTask);

            // 取得結果
            var smallResult = smallTask.Result;
            var dayResult = dayTask.Result;
            var dischargeResult = dischargeTask.Result;

            //===============================
            // 2. 無資料處理（全部沒資料）
            //===============================

            bool noSmall = !(smallResult.Success && smallResult.Data != null);
            bool noDay = !(dayResult.Success && dayResult.Data != null && dayResult.Data.Count > 0);
            bool noDischarge = !(dischargeResult.Success && dischargeResult.Data != null && dischargeResult.Data.Count > 0);

            if (noSmall && noDay && noDischarge)
            {
                returnData rd = new returnData()
                {
                    Code = -200,
                    TimeTaken = timerTotal.ToString(),
                    Result = "無藥袋資料!"
                };
                return Content(rd.JsonSerializationt(true), "application/json; charset=utf-8");
            }


            //===============================
            // 3. 資料處理
            //===============================

            try
            {
                List<OrderClass> orderClasses = new List<OrderClass>();

                if (smallResult.Success && smallResult.Data != null)
                {
                    foreach (var item in smallResult.Data)
                    {
                        OrderClass order = new OrderClass();
                    }
                }


                //===============================
                // 4. 寫入資料庫
                //===============================            
                MyTimerBasic t_db = new MyTimerBasic();
                returnData returnData_order = new returnData();
                if (orderClasses != null && orderClasses.Count > 0)
                    returnData_order = OrderClass.update_order_list_new(API_Server, orderClasses);
                else
                {
                    // 沒有產出 orderClasses
                    returnData_order = new returnData()
                    {
                        Code = -201,
                        Result = "資料轉換發生問題，未產生 OrderClass"
                    };
                    return Content(returnData_order.JsonSerializationt(true), "application/json; charset=utf-8");
                }

                DB寫入時間 = t_db.ToString();

                returnData_order.Value = "";
                returnData_order.TimeTaken += $"{timerTotal}";
                returnData_order.Result += $"，HIS呼叫時間:{HIS呼叫時間}，取得HIS資料:{HISData時間}，DB寫入時間:{DB寫入時間}";

                string json = returnData_order.JsonSerializationt(true);
                Logger.Log(json);
                return Ok(returnData_order);

            }
            catch (System.Exception ex)
            {
                return Content($"HIS系統資料解析異常 (Row)：{ex.Message}", "text/plain; charset=utf-8");
            }
        }
    }
}
