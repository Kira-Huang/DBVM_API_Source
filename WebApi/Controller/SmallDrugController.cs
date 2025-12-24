using Basic;
using DBVM_API;
using DBVM_API.Models;
using DBVM_API.Services;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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
        private readonly HospitalApiService _hospitalApi;

        public SmallDrugController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
        }

        [HttpPost("barcode")]
        public async Task<IActionResult> GetOrder([FromBody] BarCodeRequest request)
        {
            MyTimerBasic timerTotal = new MyTimerBasic();
            string HIS連線時間 = "";
            string HISData時間 = "";
            string DB寫入時間 = "";


            //===============================
            // 1. 呼叫 HIS API
            //===============================
            MyTimerBasic t1 = new MyTimerBasic();
            var response = await _hospitalApi.GetSmallDrugByBarcode(request);
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
                    Result = "無此藥袋資料!"
                };
                return Content(rd.JsonSerializationt(true), "application/json; charset=utf-8");
            }

            //===============================
            // 3. 無資料處理
            //===============================
            try
            {
                OrderClass orderClass = new OrderClass();
                var data = response.Data;

                //====== 藥袋類型 ======
                orderClass.藥袋類型 = enum_藥袋類別.小藥袋.GetDescription();

                //====== 基本欄位 ======
                orderClass.藥袋條碼 = data.UDBC;
                orderClass.住院序號 = data.ORDSEQ;
                orderClass.藥品碼 = data.UDDDRGCODE;
                orderClass.藥品名稱 = data.UDDDGNMATERIAL;
                orderClass.病人姓名 = data.HNAMEC;
                orderClass.病歷號 = data.HHISTNUM;
                orderClass.領藥號 = data.DISPNO;
                orderClass.科別 = data.SECT;
                //orderClass.醫師代碼 = data.
                //orderClass.頻次 = 
                //orderClass.天數 = 
                orderClass.單次劑量 = data.UDOGIVDOSE;
                orderClass.劑量單位 = data.UDDDSPUNIT;
                //orderClass.費用別 = 
                //orderClass.批序 = 
                orderClass.途徑 = data.UDDROUTE;
                orderClass.床號 = data.HBEDNO;

                //====== 就醫時間 ======
                //string visit = SafeGet(reader, "PAC_VISITDT");
                //if (visit.Length == 8)
                //    orderClass.就醫時間 = $"{visit[..4]}-{visit[4..6]}-{visit[6..8]}";

                //====== 開方日期 ======
                //string 時間 = SafeGet(reader, "PAC_PROCDTTM");
                //if (時間.Length == 14)
                //{
                //    orderClass.開方日期 =
                //        $"{時間[..4]}/{時間[4..6]}/{時間[6..8]} " +
                //        $"{時間[8..10]}:{時間[10..12]}:{時間[12..14]}";
                //}

                //====== 交易量（負值） ======
                //double sumQTY = SafeDouble(reader, "PAC_SUMQTY");
                //orderClass.交易量 = (-sumQTY).ToString();

                ////====== PRI_KEY ======
                //string key = $"{orderClass.頻次}{orderClass.天數}{orderClass.單次劑量}{orderClass.劑量單位}";
                //orderClass.PRI_KEY = $"{時間}-{orderClass.病歷號}-{orderClass.藥品碼}{orderClass.交易量}-{key}";
                orderClass.PRI_KEY = data.ID;

                //orderClasses.Add(orderClass);
            }
            catch (Exception ex)
            {
                return Content($"HIS系統資料解析異常 (Row)：{ex.Message}", "text/plain; charset=utf-8");
            }


            //===============================
            // 5. 寫入資料庫
            //===============================            //MyTimerBasic t_db = new MyTimerBasic();
            //var returnData_order = OrderClass.update_order_list_new("http://127.0.0.1:4433", orderClasses);
            //DB寫入時間 = t_db.ToString();

            //returnData_order.Value = BarCode;
            //returnData_order.TimeTaken += $"{myTimer_total}";
            //returnData_order.Result += $"，HIS連線時間:{HIS連線時間}，取得HIS資料:{HISData時間}，DB寫入時間:{DB寫入時間}";

            //string json = returnData_order.JsonSerializationt(true);
            //Logger.Log(json);
            //conn_oracle.Close();
            //return json;
            return Ok(response.Data);

        }
    }
}
