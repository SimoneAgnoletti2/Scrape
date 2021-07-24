using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrape.Classi
{
    public class Classifica
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public List<string> Squadra { get; set; }
        public List<string> Scudetto { get; set; }
        public List<int> Posizione { get; set; }
        public List<int> Punti { get; set; }
        public List<int> PartiteGiocate { get; set; }
        public List<int> Vittorie { get; set; }
        public List<int> Pareggi { get; set; }
        public List<int> Sconfitte { get; set; }
        public List<string> Goals { get; set; }
        public Classifica()
        {
            Squadra = new List<string>();
            Scudetto = new List<string>();
            Posizione = new List<int>();
            Punti = new List<int>();
            PartiteGiocate = new List<int>();
            Vittorie = new List<int>();
            Pareggi = new List<int>();
            Sconfitte = new List<int>();
            Goals = new List<string>();
        }
    }
}
