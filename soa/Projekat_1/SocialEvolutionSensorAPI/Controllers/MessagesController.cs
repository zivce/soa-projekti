using Microsoft.AspNetCore.Mvc;
using SocialEvolutionSensor.Models;
using SocialEvolutionSensorAPI.Services;
using System.Collections.Generic;

namespace SocialEvolutionSensorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        private IMessagesDataService _MessagesDataService;
        public MessagesController(IMessagesDataService messageDataService)
        {
            _MessagesDataService = messageDataService;
        }
        // GET: api/Messages
        [HttpGet]
        public IEnumerable<Message> Get()
        {
            return _MessagesDataService.getLatest();
        }
    }
}
