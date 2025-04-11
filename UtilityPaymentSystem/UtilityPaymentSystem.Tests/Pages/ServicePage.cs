using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UtilityPaymentSystem.Tests.Pages
{
    public class ServicePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Localizadores
        private readonly By _createNewLink = By.CssSelector("a[href='/Service/Create']");
        private readonly By _nameInput = By.Id("Name");
        private readonly By _createButton = By.CssSelector("input[type='submit'], button[type='submit']");
        private readonly By _serviceTable = By.CssSelector("table.table");

        public ServicePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateToServices()
        {
            _driver.Navigate().GoToUrl(_driver.Url + "/Service");
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("h1")));
        }

        public void ClickCreateNew()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(_createNewLink)).Click();
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("h1")));
        }

        public void FillServiceForm(string name)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(_nameInput)).Clear();
            _driver.FindElement(_nameInput).SendKeys(name);
        }

        public void SubmitForm()
        {
            _driver.FindElement(_createButton).Click();
            // Esperar a que se complete la redirección
            _wait.Until(d => d.Url.Contains("/Service"));
        }

        public bool IsServiceCreatedSuccessfully()
        {
            try
            {
                // Buscar algún indicador de éxito (mensaje o tabla)
                return _driver.PageSource.Contains("created successfully") ||
                       _driver.FindElements(_serviceTable).Count > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsServiceInTable(string name)
        {
            try
            {
                // Verificar si el nombre del servicio aparece en la tabla
                return _driver.FindElement(_serviceTable).Text.Contains(name);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool DeleteService(string name)
        {
            try
            {
                // Buscar el servicio en la tabla
                var rows = _driver.FindElements(By.CssSelector("table.table tbody tr"));
                foreach (var row in rows)
                {
                    if (row.Text.Contains(name))
                    {
                        // Encontrar el botón de eliminar en esta fila
                        var deleteButton = row.FindElement(By.CssSelector("a.btn-danger"));
                        deleteButton.Click();

                        // Manejar el cuadro de diálogo de confirmación
                        // Esperar un momento para que aparezca el alert si lo hay
                        try
                        {
                            _wait.Until(ExpectedConditions.AlertIsPresent());
                            _driver.SwitchTo().Alert().Accept();
                        }
                        catch (WebDriverTimeoutException)
                        {
                            // Si no hay alert, continuar
                        }

                        // Esperar a que se complete la acción
                        _wait.Until(d => d.Url.Contains("/Service"));
                        return true;
                    }
                }
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}