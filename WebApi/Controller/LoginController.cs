using Basic;
using DBVM_API;
using DBVM_API.Models;
using DBVM_API.Services;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DBVM
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly HospitalApiService _hospitalApi;

        public LoginController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
        }

        [HttpPost()]
        public async Task<IActionResult> PharmacistLogin([FromBody] PharmacistLoginRequest request)
        {
            MyTimerBasic timerTotal = new MyTimerBasic();
            string HIS連線時間 = "";
            string HISData時間 = "";
            string DB寫入時間 = "";


            //===============================
            // 1. 呼叫 HIS API
            //===============================
            MyTimerBasic t1 = new MyTimerBasic();
            var response = await _hospitalApi.PharmacistLogin(request);
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

            return Ok(response);
        }
    }
}
