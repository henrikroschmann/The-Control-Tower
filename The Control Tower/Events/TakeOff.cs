using System;
using The_Control_Tower.Models;

namespace The_Control_Tower.Events
{
    //Define Take Off event argument you want to send while raising event.
    public class TakeOff : EventArgs
    {
        public string FlightCode { get; set; }
        public DocksEnum Dock { get; set; }

        public TakeOff(string flightCode, DocksEnum dock)
        {
            FlightCode = flightCode;
            Dock = dock;
        }
    }
}