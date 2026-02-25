using Basic;
using DBVM_API;
using DBVM_API.Constant;
using DBVM_API.Models;
using DBVM_API.Services;
using Google.Protobuf.WellKnownTypes;
using H_Pannel_lib;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using SQLUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DB2VM
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class BBARController : ControllerBase
    {
        private string API_Server = "https://127.0.0.1:4433";

        private readonly HospitalApiService _hospitalApi;
        public BBARController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
            API_Server = hospitalApi.API_Server;
        }

        /// <summary>
        /// 藥袋刷條碼
        /// </summary>
        /// <param name="barcode">barcode</param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<string> GetOrder([FromQuery] string barcode)
        {
            MyTimerBasic timerTotal = new MyTimerBasic();
            string HIS呼叫時間 = "";
            string HIS藥袋類型 = "";
            string HISData時間 = "";
            string DB寫入時間 = "";
            string DB查詢時間 = "";

            //===============================
            // 1. 呼叫 HIS API
            //===============================
            MyTimerBasic t1 = new MyTimerBasic();
            HIS呼叫時間 = t1.ToString();

            enum_藥袋類別 bagType;

            // 醫囑藥物品項查詢API
            MedicationRequest medicationRequest = new MedicationRequest()
            {
                InhospitalNum = 1893834,
                MedType = "u",
                BagNum = 1,
                Barcode = barcode
            };
            var medicationTask = _hospitalApi.GetFHIRMedicaionRequests(medicationRequest);

            // 等待所有任務完成
            await Task.WhenAll(medicationTask);

            // 取得結果
            var medicationResult = medicationTask.Result;

            MyTimerBasic t2 = new MyTimerBasic();
            HIS呼叫時間 = t2.ToString();

            //===============================
            // 2. 無資料處理（全部沒資料）
            //===============================

            bool noMedication = !(medicationResult.Success && medicationResult.Data != null && medicationResult.Data.Total > 0);

            if (noMedication)
            {
                returnData rd = new returnData()
                {
                    Code = -200,
                    TimeTaken = timerTotal.ToString(),
                    Result = "無此藥袋資料!"
                };
                //return Content(rd.JsonSerializationt(true), "application/json; charset=utf-8");
                return rd.JsonSerializationt(true);
            }

            //===============================
            // 3. 資料處理
            //===============================

            try
            {
                List<OrderClass> orderClasses = new List<OrderClass>();
                bool isExist = false;

                // 先查看DB是否有資料 (有資料直接回傳)
                orderClasses = OrderClass.get_by_barcode(API_Server, barcode);
                if (orderClasses != null && orderClasses.Count > 0)
                    isExist = true;
                else
                {
                    orderClasses = new List<OrderClass>();
                    int batchNum = 1;

                    if (medicationResult.Success && medicationResult.Data != null && medicationResult.Data.Total > 0)
                    {
                        var list = medicationResult.Data.Entry;
                        foreach (var data in list)
                        {
                            OrderClass orderClass = new OrderClass();

                            //====== 藥袋類型 ======                            
                            orderClass.藥袋類型 = data.MedType;

                            //====== 基本欄位 ======
                            orderClass.GUID = data.SerialNo.ToString();   //  用流水序號 ???
                            orderClass.批序 = (batchNum++).ToString();
                            orderClass.藥袋條碼 = barcode;
                            //orderClass.產出時間 = data.CREATETIME;
                            orderClass.住院序號 = data.InhospitalNum.ToString();
                            orderClass.就醫序號 = data.SerialNum.ToString();  // 用醫囑序號?
                            orderClass.藥品碼 = data.DrugId;
                            //orderClass.藥品名稱 = data.UDDDGNPRODUCT;
                            //orderClass.病人姓名 = data.HNAMEC;  
                            //orderClass.病歷號 = data.HHISNUM;
                            orderClass.領藥號 = data.BagNum.ToString();
                            //orderClass.單次劑量 = LogicUtility.ParseDose(data.UDDOSAGE).SingleDose;
                            //orderClass.劑量單位 = LogicUtility.ParseDose(data.UDDOSAGE).DoseUnit;
                            //orderClass.頻次 = data.UDFREQN;
                            //orderClass.途徑 = data.UDROUTE;
                            //orderClass.病房 = data.HNURSTA;
                            //orderClass.床號 = data.BEDNO;
                            orderClass.開方日期 = LogicUtility.GetPrescriptionDate(DateTime.Now);
                            orderClass.交易量 = LogicUtility.GetTradingVolume(data.FirstQty.ToString());

                            ////====== PRI_KEY ======                
                            orderClass.PRI_KEY = LogicUtility.GetPrimaryKey(orderClass);

                            orderClasses.Add(orderClass);
                        }

                        HIS藥袋類型 = "???";
                    }
                }

                //===============================
                // 4. 寫入資料庫
                //===============================            
                MyTimerBasic t_db = new MyTimerBasic();
                returnData returnData_order = new returnData();
                if (!isExist)
                {
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
                        //return Content(returnData_order.JsonSerializationt(true), "application/json; charset=utf-8");
                        return returnData_order.JsonSerializationt(true);
                    }
                }
                else
                {
                    returnData_order.Code = 200;
                    returnData_order.Data = orderClasses;
                }

                DB寫入時間 = t_db.ToString();

                string returnBarcode = barcode;

                returnData_order.Value = returnBarcode;
                returnData_order.TimeTaken += $"{timerTotal}";
                returnData_order.Result += $"，HIS呼叫時間:{HIS呼叫時間}，取得HIS資料:{HISData時間}，藥袋類型:{HIS藥袋類型}，DB寫入時間:{DB寫入時間}";

                string json = returnData_order.JsonSerializationt(true);
                Logger.Log(json);
                //return Ok(returnData_order);
                return json;

            }
            catch (Exception ex)
            {
                //return Content($"HIS系統資料解析異常 (Row)：{ex.Message}", "text/plain; charset=utf-8");
                return $"HIS系統資料解析異常 (Row)：{ex.Message}";
            }
        }
    }
}