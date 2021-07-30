using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Scrape.Classi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

        public List<Partita> live()
        {
            List<Partita> partite = new List<Partita>();
            List<Campionato> camp = new List<Campionato>();

            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.flashscore.com/";

            var timeout = 10000; /* Maximum wait time of 20 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            Thread.Sleep(10000);

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

            By squadracasa = By.ClassName("event__participant--home");
            ReadOnlyCollection<IWebElement> squadrecasa = driver.FindElements(squadracasa);

            By risultato = By.ClassName("event__scores");
            ReadOnlyCollection<IWebElement> risultati = driver.FindElements(risultato);

            By squadraospite = By.ClassName("event__participant--away");
            ReadOnlyCollection<IWebElement> squadreospite = driver.FindElements(squadraospite);

            By time = By.ClassName("event__time");
            ReadOnlyCollection<IWebElement> times = driver.FindElements(time);

            By not_time = By.ClassName("event__stage--block");
            ReadOnlyCollection<IWebElement> not_times = driver.FindElements(not_time);

            By nazione = By.ClassName("event__title--type");
            ReadOnlyCollection<IWebElement> nazioni = driver.FindElements(nazione);

            By campionato = By.ClassName("event__title--name");
            ReadOnlyCollection<IWebElement> campionati = driver.FindElements(campionato);

            By links = By.ClassName("event__match");
            ReadOnlyCollection<IWebElement> link = driver.FindElements(links);

            List<Orario> orari = new List<Orario>();
            orari = ordinaOrari(times, not_times);

            for (int i = 0; i < nazioni.Count; i++)
            {
                Campionato c = new Campionato();
                c.Nation = nazioni[i].Text;
                c.Championship = campionati[i].Text;
                c.PositionY = campionati[i].Location.Y;
                camp.Add(c);
            }

            for (int i = 0; i < squadrecasa.Count; i++)
            {
                if (orari[i].Ora != "Postponed")
                {
                    Partita p = new Partita();
                    p.NomeCasa = squadrecasa[i].Text;
                    p.Risultato = risultati[i].Text.Replace("\n", "").Replace("\r", "");
                    p.NomeFuori = squadreospite[i].Text;
                    p.Orario = orari[i].Ora.Replace("FRO", "");
                    p.PositionY = squadrecasa[i].Location.Y;

                    if (p.Risultato == "-")
                    {

                        var linkid = link[i].GetAttribute("id");
                        driver2 = new FirefoxDriver();
                        driver2.Manage().Window.Maximize();
                        driver2.Url = "https://www.flashscore.com/match/" + linkid.Replace("g_1_", "") + "/#match-summary/match-summary";

                        timeout = 10000; /* Maximum wait time of 20 seconds */
                        wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                        Thread.Sleep(3000);

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


                        By leghediv = By.ClassName("country___24Qe-aj");
                        IWebElement legadiv = driver2.FindElement(leghediv);

                        By leghe = By.TagName("a");
                        IWebElement lega = legadiv.FindElement(leghe);

                        p.Campionato = lega.Text;

                        By linksdiv = By.ClassName("participantImage___2Oi0lJ_");
                        ReadOnlyCollection<IWebElement> linkdiv = driver2.FindElements(linksdiv);

                        By linkscasa = By.TagName("img");
                        ReadOnlyCollection<IWebElement> linkcasa = linkdiv[0].FindElements(linkscasa);

                        By linksfuori = By.TagName("img");
                        ReadOnlyCollection<IWebElement> linkfuori = linkdiv[1].FindElements(linkscasa);

                        p.LinkCasa = linkcasa[0].GetAttribute("src");
                        p.LinkFuori = linkfuori[0].GetAttribute("src");

                        By groups1 = By.ClassName("tabs__tab");
                        ReadOnlyCollection<IWebElement> group1 = driver2.FindElements(groups1);


                        for (int y = 0; y < group1.Count; y++)
                        {
                            if (group1[y].Text == "Odds")
                            {
                                group1[y].Click();
                                Thread.Sleep(2000);


                                By groups2 = By.ClassName("tabs__tab");
                                ReadOnlyCollection<IWebElement> group2 = driver2.FindElements(groups2);

                                for (int w = 0; w < group2.Count; w++)
                                {

                                    if (group2[w].Text == "1X2 odds")
                                    {

                                        By prova = By.ClassName("odd___2vKX0U5");
                                        ReadOnlyCollection<IWebElement> prove = driver2.FindElements(prova);

                                        try
                                        {
                                            p.Quota_1 = prove[0].Text;
                                            p.Quota_X = prove[1].Text;
                                            p.Quota_2 = prove[2].Text;
                                        }
                                        catch (Exception ex) { }
                                    }
                                    if (group2[w].Text == "O/U")
                                    {
                                        group2[w].Click();
                                        By prova = By.ClassName("odd___2vKX0U5");
                                        ReadOnlyCollection<IWebElement> prove = driver2.FindElements(prova);

                                        try
                                        {
                                            p.Quota_Over05 = prove[0].Text;
                                            p.Quota_Under05 = prove[1].Text;
                                            p.Quota_Over15 = prove[2].Text;
                                            p.Quota_Under15 = prove[3].Text;
                                            p.Quota_Over25 = prove[4].Text;
                                            p.Quota_Under25 = prove[5].Text;
                                            p.Quota_Over35 = prove[6].Text;
                                            p.Quota_Under35 = prove[7].Text;
                                        }
                                        catch (Exception ex) { }
                                    }
                                    if (group2[w].Text == "DC")
                                    {
                                        group2[w].Click();
                                        By prova = By.ClassName("odd___2vKX0U5");
                                        ReadOnlyCollection<IWebElement> prove = driver2.FindElements(prova);

                                        try
                                        {
                                            p.Quota_1X = prove[0].Text;
                                            p.Quota_12 = prove[1].Text;
                                            p.Quota_X2 = prove[2].Text;
                                        }
                                        catch (Exception ex) { }
                                    }
                                    if (group2[w].Text == "BTS")
                                    {
                                        group2[w].Click();
                                        By prova = By.ClassName("odd___2vKX0U5");
                                        ReadOnlyCollection<IWebElement> prove = driver2.FindElements(prova);

                                        try
                                        {
                                            p.Quota_Goal = prove[0].Text;
                                            p.Quota_NoGoal = prove[1].Text;
                                        }
                                        catch (Exception ex) { }
                                    }
                                }
                                break;
                            }
                        }
                        driver2.Quit();
                    }



                    partite.Add(p);
                }
            }

            camp = ordinaPartite(camp, partite);



            close_Browser();
            return partite;
        }



        public void close_Browser()
        {
            driver.Quit();
        }
        public List<Campionato> ordinaPartite(List<Campionato> lc, List<Partita> lp)
        {
            for (int i = 0; i < lc.Count; i++)
            {
                int partenza = lc[i].PositionY;
                int dopo;
                lc[i].Matches = new List<Partita>();
                if (i + 1 < lc.Count)
                {
                    dopo = lc[i + 1].PositionY;
                }
                else
                {
                    dopo = 10000000;
                }
                for (int p = 0; p < lp.Count; p++)
                {
                    if (lp[p].PositionY < dopo & lp[p].PositionY > partenza)
                    {
                        lc[i].Matches.Add(lp[p]);
                    }
                }
            }
            return lc;
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
