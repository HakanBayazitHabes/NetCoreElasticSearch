using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDto<T> response)
        {
            if (response.Status == System.Net.HttpStatusCode.NoContent)
                return new ObjectResult(null) { StatusCode = response.Status.GetHashCode() };

            return new ObjectResult(response) { StatusCode = response.Status.GetHashCode() };
        }
    }
}