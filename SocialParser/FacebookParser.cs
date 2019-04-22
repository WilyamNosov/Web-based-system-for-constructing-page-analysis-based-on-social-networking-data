using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialParser
{
    public class FacebookParser
    {
        public string[] dateFB = DateTime.Now.ToString().Split('.');

        public IWebDriver driver = new ChromeDriver("C:\\Users\\vilya\\Downloads\\chromedriver_win32");
        public string parseUrl = "";

        public bool scrolPageFB()
        {
            IWebElement morePost = driver.FindElement(By.ClassName("uiMorePager"));
            Actions actions = new Actions(driver);
            actions.MoveToElement(morePost);
            actions.Perform();

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> postsDate = driver.FindElements(By.TagName("abbr"));

            foreach (IWebElement postDate in postsDate)
            {
                string[] dateTest = postDate.GetAttribute("title").ToString().Split('.');
                try
                {
                    if (Convert.ToInt32(dateFB[1]) > Convert.ToInt32(dateTest[1]) && Convert.ToInt32(dateFB[0]) >= Convert.ToInt32(dateTest[0]))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return false;
        }

        public void urlParse(string url)
        {
            parseUrl = url.Split('/')[3];
        }

        public List<string> getPostsTextFacebook()
        {
            while (true)
            {
                if (scrolPageFB() == true)
                {
                    break;
                }
                int milliseconds = 1000;
                Thread.Sleep(milliseconds);
            }

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> posts = driver.FindElements(By.ClassName("userContent"));

            return convertToList(posts);
        }

        public List<string> convertToList(System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> posts)
        {
            List<string> poststList = new List<string>();

            foreach (IWebElement post in posts)
            {
                poststList.Add(post.Text);
            }

            return poststList;
        }

        public void setDriver()
        {
            driver.Url = (@"https://www.facebook.com/pg/" + parseUrl + "/");
        }

        public void quit()
        {
            driver.Quit();
        }
    }
}
