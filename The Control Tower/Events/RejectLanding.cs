using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Control_Tower.Events
{
    public class RejectLanding: EventArgs
    {
        public string Flight { get; set; }
        public RejectLanding(string flight)
        {
            Flight = flight;
        }
    }
}
