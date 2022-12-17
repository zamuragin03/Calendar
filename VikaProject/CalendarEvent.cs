using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikaProject
{
    public struct CalendarEvent
    {
        public int Event_id { get; set; }
        public DateTime event_time { get; set; }
        public DateTime deadline { get; set; }
        public string description { get; set; }
        public Color currentcolor { get; set; }


        public override string ToString()
        {
            return Event_id+ ") "+ description + " "+ "до " + deadline.ToShortDateString();
        }
    }
}
