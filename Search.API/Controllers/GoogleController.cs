using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Search.Services;

namespace Search.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GoogleController : GenericController<IGoogleSearchService>
    {
        public GoogleController(IGoogleSearchService service) : base(service)
        {
        }
    }
}
