using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallStatisticsService.Models;
using CallStatisticsService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CallStatisticsService.Controllers
{
    [Route("api/call-events")]
    [ApiController]
    public class CallEventsController
    {

        private ICallEventsService _callEventsService;

        public CallEventsController(ICallEventsService callEventsService)
        {
            _callEventsService = callEventsService;
        }

        [Route("latest")]
        [HttpGet]
        public ActionResult<CallEvent> GetLatestEvent()
        {
            var res = _callEventsService.GetLatestEventAsync();
            return res.Result;
        }

        [HttpGet]
        public ActionResult<List<CallEvent>> Get()
        {
            var res = _callEventsService.GetEventsAsync();
            return res.Result;
        }

    }
}
