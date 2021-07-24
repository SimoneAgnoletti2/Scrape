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

        public List<Campionato> live()
        {
            List<Partita> partite = new List<Partita>();
            List<Campionato> camp = new List<Campionato>();

            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.flashscore.com/";

            var timeout = 10000; /* Maximum wait time of 20 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

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
                Partita p = new Partita();
                p.Home = squadrecasa[i].Text;
                p.Result = risultati[i].Text;
                p.Away = squadreospite[i].Text;
                p.Schedule = orari[i].Ora.Replace("FRO", "");
                p.PositionY = squadrecasa[i].Location.Y;
                partite.Add(p);
            }

            camp = ordinaPartite(camp, partite);

            close_Browser();
            return camp;
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
