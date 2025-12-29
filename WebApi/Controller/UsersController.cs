using Basic;
using DBVM_API.Models;
using DBVM_API.Services;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DBVM
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly HospitalApiService _hospitalApi;

        public UsersController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
        }

        [HttpGet()]
        public async Task<IActionResult> GetUserList()
        {
            MyTimerBasic timerTotal = new MyTimerBasic();
            string HIS連線時間 = "";
            string HISData時間 = "";
            string DB寫入時間 = "";


            //===============================
            // 1. 呼叫 HIS API
            //===============================
            MyTimerBasic t1 = new MyTimerBasic();
            var response = await _hospitalApi.GetCardUsers();
            HIS連線時間 = t1.ToString();

            //===============================
            // 2. 無資料處理
            //===============================
            if (response.Data == null)
            {
                returnData rd = new returnData()
                {
                    Code = -200,
                    TimeTaken = timerTotal.ToString(),
                    Result = "無此登入資料!"
                };
                return Content(rd.JsonSerializationt(true), "application/json; charset=utf-8");
            }

            returnData result = new returnData()
            {
                Code = 200,
                TimeTaken = timerTotal.ToString(),
                Data = response.Data,
                Result = $"取得使用者資料共<{response.Data.Count}>筆"
            };

            return Ok(result);
        }
    }
}
