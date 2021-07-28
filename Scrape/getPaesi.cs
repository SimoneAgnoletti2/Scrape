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

namespace Scrape
{
    //ciao
    public class getPaesi
    {
        IWebDriver driver;
        WebDriverWait wait;
        public List<Paese> paesi()
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
                Thread.Sleep(5000);

                By policy = By.Id("onetrust-accept-btn-handler");
                var pol = driver.FindElement(policy);
                pol.Click();
            }
            catch (Exception ex)
            {

            }

            List<Paese> lsc = new List<Paese>();

            lsc = clickPaese();

            close_Browser();

            return lsc;
        }

        public void close_Browser()
        {
            driver.Quit();
        }



        public List<Paese> clickPaese()
        {
            List<Paese> lsc = new List<Paese>();

            By more = By.ClassName("itemMore___iBgyToa");
            var mores = driver.FindElement(more);

            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement title = wait2.Until<IWebElement>((d) =>
            {
                return d.FindElement(By.ClassName("itemMore___iBgyToa"));
            });
            Thread.Sleep(2000);

            try
            {
                mores.Click();
            }
            catch(Exception ex)
            {
                wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                title = wait2.Until<IWebElement>((d) =>
                {
                    return d.FindElement(By.ClassName("itemMore___iBgyToa"));
                });
                Thread.Sleep(10000);
                mores.Click();
            }


            List<string> ls = new List<string>();


            By paese = By.ClassName("item___UGGA8Fu");
            ReadOnlyCollection<IWebElement> paesi = driver.FindElements(paese);

            for (int l = 0; l < paesi.Count; l++)
            {
                Paese pa = new Paese();
                pa.Nome = paesi[l].Text;
                lsc.Add(pa);
            }
            lsc = getBandiera(lsc);

            return lsc;
        }
        public List<Paese> getBandiera(List<Paese> paesi)
        {
            foreach(Paese p in paesi)
            {
                try
                {
                    driver.Navigate().GoToUrl("https://www.bandiere-mondo.it/" + p.Nome.Replace(" ", "-")); 
                    var timeout = 10000; /* Maximum wait time of 20 seconds */
                    var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
                    wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                    Thread.Sleep(1000);

                    By flag = By.ClassName("flag-detail");
                    IWebElement f = driver.FindElement(flag);

                    By bandiera = By.TagName("img");
                    IWebElement ban = f.FindElement(bandiera);

                    p.Link = ban.GetAttribute("src");
                }
                catch (Exception ex) 
                {
                    p.Link = "";
                }
            }
            return paesi;
        }
    }
}
