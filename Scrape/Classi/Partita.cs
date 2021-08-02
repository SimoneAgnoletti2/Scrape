using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrape.Classi
{
    public class Partita
    {
        public int IdPaese { get; set; }
        public string Paese { get; set; }
        public int IdCampionato { get; set; }
        public string Campionato { get; set; }
        public string NomeCasa { get; set; }
        public string LinkCasa { get; set; }
        public string NomeFuori { get; set; }
        public string LinkFuori { get; set; }
        public string Orario { get; set; }
        public string Risultato { get; set; }
        public string Data { get; set; }
        public double Quota_1 { get; set; }
        public double Quota_X { get; set; }
        public double Quota_2 { get; set; }
        public double Quota_Under05 { get; set; }
        public double Quota_Over05 { get; set; }
        public double Quota_Under15 { get; set; }
        public double Quota_Over15 { get; set; }
        public double Quota_Under25 { get; set; }
        public double Quota_Over25 { get; set; }
        public double Quota_Under35 { get; set; }
        public double Quota_Over35 { get; set; }
        public double Quota_1X { get; set; }
        public double Quota_X2 { get; set; }
        public double Quota_12 { get; set; }
        public double Quota_Goal { get; set; }
        public double Quota_NoGoal { get; set; }
        //public string link { get; set; }
        public int PositionY { get; set; }

        public Partita()
        {

        }
    }
}
