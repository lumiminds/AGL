using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.Services.DTOs;

namespace AGL.Services.Contracts
{
    public interface IPeopleService
    {
        Task<List<PeopleDTO>> RetrievePeopleListAsync(string petType);
    }
}
