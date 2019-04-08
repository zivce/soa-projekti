using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialEvolutionSensor.Models;
using SocialEvolutionSensorAPI.Services;

namespace SocialEvolutionSensorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/SMSs")]
    public class SMSsController : Controller
    {
        private ISMSsDataService _SMSsDataService;
        public SMSsController(ISMSsDataService smsDataService) {
            _SMSsDataService = smsDataService;
        }
        // GET: api/SMSs
        [HttpGet]
        public IEnumerable<SMS> Get()
        {
            return _SMSsDataService.getLatest();
        }

        // GET: api/SMSs/5
        [HttpGet("{id}", Name = "GetSMS")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/SMSs
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/SMSs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
