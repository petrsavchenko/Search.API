using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Search.Services;

namespace Search.API.Controllers
{
    public class GenericController<TService> : ControllerBase where TService : ISearchService
    {
        protected readonly TService _service;
        public GenericController(TService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<string>> GetSearchPositions(string keywords, string url)
        {
            var result = await _service.GetSearchPositions(keywords, url);

            if (string.IsNullOrWhiteSpace(result))
            {
                return NotFound();
            }

            return result;
        }
    }
}
