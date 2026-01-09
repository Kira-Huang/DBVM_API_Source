using Basic;
using DBVM_API.Services;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace DBVM
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private string API_Server = "https://127.0.0.1:4433";

        private readonly HospitalApiService _hospitalApi;
        public OrderController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
            API_Server = _hospitalApi.API_Server;
        }

        /// <summary>
        /// 取得處方資料 By 領藥號
        /// </summary>
        /// <param name="med_bag_num">領藥號</param>
        /// <returns></returns>
        [HttpGet("by_med_bag_num")]
        public IActionResult GetOrderByMedBagNum([FromQuery] string med_bag_num)
        {
            MyTimerBasic timerTotal = new MyTimerBasic();
            string API呼叫時間 = "";
            string API取得時間 = "";
            
            returnData returnData_order = new returnData();

            API呼叫時間 = timerTotal.ToString();
            
            returnData_order.Data = OrderClass.get_by_MED_BAG_NUM(API_Server, med_bag_num);

            API取得時間 = timerTotal.ToString();

            returnData_order.Code = 200;
            returnData_order.TimeTaken += $"{timerTotal}";
            returnData_order.Result += $"API呼叫時間:{API呼叫時間}，API取得時間:{API取得時間}，";

            string json = returnData_order.JsonSerializationt(true);
            Logger.Log(json);
            return Ok(returnData_order);
        }

        /// <summary>
        /// 取得處方資料 By 病歷號
        /// </summary>
        /// <param name="patientID"></param>
        /// <returns></returns>
        [HttpGet("by_patient_id")]
        public IActionResult GetOrderByPatientID([FromQuery] string patientID)
        {
            MyTimerBasic timerTotal = new MyTimerBasic();
            string API呼叫時間 = "";
            string API取得時間 = "";

            returnData returnData_order = new returnData();

            API呼叫時間 = timerTotal.ToString();

            returnData_order.Data = OrderClass.get_by_PATCODE(API_Server, patientID);

            API取得時間 = timerTotal.ToString();

            returnData_order.Code = 200;
            returnData_order.TimeTaken += $"{timerTotal}";
            returnData_order.Result += $"API呼叫時間:{API呼叫時間}，API取得時間:{API取得時間}";

            string json = returnData_order.JsonSerializationt(true);
            Logger.Log(json);
            return Ok(returnData_order);
        }
    }
}
