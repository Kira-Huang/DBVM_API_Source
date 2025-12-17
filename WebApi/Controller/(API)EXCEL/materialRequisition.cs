using Basic;
using Google.Protobuf.WellKnownTypes;
using HIS_DB_Lib;
using Microsoft.AspNetCore.Mvc;
using MyOffice;
using MySql.Data.MySqlClient;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SQLUI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ubiety.Dns.Core;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DBVM_API.Controller._API_EXCEL
{
    [Route("dbvm/[controller]")]
    [ApiController]
    public class materialRequisition : ControllerBase
    {
        
        [HttpPost(("download_excel_by_requestTime"))]
        public async Task<ActionResult> Post_download_excel_by_requestTime([FromBody] returnData returnData)
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            
            returnData.Method = "download_excel_by_requestTime";
            try
            {
                if (returnData.ValueAry.Count != 2) return Content($"下載失敗：returnData.ValueAry 內容應為[起始時間][結束時間]");
              
                string 起始時間 = returnData.ValueAry[0];
                string 結束時間 = returnData.ValueAry[1];
                if (起始時間.Check_Date_String() == false || 結束時間.Check_Date_String() == false) return Content($"下載失敗：時間範圍格式錯誤");
              
                DateTime dateTime_st = 起始時間.StringToDateTime();
                DateTime dateTime_end = 結束時間.StringToDateTime();

                List<materialRequisitionClass> materialRequisitionClasses = materialRequisitionClass.get_by_requestTime("http://127.0.0.1:4433", dateTime_st, dateTime_end);
        
                       
                if (materialRequisitionClasses == null || materialRequisitionClasses.Count == 0) return Content($"下載失敗：returnData.Data資料錯誤");

                materialRequisitionClasses = materialRequisitionClasses.Where(temp => temp.狀態 == "已過帳").ToList();
                List<List<materialRequisitionClass>> groupedList = materialRequisitionClasses
                    .GroupBy(x => x.申領單位) 
                    .Select(g => g.ToList())
                    .ToList();
                List<System.Data.DataTable> dataTables_creat = new List<System.Data.DataTable>();
                List<materialRequisitionClass> update = new List<materialRequisitionClass>();
                foreach (var list in groupedList)
                {
                    List<object[]> list_buf = new List<object[]>();
                    foreach (var item in list)
                    {
                        string flag = string.Empty;                                         
                        if (item.備註 == "已匯出") flag = "Y";
                        DateTime 申領時間 = item.申領時間.StringToDateTime();
                        object[] value = new object[new enum_materialRequisition_dbvm().GetLength()];
                        value[(int)enum_materialRequisition_dbvm.庫存碼] = item.藥碼;
                        value[(int)enum_materialRequisition_dbvm.料品名稱] = item.藥名;
                        value[(int)enum_materialRequisition_dbvm.申請數量] = item.實撥量;
                        value[(int)enum_materialRequisition_dbvm.申請庫房] = item.申領單位;
                        value[(int)enum_materialRequisition_dbvm.發料庫房] = "PH1";
                        value[(int)enum_materialRequisition_dbvm.申請日期] = 申領時間.ToString("yyyy/M/d");
                        value[(int)enum_materialRequisition_dbvm.申請時間] = 申領時間.ToString("HH:mmm:ss");
                        value[(int)enum_materialRequisition_dbvm.申請人員] = item.申領人員;
                        value[(int)enum_materialRequisition_dbvm.鴻森系統是否匯出] = flag;
                        if (item.備註 != "已匯出") 
                        {
                            item.備註 = "已匯出";
                            update.Add(item);
                        }                   
                        list_buf.Add(value);
                    }
                    System.Data.DataTable dataTable_buf = list_buf.ToDataTable(new enum_materialRequisition_dbvm());
                    string tableName = $"{list[0].申領單位}";

                    // 移除或替換非法字元
                    string safeFileName = Regex.Replace(tableName, @"[\\/:*?""<>|]", "_");

                    // 指定為合法的檔案名稱
                    dataTable_buf.TableName = safeFileName;
                    
                    // 新增一行在最前面
                    DataRow row1 = dataTable_buf.NewRow();
                    row1[0] = "庫房申請表";
                    row1[1] = "匯給C2-F009庫房領用申請作業";
                    

                    dataTable_buf.Rows.InsertAt(row1, 0);

                    // 第二行
                    DataRow row2 = dataTable_buf.NewRow();
                    row2[0] = "院區";
                    row2[1] = "Q";
                    row2[2] = "忠孝";
                    
                    dataTable_buf.Rows.InsertAt(row2, 1);

                    // 第三行
                   

                    // 第四行
                    DataRow row3 = dataTable_buf.NewRow();
                    string 核撥單位 = list[0].核撥單位;
                    if (核撥單位 == "藥庫") 核撥單位 = "PH1";
                    row3[0] = "發料庫房";
                    row3[1] = 核撥單位;
                    row3[2] = getPharName(核撥單位);
                    dataTable_buf.Rows.InsertAt(row3, 3);

                    DataRow row4 = dataTable_buf.NewRow();
                    row4[0] = "申請庫房";
                    row4[1] = $"{list[0].申領單位}";
                    row4[2] = getPharName(list[0].申領單位);

                    dataTable_buf.Rows.InsertAt(row4, 2);
                    dataTables_creat.Add(dataTable_buf);

                }

                string xlsx_command = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string xls_command = "application/vnd.ms-excel";
                //byte[] excelData = ExcelClass.NPOI_GetBytes(dataTables_creat, Excel_Type.xlsx);
                byte[] excelData = ExportToBytes(dataTables_creat, 4);
                Stream stream = new MemoryStream(excelData);
                // 更新備註
                if (update.Count > 0)
                {
                    returnData returnData_update = materialRequisitionClass.update_by_guid("http://127.0.0.1:4433", update);
                    if (returnData_update == null || returnData_update.Code != 200)
                    {
                        return Content($"下載失敗：更新匯出狀態失敗");
                    }
                }
                
                return await Task.FromResult(File(stream, xlsx_command, $"{DateTime.Now.ToDateString("-")}_申領明細.xlsx"));
            }
            catch (Exception ex)
            {
                return null;
            }
            

        }
        public enum enum_materialRequisition_dbvm
        {
            庫存碼,
            料品名稱,
            申請數量,
            申請庫房,
            發料庫房,
            申請日期,
            申請時間,
            申請人員,
            鴻森系統是否匯出,
        }
        public static Dictionary<string, string> pharmacyDictionary = new Dictionary<string, string>
        {
            { "PH1", "西藥庫" },
            { "PH5", "住院藥局" },
            { "PH7", "門急藥局" }
        };
        public static string getPharName(string phar)
        {
            if (pharmacyDictionary.TryGetValue(phar , out string name))
            {
                return name;
            }
            else
            {
                return "";
            }
        }
        private static byte[] ExportToBytes(List<DataTable> tables, int preHeaderRows = 4)
        {
            if (tables == null || tables.Count == 0) return Array.Empty<byte>();

            IWorkbook wb = new XSSFWorkbook();

            foreach (var dt in tables)
            {
                // 你已自行保證 TableName 合法且不重複
                ISheet sheet = wb.CreateSheet(string.IsNullOrWhiteSpace(dt.TableName) ? "Sheet1" : dt.TableName);
                int rowIndex = 0;

                int headerRows = Math.Max(0, Math.Min(preHeaderRows, dt.Rows.Count));

                // 1) 抬頭列（位於最上）
                for (int r = 0; r < headerRows; r++)
                {
                    IRow row = sheet.CreateRow(rowIndex++);
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        row.CreateCell(c).SetCellValue(dt.Rows[r][c]?.ToString() ?? string.Empty);
                    }
                }

                // 2) 欄位標題
                IRow headerRow = sheet.CreateRow(rowIndex++);
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    headerRow.CreateCell(c).SetCellValue(dt.Columns[c].ColumnName);
                }

                // 3) 資料列
                for (int r = headerRows; r < dt.Rows.Count; r++)
                {
                    IRow row = sheet.CreateRow(rowIndex++);
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        row.CreateCell(c).SetCellValue(dt.Rows[r][c]?.ToString() ?? string.Empty);
                    }
                }
            }

            using var ms = new MemoryStream();
            wb.Write(ms);
            return ms.ToArray();
        }


    }
}
