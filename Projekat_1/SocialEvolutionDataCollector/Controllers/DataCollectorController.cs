using Microsoft.AspNetCore.Mvc;
using SocialEvolutionDataCollector.Services;
using SocialEvolutionSensor.Models;
using System.Collections.Generic;

namespace SocialEvolutionDataCollector.Controllers
{
    [Route("api/DataCollector")]
    [ApiController]
    public class DataCollectorController : ControllerBase
    {

        private IDataCollectorService<Call> _callCollectorService;
        private IDataCollectorService<Message> _messageCollectorService;

        public DataCollectorController(
            IDataCollectorService<Call> callCollectorService,
            IDataCollectorService<Message> messageCollectorService)
        {
            _callCollectorService = callCollectorService;
            _messageCollectorService = messageCollectorService;
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
        public ActionResult<List<Message>> GetMessages()
        {
            var res = _messageCollectorService.GetDataAsync();
            return res.Result;
        }
    }
}
