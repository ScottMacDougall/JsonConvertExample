using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public IActionResult Get(bool ignoreNull = false, bool excludeLastName = false)
        {
            var settings = new JsonSerializerSettings{
                //DefaultValueHandling can be used to not serialize null properties
               DefaultValueHandling = ignoreNull ? DefaultValueHandling.Ignore : DefaultValueHandling.Include,
               ContractResolver = excludeLastName ? ShouldSerializeContractResolver.Instance : null
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
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        public new static readonly ShouldSerializeContractResolver Instance = new ShouldSerializeContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(Person) && property.PropertyName == nameof(Person.LastName))
            {
                property.ShouldSerialize =
                    instance =>
                    {
                        return false;
                    };
            }

            return property;
        }
    }
    
}
