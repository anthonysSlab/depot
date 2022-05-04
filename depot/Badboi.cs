using Depot.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    public class Badboi
    {
        public Badboi(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; set; }

        public List<Sacredtext> Sacredtexts { get; set; } = new();
    }
}