using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHSD.GPITF.BuyingCatalog.SystemTests
{
  [TestFixture]
  public sealed class BuyingCatalog_Tests
  {
    /*
     * structure of Users section in secrets.json

    "Users":
    [
      {
        "Id": "1-1",
        "Email": "user1@organisation1.com",
        "Password": "user1Password"
      },
      {
        "Id": "1-2",
        "Email": "user2@organisation1.com",
        "Password": "user2Password"
      },
      {
        "Id": "1-3",
        "Email": "user3@organisation1.com",
        "Password": "user3Password"
      },
      {
        "Id": "1-4",
        "Email": "user4@organisation1.com",
        "Password": "user4Password"
      },
      {
        "Id": "2-1",
        "Email": "user1@organisation2.com",
        "Password": "user1Password"
      },
      {
        "Id": "2-2",
        "Email": "user2@organisation2.com",
        "Password": "user2Password"
      }
    ]
  }
    */

    private string BASE_URL;
    private string _chromeDriverDirectory;
    private List<User> _users;

    [SetUp]
    public void Setup()
    {
      var builder = new ConfigurationBuilder()
                .AddUserSecrets<BuyingCatalog_Tests>();
      var config = builder.Build();

      BASE_URL = Settings.BASE_URL(config);
      _chromeDriverDirectory = config["ChromeDriverDirectory"];
      _users = config.GetSection("Users").Get<List<User>>();
    }

    [Test]
    public void MultiUser_SameOrganisation_Login_Succeeds()
    {
      RunLogin(2, 1);
    }

    [Test]
    public void MultiUser_MultiOrganisation_Login_Succeeds()
    {
      RunLogin(2, 2);
    }

    private void RunLogin(int orgUserCount, int orgCount)
    {
      var allTasks = new List<Task>();
      for (var user = 1; user <= orgUserCount; user++)
      {
        for (var org = 1; org <= orgCount; org++)
        {
          allTasks.Add(Task.Factory.StartNew(userInfoObj =>
          {
            using (var driver = new ChromeDriver(_chromeDriverDirectory))
            {
              var userInfo = (UserInfo)userInfoObj;
              Login(driver, userInfo);
              AddSolution(driver, userInfo);
            }
          }, new UserInfo(user, org)));
        }
      }
      Task.WaitAll(allTasks.ToArray());
    }

    private void Login(IWebDriver driver, UserInfo userInfo)
    {
      var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

      // BC
      driver.Navigate().GoToUrl(BASE_URL);

      // BC
      wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.LinkText("Log in")));
      driver.Click(By.LinkText("Log in"));

      // Auth0
      wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Sign In"));
      wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("submit")));

      var user = _users.Single(u => u.Id == userInfo.GetId());
      driver.SendKeys(By.Name("email"), user.Email);
      driver.SendKeys(By.Name("password"), user.Password);
      driver.Click(By.Name("submit"));

      // BC
      wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("NHS Digital Buying Catalogue"));
    }

    private void AddSolution(IWebDriver driver, UserInfo userInfo)
    {
      try
      {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        // BC
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("add-new-solution")));
      }
      catch (WebDriverTimeoutException)
      {
        var user = _users.Single(u => u.Id == userInfo.GetId());
        TestContext.WriteLine($"Failed for user: {user.Email}");

        throw;
      }
    }

    private sealed class UserInfo
    {
      private readonly int _userNumber;
      private readonly int _orgNumber;

      public UserInfo(int userNumber, int orgNumber)
      {
        _userNumber = userNumber;
        _orgNumber = orgNumber;
      }

      public string GetId()
      {
        var retVal = $"{_orgNumber}-{_userNumber}";
        return retVal;
      }
    }

    private sealed class User
    {
      public string Id { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }
    }
  }
}