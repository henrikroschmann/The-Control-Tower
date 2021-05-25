using System;

namespace The_Control_Tower.Models
{
    public class Hangar
    {
        /// <summary>
        /// Name of hangar
        /// </summary>
        public DocksEnum Name { get; set; }

        /// <summary>
        /// Is the hangar in use
        /// </summary>
        public Boolean InUse { get; set; }
    }
}