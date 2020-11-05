using System;
using System.Collections.Generic;

namespace AGL.Services.DTOs
{
    public class PeopleDTO
    {
        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Pets
        /// </summary>
        public List<PetDTO> Pets { get; set; }
    }
}
