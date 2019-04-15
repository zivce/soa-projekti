using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialEvolutionDataCollector.Services;

namespace SocialEvolutionDataCollector.Controllers
{
    [Route("api/DataCollector")]
    [ApiController]
    public class DataCollectorController : ControllerBase
    {
        private string _endpointURL = "http://localhost:55834/api/Calls";

        private IDataCollectorService _dataCollectorService;

        public DataCollectorController(IDataCollectorService dataCollectorService){
            _dataCollectorService = dataCollectorService;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var collectedData = _dataCollectorService.CollectData(_endpointURL);
            var res = _dataCollectorService.PersistData(collectedData);
            return "Radiii";
        }
    }
}
