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
    public class getPaesiFS
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

            return lsc;
        }
    }
}
