using OpenQA.Selenium;

namespace NHSD.GPITF.BuyingCatalog.SystemTests
{
  internal static class WebDriverExtensions
  {
      public static void SendKeys(this IWebDriver driver, By by, string text)
      {
        var elm = driver.FindElement(by);
        elm.SendKeys(text);
      }

      public static void Click(this IWebDriver driver, By by)
      {
        var elm = driver.FindElement(by);
        elm.Click();
      }
  }
}
