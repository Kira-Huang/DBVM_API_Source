using Basic;
using H_Pannel_lib;
using HIS_DB_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_即時庫存更新
{
    class Program
    {
        static void Main(string[] args)
        {
            string API = "http://192.168.5.200:4450";
            List<sys_serverSettingClass> settingClasses = sys_serverSettingClass.get_name(API);
            List<medClass> med_cloud = medClass.get_med_cloud(API);
            Dictionary<string, List<medClass>> medCloudDict = medClass.CoverToDictionaryByCode(med_cloud);

            foreach (var item in settingClasses)
            {
                string serverName = item.設備名稱;
                string serverType = item.類別;
                string url = $"{API}/api/device/list/{serverName}";
                if (serverType == "藥庫")
                    url = $"{API}/api/device/list_ds01/{serverName}";
                string result = Net.WEBApiGet(url);
                List<string> code_stock = new List<string>();
                returnData returnData = result.JsonDeserializet<returnData>();
                if (returnData == null || returnData.Code != 200)
                    continue;
                List<DeviceBasic> deviceBasics = returnData.Data.ObjToClass<List<DeviceBasic>>();
                List<stockClass> add = new List<stockClass>();
                List<stockClass> update = new List<stockClass>();

                returnData returnData_stock = stockClass.get_stock(API, serverName, serverType);
                List<stockClass> stockClasses = returnData_stock.Data.ObjToClass<List<stockClass>>();
                Dictionary<string, List<stockClass>> dic_stock = stockClasses.ToDictByCode();

                for (int i = 0; i < deviceBasics.Count; i++)
                {
                    string 藥碼 = deviceBasics[i].BarCode;
                    stockClass stocks = dic_stock.GetByCode(藥碼).FirstOrDefault();
                    code_stock.Add(藥碼);
                    if (stocks == null)
                    {
                        stockClass stockClass = new stockClass();
                        stockClass.藥碼 = 藥碼;
                        stockClass.效期 = deviceBasics[i].List_Validity_period;
                        stockClass.批號 = deviceBasics[i].List_Lot_number;
                        stockClass.數量 = deviceBasics[i].List_Inventory;
                        add.Add(stockClass);
                    }
                    else
                    {
                        stocks.效期 = deviceBasics[i].List_Validity_period;
                        stocks.批號 = deviceBasics[i].List_Lot_number;
                        stocks.數量 = deviceBasics[i].List_Inventory;
                        update.Add(stocks);
                    }

                    if (add.Count > 0)
                    {
                        returnData returnData_add = stockClass.add(API, serverName, serverType, add);
                        if (returnData_add == null)
                            Logger.Log(add.JsonSerializationt(true));
                        else
                            Logger.Log(returnData_add.JsonSerializationt(true));
                    }

                    if (update.Count > 0)
                    {
                        returnData returnData_update = stockClass.update(API, serverName, serverType, update);
                        if (returnData_update == null)
                            Logger.Log(update.JsonSerializationt(true));
                        else
                            Logger.Log(returnData_update.JsonSerializationt(true));
                    }

                    /// 消耗量
                    DateTime dateTime = DateTime.Now.AddDays(-1);
                    string start = dateTime.GetStartDate().ToDateTimeString();
                    string end = dateTime.GetEndDate().ToDateTimeString();

                    DateTime date_now = DateTime.Now;
                    string now = date_now.ToDateTimeString();
                    string today_start = date_now.GetStartDate().ToDateTimeString();
                    string today_end = date_now.GetEndDate().ToDateTimeString();

                    returnData returnData_avg = consumptionClass.get_avg_by_start_end(API, serverName, serverType, start, end);
                    List<consumptionClass> consumptionClasses = returnData_avg.Data.ObjToClass<List<consumptionClass>>();
                    if (consumptionClasses.Count() != 0)
                        continue;
                    string com_url = $"{API}/api/consumption/serch_datas_by_ST_END";
                    string com_init_url = $"{API}/api/consumption/init";

                    returnData returnData_consumption = new returnData();
                    returnData_consumption.ValueAry.Add(start);
                    returnData_consumption.ValueAry.Add(end);
                    returnData_consumption.ValueAry.Add(serverName);
                    returnData_consumption.ValueAry.Add(serverType);
                    returnData_consumption.ServerName = serverName;
                    returnData_consumption.ServerType = serverType;
                    string json_in = returnData_consumption.JsonSerializationt();
                    string json_out_init = Net.WEBApiPostJson(com_init_url, json_in);
                    string json_out = Net.WEBApiPostJson(com_url, json_in);
                    returnData consumption = json_out.JsonDeserializet<returnData>();
                    List<consumptionClass> value = consumption.Data.ObjToClass<List<consumptionClass>>();
                    if (value == null || value.Count == 0)
                        continue;
                    value = value.Where(x => x.消耗量 != "0").ToList();
                    foreach (var consume in value)
                        consume.建立時間 = now;
                    returnData returnData_consumeAdd = consumptionClass.add(API, serverName, serverType, value);
                }
            }
        }
    }
}

