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
    class getDettaglio
    {
        IWebDriver driver;

        public List<Campionato> dettaglio()
        {
            List<Partita> partite = new List<Partita>();
            List<Campionato> camp = new List<Campionato>();

            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.flashscore.com/";

            var timeout = 10000; /* Maximum wait time of 20 seconds */
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(6000);

            By squadracasa = By.ClassName("event__participant--home");
            ReadOnlyCollection<IWebElement> squadrecasa = driver.FindElements(squadracasa);
            


            close_Browser();
            return camp;
        }



        public void close_Browser()
        {
            driver.Quit();
        }
    }
}
