using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrape.Classi
{
    public class Lega
    {
        public int idPaese { get; set; }
        public string Nome { get; set; }
        public string Immagine { get; set; }
        //public Classifica Classifica { get; set; }
        public Lega()
        {

        }
        public Lega(int paese, string nome, string immagine)
        {
            idPaese = paese;
            Nome = nome;
            Immagine = immagine;
        }
    }
}
