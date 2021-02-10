using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<string> GetSearchPositions(string keywords, string url)
        {
            return await _service.GetSearchPositions(keywords, url);
        }
    }
}
