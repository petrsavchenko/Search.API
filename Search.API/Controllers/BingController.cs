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
    public class BingController : GenericController<IBingSearchService>
    {
        public BingController(IBingSearchService service) : base(service)
        {
        }
    }
}
