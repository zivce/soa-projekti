using Microsoft.AspNetCore.Mvc;
using SocialEvolutionSensor.Models;
using SocialEvolutionSensorAPI.Services;
using System.Collections.Generic;

namespace SocialEvolutionSensorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/SMSs")]
    public class SMSsController : Controller
    {
        private ISMSsDataService _SMSsDataService;
        public SMSsController(ISMSsDataService smsDataService)
        {
            _SMSsDataService = smsDataService;
        }
        // GET: api/SMSs
        [HttpGet]
        public IEnumerable<SMS> Get()
        {
            return _SMSsDataService.getLatest();
        }
    }
}
