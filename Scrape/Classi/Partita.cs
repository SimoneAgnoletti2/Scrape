using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrape.Classi
{
    public class Partita
    {
        public string Campionato { get; set; }
        public string NomeCasa { get; set; }
        public string LinkCasa { get; set; }
        public string NomeFuori { get; set; }
        public string LinkFuori { get; set; }
        public string Orario { get; set; }
        public string Risultato { get; set; }
        public string Data { get; set; }
        public string Quota_1 { get; set; }
        public string Quota_X { get; set; }
        public string Quota_2 { get; set; }
        public string Quota_Under05 { get; set; }
        public string Quota_Over05 { get; set; }
        public string Quota_Under15 { get; set; }
        public string Quota_Over15 { get; set; }
        public string Quota_Under25 { get; set; }
        public string Quota_Over25 { get; set; }
        public string Quota_Under35 { get; set; }
        public string Quota_Over35 { get; set; }
        public string Quota_1X { get; set; }
        public string Quota_X2 { get; set; }
        public string Quota_12 { get; set; }
        public string Quota_Goal { get; set; }
        public string Quota_NoGoal { get; set; }
        //public string link { get; set; }
        public int PositionY { get; set; }

        public Partita()
        {

        }
    }
}
