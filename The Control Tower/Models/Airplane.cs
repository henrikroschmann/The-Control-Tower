using System;
using System.Globalization;

namespace The_Control_Tower.Models
{
    public class Airplane : IAirplane
    {
        /// <summary>
        /// Flight Code
        /// </summary>
        public string FlightCode { get; set; }

        public Status Statuses { get; set; }
        public string Time { get; set; }

        public DocksEnum Dock { get; set; }

        public RunwayEnum Runway { get; set; }

        public Airplane CreateAirplane(Status status = Status.RequestToLand, DocksEnum docks = DocksEnum.None)
        {
            return new Airplane()
            {
                FlightCode = CreateFlightCode(),
                Statuses = status,
                Time = GetTime(),
                Dock = docks
            };
        }

        private static string CreateFlightCode()
        {
            var rnd = new Random();
            var value = rnd.Next(1000);
            var code = value.ToString("000");

            string[] airlines = { "KLM", "SAS", "LUF", "ABC" };
            rnd = new Random();
            return $"{airlines[rnd.Next(airlines.Length)]} {code}";
        }

        private string GetTime()
        {
            return DateTime.UtcNow.ToString("mm:ss",
                CultureInfo.InvariantCulture);
        }
    }
}