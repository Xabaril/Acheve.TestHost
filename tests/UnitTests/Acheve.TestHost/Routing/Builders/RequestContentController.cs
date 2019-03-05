using Microsoft.AspNetCore.Mvc;
using System;

namespace UnitTests.Acheve.TestHost.Builders
{
    [Route("api/content")]
    [ApiController]
    public class RequestContentController
       : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromForm]ComplexObject complexObject)
        {
            return ValidateComplexObject(complexObject);
        }

        private IActionResult ValidateComplexObject(ComplexObject complexObject)
        {
            if (string.IsNullOrEmpty(complexObject.StringParameter))
            {
                return BadRequest();
            }

            if (!complexObject.BoolParameter)
            {
                return BadRequest();
            }

            if (!complexObject.BoolNullableParameter.HasValue)
            {
                return BadRequest();
            }

            if (complexObject.IntParameter == 0)
            {
                return BadRequest();
            }

            if (!complexObject.IntNullableParameter.HasValue)
            {
                return BadRequest();
            }

            if (complexObject.ComplexParameter == null)
            {
                return BadRequest();
            }

            if (complexObject.ComplexParameter.LongParameter == 0)
            {
                return BadRequest();
            }

            if (!complexObject.ComplexParameter.LongNullableParameter.HasValue)
            {
                return BadRequest();
            }

            if (complexObject.ComplexParameter.DateTimeParameter.HasValue)
            {
                return BadRequest();
            }

            if (complexObject.ComplexParameter.Pagination == null)
            {
                return BadRequest();
            }

            if (complexObject.ComplexParameter.Pagination.PageCount == 0 || complexObject.ComplexParameter.Pagination.PageIndex == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
    }

    public class ComplexObject
    {
        public string StringParameter { get; set; }
        public bool BoolParameter { get; set; }
        public bool? BoolNullableParameter { get; set; }
        public int IntParameter { get; set; }
        public int? IntNullableParameter { get; set; }
        public DateTime DateTimeParameter { get; set; }
        public Complex ComplexParameter { get; set; }
    }

    public class Complex
    {
        public Pagination Pagination { get; set; }
        public long LongParameter { get; set; }
        public long? LongNullableParameter { get; set; }
        public DateTime? DateTimeParameter { get; set; }
    }
}
