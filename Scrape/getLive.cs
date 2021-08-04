using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Scrape.Classi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrape
{
    class getLive
    {
        IWebDriver driver;
        IWebDriver driver2;
        List<PaeseLega> paeselega = new List<PaeseLega>();
        List<Partita> partite = new List<Partita>();
        List<Campionato> camp = new List<Campionato>();

        public void live()
        {
            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);

            getPaesiLeghe();

            driver = new FirefoxDriver();
            driver.Manage().Window.Minimize();
            driver.Url = "https://www.flashscore.com/";

            for (int giorno = 0; giorno < 7; giorno++)
            {

                var timeout = 10000; /* Maximum wait time of 20 seconds */
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                Thread.Sleep(1000);

                try
                {
                    WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    IWebElement title = wait2.Until<IWebElement>((d) =>
                    {
                        return d.FindElement(By.Id("onetrust-accept-btn-handler"));
                    });
                    Thread.Sleep(5000);

                    By policy = By.Id("onetrust-accept-btn-handler");
                    var pol = driver.FindElement(policy);
                    pol.Click();
                }
                catch (Exception ex)
                {

                }
                //tutte i nodi squadre di casa
                By squadracasa = By.ClassName("event__participant--home");
                ReadOnlyCollection<IWebElement> squadrecasa = driver.FindElements(squadracasa);

                //tutti i nodi risultati
                By risultato = By.ClassName("event__scores");
                ReadOnlyCollection<IWebElement> risultati = driver.FindElements(risultato);

                //tutti i nodi squadre ospiti
                By squadraospite = By.ClassName("event__participant--away");
                ReadOnlyCollection<IWebElement> squadreospite = driver.FindElements(squadraospite);

                //tutti i nodi squadre che devono ancora giocare o stanno giocando
                By time = By.ClassName("event__time");
                ReadOnlyCollection<IWebElement> times = driver.FindElements(time);

                //tutti i nodi squadre che hanno finito di giocare
                By not_time = By.ClassName("event__stage--block");
                ReadOnlyCollection<IWebElement> not_times = driver.FindElements(not_time);

                //tutte i nodi squadre di casa
                By nazione = By.ClassName("event__title--type");
                ReadOnlyCollection<IWebElement> nazioni = driver.FindElements(nazione);

                //prendo la parte di id per il reindirizzamento ai dettagli della partita

                //unisco partite cominciate e partite terminate o non iniziate in ordine come in pagina
                List<Orario> orari = new List<Orario>();
                orari = ordinaOrari(times, not_times);

                for (int i = 0; i < squadrecasa.Count; i++)
                {
                    By links = By.ClassName("event__match");
                    ReadOnlyCollection<IWebElement> link = driver.FindElements(links);
                    //apro la pagina dettaglio di ogni partita
                    var linkid = link[i].GetAttribute("id");


                    if(PartitaDaScaricare(linkid.Replace("g_1_", "")))
                    {
                        if (orari[i].Ora != "Postponed" && orari[i].Ora != "Cancelled")
                        {
                            Partita p = new Partita();
                            p.NomeCasa = squadrecasa[i].Text;
                            p.Risultato = risultati[i].Text.Replace("\n", "").Replace("\r", "");
                            p.NomeFuori = squadreospite[i].Text;
                            p.Orario = orari[i].Ora.Replace("FRO", "");
                            if(p.Orario == "Finished" || p.Orario == "After Pen.")
                            {
                                p.Stato = "FINITA";
                            }
                            else if(p.Risultato == "-" || p.Risultato == "PREVIEW" || p.Risultato == "")
                            {
                                p.Stato = "DA_INIZIARE";
                                p.Risultato = "-";
                            }
                            else
                            {
                                p.Stato = "IN_CORSO";
                            }
                            p.PositionY = squadrecasa[i].Location.Y;

                            //if (p.Risultato == "-" || p.Orario == "Finished" || p.Orario == "After Pen.")
                            //{ 

                            driver2 = new FirefoxDriver();
                            driver2.Manage().Window.Minimize();
                            driver2.Url = "https://www.flashscore.com/match/" + linkid.Replace("g_1_", "") + "/#match-summary/match-summary";

                            timeout = 10000; /* Maximum wait time of 20 seconds */
                            wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));



                            try
                            {
                                WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                IWebElement title = wait2.Until<IWebElement>((d) =>
                                {
                                    return d.FindElement(By.Id("onetrust-accept-btn-handler"));
                                });
                                Thread.Sleep(1000);

                                By policy = By.Id("onetrust-accept-btn-handler");
                                var pol = driver.FindElement(policy);
                                pol.Click();
                            }
                            catch (Exception ex)
                            {

                            }

                            //prendo il nodo che mi dice il nome campionato
                            By leghediv = By.ClassName("country___24Qe-aj");
                            IWebElement legadiv = driver2.FindElement(leghediv);
                            p.Paese = legadiv.Text.Split(':')[0];

                            By leghe = By.TagName("a");
                            IWebElement lega = legadiv.FindElement(leghe);
                            p.Campionato = lega.Text;
                            p.IdDiv = linkid.Replace("g_1_", "");

                            //prendo le due immagini delle squadre che stanno giocando
                            By linksdiv = By.ClassName("participantImage___2Oi0lJ_");
                            ReadOnlyCollection<IWebElement> linkdiv = driver2.FindElements(linksdiv);
                            By linkscasa = By.TagName("img");
                            ReadOnlyCollection<IWebElement> linkcasa = linkdiv[0].FindElements(linkscasa);
                            By linksfuori = By.TagName("img");
                            ReadOnlyCollection<IWebElement> linkfuori = linkdiv[1].FindElements(linkscasa);
                            p.LinkCasa = linkcasa[0].GetAttribute("src");
                            p.LinkFuori = linkfuori[0].GetAttribute("src");

                            //prendo le tab nel dettaglio (per cercare le odds)
                            By groups1 = By.ClassName("tabs__tab");
                            ReadOnlyCollection<IWebElement> group1 = driver2.FindElements(groups1);

                            //prendo data e ora avvenimento
                            By dataora = By.ClassName("startTime___2oy0czV");
                            IWebElement dateora = driver2.FindElement(dataora);
                            p.Data = dateora.Text.Split(' ')[0];


                            for (int y = 0; y < group1.Count; y++)
                            {
                                //apro le odds
                                if (group1[y].Text == "Odds")
                                {
                                    group1[y].Click();

                                    //adesso riprendo tutte le tab perchè dopo il click su odds ne sono comparse di nuove
                                    By groups2 = By.ClassName("tabs__tab");
                                    ReadOnlyCollection<IWebElement> group2 = driver2.FindElements(groups2);


                                    for (int w = 0; w < group2.Count; w++)
                                    {
                                        if (group2[w].Text == "1X2 odds")
                                        {
                                            //prendo tutti nodi con le quote 1 x 2 e popolo le prime 3 quote che trovo 
                                            By prova = By.ClassName("odd___2vKX0U5");
                                            ReadOnlyCollection<IWebElement> prove = driver2.FindElements(prova);

                                            try
                                            {
                                                p.Quota_1 = Double.Parse(prove[0].Text, CultureInfo.InvariantCulture);
                                                p.Quota_X = Double.Parse(prove[1].Text, CultureInfo.InvariantCulture);
                                                p.Quota_2 = Double.Parse(prove[2].Text, CultureInfo.InvariantCulture);
                                            }
                                            catch (Exception ex)
                                            {
                                                p.Quota_1 = 0;
                                                p.Quota_X = 0;
                                                p.Quota_2 = 0;
                                            }
                                        }

                                        //prendo tutti nodi con le quote under e over e popolo le prime 8 che trovo (da rivedere)
                                        if (group2[w].Text == "O/U")
                                        {
                                            group2[w].Click();
                                            By prova = By.ClassName("odd___2vKX0U5");
                                            ReadOnlyCollection<IWebElement> prove = driver2.FindElements(prova);

                                            try
                                            {
                                                p.Quota_Over05 = Double.Parse(prove[0].Text, CultureInfo.InvariantCulture);
                                                p.Quota_Under05 = Double.Parse(prove[1].Text, CultureInfo.InvariantCulture);
                                                p.Quota_Over15 = Double.Parse(prove[2].Text, CultureInfo.InvariantCulture);
                                                p.Quota_Under15 = Double.Parse(prove[3].Text, CultureInfo.InvariantCulture);
                                                p.Quota_Over25 = Double.Parse(prove[4].Text, CultureInfo.InvariantCulture);
                                                p.Quota_Under25 = Double.Parse(prove[5].Text, CultureInfo.InvariantCulture); p.Quota_Under25 = (float)Convert.ToDouble(prove[5].Text);
                                                p.Quota_Over35 = Double.Parse(prove[6].Text, CultureInfo.InvariantCulture);
                                                p.Quota_Under35 = Double.Parse(prove[7].Text, CultureInfo.InvariantCulture);
                                            }
                                            catch (Exception ex)
                                            {
                                                p.Quota_Over05 = 0;
                                                p.Quota_Under05 = 0;
                                                p.Quota_Over15 = 0;
                                                p.Quota_Under15 = 0;
                                                p.Quota_Over25 = 0;
                                                p.Quota_Under25 = 0;
                                                p.Quota_Over35 = 0;
                                                p.Quota_Under35 = 0;
                                            }
                                        }

                                        //prendo tutti nodi con le quote doppie chance e popolo le prime 3 quote che trovo 
                                        if (group2[w].Text == "DC")
                                        {
                                            group2[w].Click();
                                            By prova = By.ClassName("odd___2vKX0U5");
                                            ReadOnlyCollection<IWebElement> prove = driver2.FindElements(prova);

                                            try
                                            {
                                                p.Quota_1X = Double.Parse(prove[0].Text, CultureInfo.InvariantCulture);
                                                p.Quota_12 = Double.Parse(prove[1].Text, CultureInfo.InvariantCulture);
                                                p.Quota_X2 = Double.Parse(prove[2].Text, CultureInfo.InvariantCulture);
                                            }
                                            catch (Exception ex)
                                            {
                                                p.Quota_1X = 0;
                                                p.Quota_12 = 0;
                                                p.Quota_X2 = 0;
                                            }
                                        }

                                        //prendo tutti nodi con le quote goal nogoal e popolo le prime 2 quote che trovo 
                                        if (group2[w].Text == "BTS")
                                        {
                                            group2[w].Click();
                                            By prova = By.ClassName("odd___2vKX0U5");
                                            ReadOnlyCollection<IWebElement> prove = driver2.FindElements(prova);

                                            try
                                            {
                                                p.Quota_Goal = Double.Parse(prove[0].Text, CultureInfo.InvariantCulture);
                                                p.Quota_NoGoal = Double.Parse(prove[1].Text, CultureInfo.InvariantCulture);
                                            }
                                            catch (Exception ex)
                                            {
                                                p.Quota_Goal = 0;
                                                p.Quota_NoGoal = 0;
                                            }
                                        }
                                    }

                                    //metto un break perchè ho già preso tutto ciò che mi interessa ignorando le altre tabs
                                    break;
                                }
                            }
                            var scarica = verificaPartiteDaScaricare(linkid.Replace("g_1_", ""));
                            if (!scarica)
                            {
                                aggiornaInDatabase(p);
                            }
                            else
                            {
                                inserisciInDatabase(p);
                            }


                            driver2.Quit();
                            driver2.Dispose();

                            //}
                        }
                    }
                }
                By next = By.ClassName("calendar__direction--tomorrow");
                IWebElement nex = driver.FindElement(next);
                nex.Click();

                if (giorno == 6)
                {
                    close_Browser();
                }
            }
        }

        public void inserisciInDatabase(Partita partita)
        {
            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    int campionato = 0;
                    foreach (var nomelega in paeselega)
                    {
                        if (partita.Campionato == nomelega.Campionato && partita.Paese.ToLower() == nomelega.Paese.ToLower())
                        {
                            campionato = nomelega.IdCampionato;
                        }
                        else if (partita.Campionato.Contains(nomelega.Campionato) && partita.Paese.ToLower() == nomelega.Paese.ToLower())
                        {
                            campionato = nomelega.IdCampionato;
                        }
                        else if (nomelega.Campionato.Contains(partita.Campionato) && partita.Paese.ToLower() == nomelega.Paese.ToLower())
                        {
                            campionato = nomelega.IdCampionato;
                        }
                    }
                    SqlCommand command;

                    command = new SqlCommand("INSERT INTO Partita (id_Lega, Stato,idDiv,NomeCasa,LinkCasa,NomeFuori,LinkFuori," +
                                                               "Orario,Risultato,Data,Quota_1,Quota_X,Quota_2,Quota_Under05," +
                                                               "Quota_Over05,Quota_Under15,Quota_Over15,Quota_Under25," +
                                                               "Quota_Over25,Quota_Under35,Quota_Over35,Quota_1X,Quota_X2," +
                                                               "Quota_12,Quota_Goal,Quota_NoGoal) " +
                                                               "values " +
                                                               "(@id_Lega,@Stato,@idDiv,@NomeCasa,@LinkCasa,@NomeFuori,@LinkFuori," +
                                                               "@Orario,@Risultato,@Data,@Quota_1,@Quota_X,@Quota_2,@Quota_Under05," +
                                                               "@Quota_Over05,@Quota_Under15,@Quota_Over15,@Quota_Under25," +
                                                               "@Quota_Over25,@Quota_Under35,@Quota_Over35,@Quota_1X,@Quota_X2," +
                                                               "@Quota_12,@Quota_Goal,@Quota_NoGoal)", connection);
                    command.Parameters.AddWithValue("@id_Lega", campionato);
                    command.Parameters.AddWithValue("@Stato", partita.Stato);
                    command.Parameters.AddWithValue("@idDiv", partita.IdDiv);
                    command.Parameters.AddWithValue("@NomeCasa", partita.NomeCasa);
                    command.Parameters.AddWithValue("@LinkCasa", partita.LinkCasa);
                    command.Parameters.AddWithValue("@NomeFuori", partita.NomeFuori);
                    command.Parameters.AddWithValue("@LinkFuori", partita.LinkFuori);
                    command.Parameters.AddWithValue("@Orario", partita.Orario);
                    command.Parameters.AddWithValue("@Risultato", partita.Risultato);
                    command.Parameters.AddWithValue("@Data", partita.Data);
                    command.Parameters.AddWithValue("@Quota_1", partita.Quota_1);
                    command.Parameters.AddWithValue("@Quota_X", partita.Quota_X);
                    command.Parameters.AddWithValue("@Quota_2", partita.Quota_2);
                    command.Parameters.AddWithValue("@Quota_Under05", partita.Quota_Under05);
                    command.Parameters.AddWithValue("@Quota_Over05", partita.Quota_Over05);
                    command.Parameters.AddWithValue("@Quota_Under15", partita.Quota_Under15);
                    command.Parameters.AddWithValue("@Quota_Over15", partita.Quota_Over15);
                    command.Parameters.AddWithValue("@Quota_Under25", partita.Quota_Under25);
                    command.Parameters.AddWithValue("@Quota_Over25", partita.Quota_Over25);
                    command.Parameters.AddWithValue("@Quota_Under35", partita.Quota_Under35);
                    command.Parameters.AddWithValue("@Quota_Over35", partita.Quota_Over35);
                    command.Parameters.AddWithValue("@Quota_1X", partita.Quota_1X);
                    command.Parameters.AddWithValue("@Quota_X2", partita.Quota_X2);
                    command.Parameters.AddWithValue("@Quota_12", partita.Quota_12);
                    command.Parameters.AddWithValue("@Quota_Goal", partita.Quota_Goal);
                    command.Parameters.AddWithValue("@Quota_NoGoal", partita.Quota_NoGoal);
                    command.ExecuteNonQuery();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            connection.Close();
        }

        public void aggiornaInDatabase(Partita partita)
        {
            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    int campionato = 0;
                    foreach (var nomelega in paeselega)
                    {
                        if (partita.Campionato == nomelega.Campionato && partita.Paese.ToLower() == nomelega.Paese.ToLower())
                        {
                            campionato = nomelega.IdCampionato;
                        }
                        else if (partita.Campionato.Contains(nomelega.Campionato) && partita.Paese.ToLower() == nomelega.Paese.ToLower())
                        {
                            campionato = nomelega.IdCampionato;
                        }
                        else if (nomelega.Campionato.Contains(partita.Campionato) && partita.Paese.ToLower() == nomelega.Paese.ToLower())
                        {
                            campionato = nomelega.IdCampionato;
                        }
                    }
                    SqlCommand command;
                    string query = "UPDATE Partita SET " +
                        "id_Lega = @id_Lega" +
                        "Stato = @Stato" +
                        ",idDiv = @idDiv" +
                        ",NomeCasa = @NomeCasa" +
                        ",LinkCasa = @LinkCasa" +
                        ",NomeFuori = @NomeFuori" +
                        ",LinkFuori = @LinkFuori" +
                        ",Orario = @Orario" +
                        ",Risultato = @Risultato" +
                        ",Data = @Data" +
                        ",Quota_1 = @Quota_1" +
                        ",Quota_X = @Quota_X" +
                        ",Quota_2 = @Quota_2" +
                        ",Quota_Under05 = @Quota_Under05" +
                        ",Quota_Over05 = @Quota_Over05" +
                        ",Quota_Under15 = @Quota_Under15" +
                        ",Quota_Over15 = @Quota_Over15" +
                        ",Quota_Under25 = @Quota_Under25" +
                        ",Quota_Over25 = @Quota_Over25" +
                        ",Quota_Under35 = @Quota_Under35" +
                        ",Quota_Over35 = Quota_Over35" +
                        ",Quota_1X = @Quota_1X" +
                        ",Quota_X2 = @Quota_X2," +
                        "Quota_12 = @Quota_12" +
                        ",Quota_Goal = @Quota_Goal" +
                        ",Quota_NoGoal = @Quota_NoGoal" +
                        " WHERE idDiv = '" + partita.IdDiv +  "'";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_Lega", campionato);
                    command.Parameters.AddWithValue("@Stato", partita.Stato);
                    command.Parameters.AddWithValue("@idDiv", partita.IdDiv);
                    command.Parameters.AddWithValue("@NomeCasa", partita.NomeCasa);
                    command.Parameters.AddWithValue("@LinkCasa", partita.LinkCasa);
                    command.Parameters.AddWithValue("@NomeFuori", partita.NomeFuori);
                    command.Parameters.AddWithValue("@LinkFuori", partita.LinkFuori);
                    command.Parameters.AddWithValue("@Orario", partita.Orario);
                    command.Parameters.AddWithValue("@Risultato", partita.Risultato);
                    command.Parameters.AddWithValue("@Data", partita.Data);
                    command.Parameters.AddWithValue("@Quota_1", partita.Quota_1);
                    command.Parameters.AddWithValue("@Quota_X", partita.Quota_X);
                    command.Parameters.AddWithValue("@Quota_2", partita.Quota_2);
                    command.Parameters.AddWithValue("@Quota_Under05", partita.Quota_Under05);
                    command.Parameters.AddWithValue("@Quota_Over05", partita.Quota_Over05);
                    command.Parameters.AddWithValue("@Quota_Under15", partita.Quota_Under15);
                    command.Parameters.AddWithValue("@Quota_Over15", partita.Quota_Over15);
                    command.Parameters.AddWithValue("@Quota_Under25", partita.Quota_Under25);
                    command.Parameters.AddWithValue("@Quota_Over25", partita.Quota_Over25);
                    command.Parameters.AddWithValue("@Quota_Under35", partita.Quota_Under35);
                    command.Parameters.AddWithValue("@Quota_Over35", partita.Quota_Over35);
                    command.Parameters.AddWithValue("@Quota_1X", partita.Quota_1X);
                    command.Parameters.AddWithValue("@Quota_X2", partita.Quota_X2);
                    command.Parameters.AddWithValue("@Quota_12", partita.Quota_12);
                    command.Parameters.AddWithValue("@Quota_Goal", partita.Quota_Goal);
                    command.Parameters.AddWithValue("@Quota_NoGoal", partita.Quota_NoGoal);
                    command.ExecuteNonQuery();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            connection.Close();
        }

        public void getPaesiLeghe()
        {
            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "select P.id as idPaese, P.Nome as NomePaese, L.id as idLega, L.Nome as NomeLega from Lega L inner join Paese P on P.id = L.idPaese";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PaeseLega l = new PaeseLega();
                            l.IdPaese = reader.GetInt32(0);
                            l.Paese = reader.GetString(1);
                            l.IdCampionato = reader.GetInt32(2);
                            l.Campionato = reader.GetString(3);
                            paeselega.Add(l);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            connection.Close();
        }

        public bool verificaPartiteDaScaricare(string div)
        {
            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT count(*) FROM Partita WHERE idDiv = '" + div + "'";// AND (Orario <> 'Finished' OR Orario <> 'After Pen.' OR Orario <> 'Postponed' OR Orario <> 'Cancelled')";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int sino = reader.GetInt32(0);
                            if (sino > 0)
                                return false;
                            else
                                return true;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            connection.Close();
            return false;
        }

        public bool PartitaDaScaricare(string div)
        {
            SqlConnection connection;
            string connectionString;
            connectionString = @"Data Source=pinexp.ns0.it\MIOSERVER,65004;" + "Initial Catalog=Soccer;" + @"User id=sa;" + "Password=Pinexp93;";
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "SELECT count(*) FROM Partita WHERE idDiv = '" + div + "' AND (Orario <> 'Finished' OR Orario <> 'After Pen.')";// AND (Orario <> 'Finished' OR Orario <> 'After Pen.' OR Orario <> 'Postponed' OR Orario <> 'Cancelled')";
                    SqlCommand command = new SqlCommand(query, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int sino = reader.GetInt32(0);
                            if (sino > 0)
                                return false;
                            else
                                return true;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            connection.Close();
            return false;
        }

        public void close_Browser()
        {
            driver.Quit();
        }

        public List<Orario> ordinaOrari(ReadOnlyCollection<IWebElement> t, ReadOnlyCollection<IWebElement> nt)
        {
            List<Orario> lsora = new List<Orario>();

            for (int i = 0; i < t.Count; i++)
            {
                Orario ora = new Orario();
                ora.Ora = t[i].Text;
                ora.LocationY = t[i].Location.Y;
                lsora.Add(ora);
            }

            for (int i = 0; i < nt.Count; i++)
            {
                Orario ora = new Orario();
                ora.Ora = nt[i].Text;
                ora.LocationY = nt[i].Location.Y;
                lsora.Add(ora);
            }
            lsora = lsora.OrderBy(ora => ora.LocationY).ToList();
            return lsora;
        }
    }
}
