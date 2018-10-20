using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutoAmazon
{
    class Program
    {
        private static string ProxID_txt = "../ProxID.txt";
        private static string Keyword_txt = "../Keyword.txt";
        private static string Product_txt = "../ProductCod.txt";
        private static string WebServer_url = "https://www.amazon.com";
        static string[] proxIDs;
        static string[] keywords;
        static string[] productCods;

        static void Main(string[] args)
        {
            if (!getIP())
            {
                System.Console.WriteLine("../ProxID.txt is empty!");
                return;
            }
            if (!getProductCod())
            {
                System.Console.WriteLine("../Product.txt is empty");
            }
            if (!getKeyword())
            {
                System.Console.WriteLine("../Keyword.txt is empty");
            }

            for (int i = 0; i < productCods.Length; i++)
            {
                Random rnd = new Random();
                int indexIP = rnd.Next(0, proxIDs.Length);
                int indexKeyword = rnd.Next(0, 47);

                try
                {
                    ChromeOptions option = new ChromeOptions();
                    string sHostIdandPort = proxIDs[indexIP] + ":80";
                    Proxy proxy = new Proxy();
                    proxy.HttpProxy = sHostIdandPort;
                    option.Proxy = proxy;
                    IWebDriver driver = new ChromeDriver(option);
                    driver.Url = WebServer_url;
                    driver.Manage().Window.Maximize();
                    IWebElement txtEmail = driver.FindElement(By.Id("twotabsearchtextbox"));
                    IWebElement selBox = driver.FindElement(By.Id("searchDropdownBox"));
                    selBox.Click();
                    selBox.FindElement(By.XPath(getPaath(indexKeyword))).Click();
                    IWebElement searchInputBox = driver.FindElement(By.Id("twotabsearchtextbox"));
                    searchInputBox.SendKeys(getProductKey(4));
                    IWebElement btnSubmit = driver.FindElement(By.ClassName("nav-input"));
                    btnSubmit.Click();
                    driver.Close();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                    return;
                }
            }
        }

        static bool getIP()
        {
            proxIDs = System.IO.File.ReadAllLines(ProxID_txt);
            if (proxIDs.Length > 0)
                return true;
            return false;
        }

        static bool getKeyword()
        {
            keywords = System.IO.File.ReadAllLines(Keyword_txt);
            if (keywords.Length > 0)
                return true;
            return false;
        }

        static bool getProductCod()
        {
            productCods = System.IO.File.ReadAllLines(Product_txt);
            if (productCods.Length > 0)
                return true;
            return false;
        }

        static string getPaath(int iIndex)
        {
            string sResult = ".//option[contains(text(),'";
            sResult = sResult + keywords[iIndex] + "')]";
            return sResult;
        }

        static string getProductKey(int iIndex)
        {
            return productCods[iIndex];
        }
    }
}
