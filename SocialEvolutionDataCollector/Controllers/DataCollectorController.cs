using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialEvolutionDataCollector.Services;
using SocialEvolutionSensor.Models;

namespace SocialEvolutionDataCollector.Controllers
{
    [Route("api/DataCollector")]
    [ApiController]
    public class DataCollectorController : ControllerBase
    {

        private IDataCollectorService<Call> _callCollectorService;
        private IDataCollectorService<SMS> _smsCollectorService;

        public DataCollectorController(
            IDataCollectorService<Call> callCollectorService,
            IDataCollectorService<SMS> smsCollectorService){
            _callCollectorService = callCollectorService;
            _smsCollectorService = smsCollectorService;
        }

        [Route("calls")]
        [HttpGet]
        public ActionResult<List<Call>> GetCalls()
        {
            var res = _callCollectorService.GetDataAsync();
            return res.Result;
        }

        [Route("messages")]
        [HttpGet]
        public ActionResult<List<SMS>> GetSmss()
        {
            var res = _smsCollectorService.GetDataAsync();
            return res.Result;
        }

    }
}
