using System;
using The_Control_Tower.Models;

namespace The_Control_Tower.Events
{
    public class Land : EventArgs
    {
        public string FlightCode { get; set; }
        public DocksEnum Dock { get; set; }

        public Land(string flightCode, DocksEnum dock)
        {
            FlightCode = flightCode;
            Dock = dock;
        }
    }
}