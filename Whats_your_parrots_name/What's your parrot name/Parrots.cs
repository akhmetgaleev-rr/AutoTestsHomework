using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTestsAtPrictice
{
    public class WhatIsYourParrotName
    {
        public ChromeDriver driver;
        public WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized"); // браузер раскрывается на весь экран
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); //явные ожидания
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //неявные ожидания
        }

        private By emailInputLocator = By.Name("email");
        private By buttonLocator = By.Id("sendMe");
        private By emailResultLocator = By.ClassName("your-email");
        private By anotherEmailLinkLocator = By.LinkText("указать другой e-mail");
        private By anotherEmailLinkIdLocator = By.Id("anotherEmail");
        private By emailErrorLocator = By.ClassName("form-error");
        private By boyRadioButtonLocator = By.Id("boy");
        private By resultTextLocator = By.ClassName("result-text");
        private By girlRadioButtonLocator = By.Id("girl");

        [Test]
        public void ParrotNameSite_FillFormWIthEmail_Success()

        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            var expectedEmail = "test@mail.ru";

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(expectedEmail, driver.FindElement(emailResultLocator).Text, "Сделали заявку не на тот e-mail");

            Thread.Sleep(2000);
        }

        [Test]
        public void ParrotNameSite_ClickAnotherEmail_EmailInputIsEmpty()

        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");

            driver.FindElement(emailInputLocator).SendKeys("test@mail.ru");
            driver.FindElement(buttonLocator).Click();

            driver.FindElement(anotherEmailLinkLocator).Click();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "После клика по ссылке поле не очистилось");
            Assert.IsFalse(driver.FindElement(anotherEmailLinkIdLocator).Displayed, "Не исчезла ссылка для ввода другого e-mail");

            Thread.Sleep(2000);
        }

        [Test]
        public void ParrotNameSite_FillFormWIthEmptyEmail_Fail()

        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            var expectedEmail = "";

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Введите email", driver.FindElement(emailErrorLocator).Text, "Не тот текст ошибки при пустом email");

            Thread.Sleep(2000);
        }

        [Test]
        public void ParrotNameSite_FillFormWIthInvalidEmail_Fail()

        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            var expectedEmail = "12345";

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Некорректный email", driver.FindElement(emailErrorLocator).Text, "Не тот текст ошибки при некорректном email");

            Thread.Sleep(2000);
        }

        [Test]
        public void ParrotNameSite_FillFormWIthEmailWithoutDomen_Fail() //не срабатывает валидация при отсутствии домена в email

        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            var expectedEmail = "test@mail.";

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Некорректный email", driver.FindElement(emailErrorLocator).Text, "Не тот текст ошибки при некорректном email");

            Thread.Sleep(2000);
        }

        [Test]
        public void ParrotNameSite_ChooseBoyName_Success()

        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            var expectedEmail = "test@mail.ru";

            driver.FindElement(boyRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Хорошо, мы пришлём имя для вашего мальчика на e-mail:", driver.FindElement(resultTextLocator).Text, "Выведено сообщение не для того пола");

            Thread.Sleep(2000);
        }

        [Test]
        public void ParrotNameSite_ChooseGirlName_Success()

        {
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-practice/");
            var expectedEmail = "test@mail.ru";

            driver.FindElement(girlRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Хорошо, мы пришлём имя для вашей девочки на e-mail:", driver.FindElement(resultTextLocator).Text, "Выведено сообщение не для того пола");

            Thread.Sleep(2000);
        }

        [TearDown]
        public void TeadDown()
        {
            driver.Quit();
        }

    }
}
