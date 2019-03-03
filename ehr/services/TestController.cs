using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EMR.WebAPI.ehr.models;

namespace EMR.WebAPI.ehr.services
{
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("api/testcors")]
        public IHttpActionResult TestCors()
        {
            ServiceRequestStatus status = new ServiceRequestStatus(true, "CORS TEST SUCCESSFUL");

            return Ok(new { results = status });
        }
    }
}
