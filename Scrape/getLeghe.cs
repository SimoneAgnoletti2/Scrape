using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Scrape.Classi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scrape
{
    public class getLeghe
    {
        IWebDriver driver;
        IWebDriver driver2;
        WebDriverWait wait;
        public List<Lega> leghe()
        {
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

            List<Lega> lsc = new List<Lega>();

            lsc = clickLeghe();

            close_Browser();

            return lsc;
        }

        public void close_Browser()
        {
            driver.Quit();
        }



        public List<Lega> clickLeghe()
        {
            List<Lega> lsc = new List<Lega>();

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
            catch (Exception ex)
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
                paesi[l].Click();
                Thread.Sleep(2000);
                By leghe = By.ClassName("templateHref___1W3iWwu");
                ReadOnlyCollection<IWebElement> lega = driver.FindElements(leghe);

                for (int k = 0; k < lega.Count; k++)
                {
                    Lega lega_classe = new Lega();
                    lega_classe.idPaese = l+1;
                    lega_classe.Nome = lega[k].Text;
                    lsc.Add(lega_classe);
                }
                paesi[l].Click();
            }
            return lsc;
        }
    }
}
