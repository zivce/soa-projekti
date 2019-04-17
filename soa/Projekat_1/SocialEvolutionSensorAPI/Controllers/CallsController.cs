using Microsoft.AspNetCore.Mvc;
using SocialEvolutionSensor.Models;
using SocialEvolutionSensorAPI.Services;
using System.Collections.Generic;

namespace SocialEvolutionSensorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Calls")]
    public class CallsController : Controller
    {
        private ICallsDataService _callsDataService;
        public CallsController(ICallsDataService callDataService)
        {
            _callsDataService = callDataService;
        }
        // GET: api/Calls
        [HttpGet]
        public IEnumerable<Call> Get()
        {
            return _callsDataService.getLatest();
        }
    }
}
