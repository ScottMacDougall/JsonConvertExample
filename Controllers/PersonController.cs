using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonConvertExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(bool ignoreNull = false)
        {
            var settings = new JsonSerializerSettings{
                //DefaultValueHandling can be used to not serialize null properties
               DefaultValueHandling = ignoreNull ? DefaultValueHandling.Ignore : DefaultValueHandling.Include
           };
           
           //Person has 2 addresses. WorkAddress is null, so will be excluded when ignoreNull == true
            return new JsonResult(new Person
            {
               FirstName = "John",
               LastName = "Smith",
               //Address has Line3 property that is also excluded when ignoreNull == true
               Address = new Address
               {
                   Line1 = "123 Main St.",
                   Line2 = "Unit 1001"
               }
            }, settings);
        }
    }
}
