using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrape.Classi
{
    public class Campionato
    {
        public string Nation { get; set; }
        public string Championship { get; set; }
        public int PositionY { get; set; }
        public string Giorno { get; set; }
        public List<Partita> Matches { get; set; }
        public Campionato()
        {
        }
    }
}
