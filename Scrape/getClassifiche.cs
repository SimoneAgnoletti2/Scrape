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
    class getClassifiche
    {
        IWebDriver driver;
        WebDriverWait wait;
        public void classifiche()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.flashscore.it/";

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
                Thread.Sleep(10000);

                By policy = By.Id("onetrust-accept-btn-handler");
                var pol = driver.FindElement(policy);
                pol.Click();
            }
            catch (Exception ex)
            {

            }

            List<Lega> lsc = new List<Lega>();

            lsc = clickClassifica();

            close_Browser();
        }

        public void close_Browser()
        {
            driver.Quit();
        }



        public List<Lega> clickClassifica()
        {
            List<Lega> lsc = new List<Lega>();

            By more = By.ClassName("itemMore___iBgyToa");
            var mores = driver.FindElement(more);

            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement title = wait2.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.ClassName("itemMore___iBgyToa"));
            });
            Thread.Sleep(10000);

            mores.Click();


            List<string> ls = new List<string>();


            By classifica = By.ClassName("item___UGGA8Fu");
            ReadOnlyCollection<IWebElement> classifiche = driver.FindElements(classifica);

            int n = 0;

            for (int l = 0; l < classifiche.Count; l++)
            {
                if (classifiche[l].Text == "Italia" || classifiche[l].Text == "Francia" || classifiche[l].Text == "Spagna"
                 || classifiche[l].Text == "Germania" || classifiche[l].Text == "Inghilterra" || classifiche[l].Text == "Portogallo"
                 || classifiche[l].Text == "Brasile")
                {
                    Lega lega = new Lega();
                    lega.Paese = classifiche[l].Text;

                    classifiche[l].Click();

                    WebDriverWait wait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    IWebElement title3 = wait4.Until<IWebElement>((d) =>
                    {
                        return d.FindElement(By.ClassName("templateHref___1W3iWwu"));
                    });
                    Thread.Sleep(10000);

                    By league = By.ClassName("templateHref___1W3iWwu");
                    ReadOnlyCollection<IWebElement> leagues = driver.FindElements(league);

                    leagues[n].Click();

                    By nome = By.ClassName("teamHeader__name");
                    IWebElement name = driver.FindElement(nome);

                    By immagine = By.ClassName("teamHeader__logo");
                    IWebElement imm = driver.FindElement(immagine);


                    lega.Nome = name.Text;

                    By tabstanding = By.ClassName("standings_table");
                    IWebElement tabs = driver.FindElement(tabstanding);

                    tabs.Click();

                    WebDriverWait wait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    IWebElement title2 = wait3.Until<IWebElement>((d) =>
                    {
                        return d.FindElement(By.ClassName("sortingColumn___1ohGY1e"));
                    });
                    Thread.Sleep(10000);

                    By link = By.ClassName("rowCellParticipantImage___21gNVKC");
                    ReadOnlyCollection<IWebElement> links = driver.FindElements(link);

                    By squadra = By.ClassName("rowCellParticipantName___38vskiN");
                    ReadOnlyCollection<IWebElement> squadre = driver.FindElements(squadra);

                    By casella = By.ClassName("rowCell____vgDgoa"); //cell___4WLG6Yd
                    ReadOnlyCollection<IWebElement> caselle = driver.FindElements(casella);


                    Classifica classif = new Classifica();
                    int k = 0;
                    for (int i = 0; i < squadre.Count; i++)
                    {
                        classif.Posizione.Add(Convert.ToInt32(caselle[k].Text.Replace(".", "")));
                        classif.Scudetto.Add(links[i].GetAttribute("href"));
                        classif.Squadra.Add(squadre[i].Text);
                        classif.PartiteGiocate.Add(Convert.ToInt32(caselle[k + 1].Text));
                        classif.Vittorie.Add(Convert.ToInt32(caselle[k + 2].Text));
                        classif.Pareggi.Add(Convert.ToInt32(caselle[k + 3].Text));
                        classif.Sconfitte.Add(Convert.ToInt32(caselle[k + 4].Text));
                        classif.Goals.Add(caselle[k + 5].Text);
                        classif.Punti.Add((Convert.ToInt32(caselle[k + 6].Text)));
                        k = k + 7;

                    }
                    lega.Classifica = classif;
                    lsc.Add(lega);

                    By more2 = By.ClassName("itemMore___iBgyToa");
                    var mores2 = driver.FindElement(more2);

                    WebDriverWait wait5 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    IWebElement title4 = wait5.Until<IWebElement>((d) =>
                    {
                        return d.FindElement(By.ClassName("itemMore___iBgyToa"));
                    });
                    Thread.Sleep(2000);

                    mores2.Click();

                    classifica = By.ClassName("item___UGGA8Fu");
                    classifiche = driver.FindElements(classifica);

                    if (n.Equals(0))
                    {
                        n++;
                        l = l - 1;
                    }
                    else
                    {
                        n = 0;
                    }
                }
            }

            return lsc;
        }
    }
}
