using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrape.Classi
{
    public class Partita
    {
        public string Home { get; set; }
        public string Away { get; set; }
        public string Result { get; set; }
        public string Schedule { get; set; }
        public string Day { get; set; }
        public string linkImage { get; set; }

        //public string link { get; set; }
        public int PositionY { get; set; }

        public Partita()
        {

        }
    }
}
