// 1. Primero, creamos un Page Object para la página de pagos
// Ruta: UtilityPaymentSystem.Tests/Pages/PaymentPage.cs
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UtilityPaymentSystem.Tests.Pages
{
    public class PaymentPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Localizadores
        private readonly By _paymentTitle = By.TagName("h1");
        private readonly By _paymentsTable = By.CssSelector("table.table");

        public PaymentPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateToPayments()
        {
            // Intenta usar el enlace de navegación "Pago"
            try
            {
                _driver.FindElement(By.LinkText("Pago")).Click();
            }
            catch (NoSuchElementException)
            {
                // Intenta una alternativa
                try
                {
                    _driver.FindElement(By.PartialLinkText("ago")).Click();
                }
                catch (NoSuchElementException)
                {
                    // Si todo falla, navega directamente por URL
                    _driver.Navigate().GoToUrl(_driver.Url + "Payment");
                }
            }

            // Espera a que la página cargue
            _wait.Until(ExpectedConditions.ElementExists(_paymentTitle));
        }

        public bool IsPaymentPageLoaded()
        {
            try
            {
                var title = _driver.FindElement(_paymentTitle);
                return title.Text.Contains("Historial de Pagos") ||
                       title.Text.Contains("Payment") ||
                       title.Text.Contains("Pagos");
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsPaymentTableVisible()
        {
            try
            {
                return _driver.FindElement(_paymentsTable).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}