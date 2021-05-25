using System.Collections.Generic;
using The_Control_Tower.Models;

namespace The_Control_Tower
{
    public class CreateAirport
    {
        /// <summary>
        /// Creates a new Aiport object.
        /// With docks and hangars
        /// </summary>
        /// <returns>Airport object</returns>
        public static Airport CreateAirport()
        {
            var runways = new List<Runway>()
            {
                new Runway()
                {
                    Name = "Arrival",
                    InUse = false
                },
                new Runway()
                {
                    Name = "Departure",
                    InUse = false
                }
            };
            var hangars = new List<Hangar>()
            {
                new Hangar()
                {
                    Name = DocksEnum.Dock1,
                    InUse = false
                },
                new Hangar()
                {
                    Name = DocksEnum.Dock2,
                    InUse = false
                },
                new Hangar()
                {
                    Name = DocksEnum.Dock3,
                    InUse = false
                },
                new Hangar()
                {
                    Name = DocksEnum.Dock4,
                    InUse = false
                },
                new Hangar()
                {
                    Name = DocksEnum.Dock5,
                    InUse = false
                }
            };
            return new Airport()
            {
                Docks = hangars,
                Runway = runways
            };
        }
    }
}