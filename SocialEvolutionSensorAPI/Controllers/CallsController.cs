using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialEvolutionSensor.Models;

namespace SocialEvolutionSensorAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Calls")]
    public class CallsController : Controller
    {
        private static List<Call> getCalls()
        {
            List<Call> calls = new List<Call>();
            using (var reader = new StreamReader("..\\..\\..\\SensorData\\Calls.csv"))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Read();
                while (csv.Read())
                {
                    var record = csv.GetRecord<Call>();
                    calls.Add(record);
                }
            }
            return calls;
        }

        // GET: api/Calls
        [HttpGet]
        public IEnumerable<Call> Get()
        {
            return getCalls();
        }

        // GET: api/Calls/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Calls
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Calls/5
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
