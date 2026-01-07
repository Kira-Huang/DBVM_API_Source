using Basic;
using DBVM_API.Services;
using HIS_DB_Lib;
using IBM.Data.DB2.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace DB2VM
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly HospitalApiService _hospitalApi;

        public TestController(HospitalApiService hospitalApi)
        {
            _hospitalApi = hospitalApi;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            try
            {
                var response = _hospitalApi.GetCardUsers();

                return $"HIS API Connecting success! {response.Result.Success}";
            }
            catch (Exception ex)
            {
                return $"HIS API  Connecting failed! , {ex.GetType().Name} : {ex.Message}";
            }
        }
    }
}
