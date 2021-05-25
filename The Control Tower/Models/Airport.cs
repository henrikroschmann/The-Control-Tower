using System.Collections.Generic;

namespace The_Control_Tower.Models
{
    public class Airport
    {
        public List<IAirplane> Airplanes { get; set; }
        public List<Hangar> Docks { get; set; }
        public List<Runway> Runway { get; set; }

        /// <summary>
        /// Creates a new Airport object.
        /// With docks and hangars
        /// </summary>
        /// <returns>Airport object</returns>
        public static Airport CreateAirport()
        {
            var runways = new List<Runway>()
            {
                new Runway()
                {
                    Name = RunwayEnum.Arrival,
                    InUse = false
                },
                new Runway()
                {
                    Name = RunwayEnum.Departure,
                    InUse = false
                }
            };
            var hangars = new List<Hangar>()
            {
                new Hangar()
                {
                    Name = DocksEnum.Dock1,
                    InUse = true
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