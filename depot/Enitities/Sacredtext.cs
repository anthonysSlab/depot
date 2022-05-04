using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot.Enitities
{
    public class Sacredtext
    {
        public Sacredtext(DateTime timestamp, string comment)
        {
            Timestamp = timestamp;
            Comment = comment;
        }

        public DateTime Timestamp { get; set; }
        public string Comment { get; set; }
    }
}