using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.Services.Contracts;
using AGL.Services.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AGL.App.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _peopleService;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(ILogger<PeopleController> logger, IPeopleService peopleService)
        {
            _logger = logger;
            _peopleService = peopleService;
        }

        /// <summary>
        /// List of all the pets in alphabetical order under a heading of the gender of their owner.
        /// </summary>
        /// <param name="petType">Please enter which petType you want to get</param>
        /// <returns>Returns all the pets in alphabetical order under a heading of the gender of their owner based on supplied petType</returns>
        [HttpGet("{petType}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PeopleDTO>))]
        public async Task<IActionResult> Get(string petType)
        {
            try
            {
                if (string.IsNullOrEmpty(petType))
                {
                    return BadRequest(petType);
                }

                var peopleDto = await _peopleService.RetrievePeopleListAsync(petType);
                return Ok(peopleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller: People, Method: Get, ErrorMessage: {ex.Message}");
                throw ex;
            }
        }
    }
}
