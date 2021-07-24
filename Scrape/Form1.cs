
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Scrape.Classi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Scrape
{
    public partial class Form1 : Form
    {
        ILogger log;
        public Form1()
        {
            InitializeComponent();
            log = NullLogger.Instance;

        }
        private void Live_Click(object sender, EventArgs e)
        {
            log.LogInformation("Click riempi partite di oggi");

            getLive fd = new getLive();
            List<Campionato> campionati = new List<Campionato>();
            campionati = fd.live();

            log.LogInformation(string.Format("{0} {1}", "Partite recuperate: ", campionati.Count));

            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    log.LogInformation(@"Connessione a pinexp.ns0.it\MIOSERVER,65004 riuscita");

                    SqlCommand cmd = new SqlCommand("TRUNCATE TABLE Match", connection);
                    cmd.ExecuteNonQuery();

                    for (int i = 0; i < campionati.Count; i++)
                    {
                        foreach (Partita partita in campionati[i].Matches)
                        {
                            cmd = new SqlCommand("INSERT INTO Match (Home,Away,Result,Schedule,Nation,Championship, Day) values(@Home,@Away,@Result,@Schedule,@Nation,@Championship, @Day)", connection);
                            cmd.Parameters.AddWithValue("@Home", partita.Home);
                            cmd.Parameters.AddWithValue("@Away", partita.Away);
                            cmd.Parameters.AddWithValue("@Result", partita.Result);
                            cmd.Parameters.AddWithValue("@Schedule", partita.Schedule);
                            cmd.Parameters.AddWithValue("@Nation", campionati[i].Nation);
                            cmd.Parameters.AddWithValue("@Championship", campionati[i].Championship);
                            cmd.Parameters.AddWithValue("@Day", DateTime.Today.ToString());
                            cmd.ExecuteNonQuery();

                            log.LogInformation(string.Format("{0}: {1} - {2} ", "Inserita partita", partita.Home, partita.Away));
                        }
                    }
                }
                else
                {
                    log.LogError("Connessione non riuscita");
                }
            }
            catch (Exception ex)
            {
                log.LogError("Eccezione: " + ex.Message);
            }

            connection.Close();
        }

        private void Classifiche_Click(object sender, EventArgs e)
        {
            getClassifiche fd = new getClassifiche();
            fd.classifiche();
        }

        private void DettagliPartite_Click(object sender, EventArgs e)
        {
            getDettaglio fd = new getDettaglio();
            fd.dettaglio();
        }

        private void ClassificheProva_Click(object sender, EventArgs e)
        {
            getClassifiche fd = new getClassifiche();
            fd.classifiche();
        }

        private void Paesi_Click(object sender, EventArgs e)
        {
            getPaesi fd = new getPaesi();
            List<Paese> paesi = new List<Paese>();
            paesi = fd.paesi();

            log.LogInformation("Click paesi");



            log.LogInformation(string.Format("{0} {1}", "Paesi recuperati: ", paesi.Count));

            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    log.LogInformation(@"Connessione a pinexp.ns0.it\MIOSERVER,65004 riuscita");

                    SqlCommand cmd = new SqlCommand("TRUNCATE TABLE Paese", connection);
                    cmd.ExecuteNonQuery();

                    foreach (Paese p in paesi)
                    {
                        cmd = new SqlCommand("INSERT INTO Paese (Nome,LinkImage,Valida,Preferita) values(@Nome,@LinkImage,@Valida, @Preferita)", connection);
                        cmd.Parameters.AddWithValue("@Nome", p.Nome);
                        cmd.Parameters.AddWithValue("@LinkImage", p.Link);
                        cmd.Parameters.AddWithValue("@Valida", "1");
                        if(p.Nome == "Argentina" || p.Nome == "Belgio" || p.Nome == "Brasile" || p.Nome == "Francia" 
                        || p.Nome == "Francia" || p.Nome == "Germania" || p.Nome == "Italia" || p.Nome == "Olanda" 
                        || p.Nome == "Portogallo" || p.Nome == "Inghilterra")
                            cmd.Parameters.AddWithValue("@Preferita", 1);
                        else
                        {
                            cmd.Parameters.AddWithValue("@Preferita", 0);
                        }
                        cmd.ExecuteNonQuery();

                        log.LogInformation(string.Format("{0}: {1} ", "Inserito paese", p.Nome));
                    }
                }
                else
                {
                    log.LogError("Connessione non riuscita");
                }
            }
            catch (Exception ex)
            {
                log.LogError("Eccezione: " + ex.Message);
            }

            connection.Close();
        }
    }
}
