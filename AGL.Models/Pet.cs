using System;
namespace AGL.Models
{
    public class Pet : BaseEntity
    {
        /// <summary>
        /// Pet Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pet Type
        /// </summary>
        public string Type { get; set; }
    }
}
