using System;

namespace The_Control_Tower.Events
{
    //Define Request to land event argument you want to send while raising event.
    public class FlyingAway : EventArgs
    {
        public string Flight { get; set; }

        public FlyingAway(string flight)
        {
            Flight = flight;
        }
    }
}