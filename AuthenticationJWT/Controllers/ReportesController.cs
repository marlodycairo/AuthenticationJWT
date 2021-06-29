using AuthenticationJWT.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllReportes()
        {
            string[] reportes = new string[] { "Presentación", "Presentación2", "Presentación3", "Presentación4" };

            return reportes;
        }
    }
}
