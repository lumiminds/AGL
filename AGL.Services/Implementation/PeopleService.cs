using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.Models;
using AGL.Services.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AGL.Services.DTOs;
using AutoMapper;
using AGL.Services.Shared;

namespace AGL.Services.Implementation
{
    public class PeopleService : BaseService<People>, IPeopleService
    {
        private readonly ILogger<PeopleService> _logger;

        // Services
        private readonly IHttpService _httpService;
        public PeopleService(IMapper mapper, ILogger<PeopleService> logger, IHttpService httpService) : base(mapper)
        {
            _logger = logger;

            // Services
            _httpService = httpService;
        }

        public async Task<List<PeopleDTO>> RetrievePeopleListAsync(string petType)
        {
            try
            {
                // Retrieve jsonData for httpClient names PeopleList
                var peopleList = await _httpService.RetrieveJsonDataAsync<List<People>>("PeopleList");
                // var peopleList = JsonConvert.DeserializeObject<List<People>>(jsonData);

                // Group cats by owner's gender and sort ASC
                var result = peopleList.Where(p => p.Pets != null && p.Pets.Any())
                    .GroupBy(g => g.Gender)
                    .Select(
                        group => new People
                        {
                            Gender = group.Key,
                            Pets = group.SelectMany(p => p.Pets)
                                .Where(p => p.Type.Equals(petType, StringComparison.OrdinalIgnoreCase))
                                    .Select(pet => pet)
                                        .OrderBy(pet => pet.Name).ToList()
                        }).ToList();

                return Mapper.Map<List<PeopleDTO>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}
