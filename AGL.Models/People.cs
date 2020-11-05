using System;
using System.Collections.Generic;

namespace AGL.Models
{
    public class People : BaseEntity
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Pets
        /// </summary>
        public List<Pet> Pets { get; set; }
    }
}
