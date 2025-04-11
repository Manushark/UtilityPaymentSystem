// 2. Creamos el Page Object para la página de usuarios
// Ruta: UtilityPaymentSystem.Tests/Pages/UserPage.cs
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UtilityPaymentSystem.Tests.Pages
{
    public class UserPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Localizadores actualizados según el HTML real
        private readonly By _createNewLink = By.LinkText("Crear"); // O puede ser un botón
        private readonly By _nameInput = By.Id("Name");
        private readonly By _emailInput = By.Id("Email");
        private readonly By _passwordInput = By.Id("PasswordHash");
        private readonly By _createButton = By.CssSelector("button[type='submit']");
        private readonly By _usersTable = By.CssSelector("table.table");

        public UserPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateToUsers()
        {
            _driver.Navigate().GoToUrl(_driver.Url + "/User/Create");
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("h1")));
        }

        public void ClickCreateNew()
        {
            // Busca el enlace "Create New" o similar
            var createLinks = _driver.FindElements(By.LinkText("Create New"));
            if (createLinks.Count > 0)
            {
                createLinks[0].Click();
            }
            else
            {
                // Alternativa: buscar por "Crear Usuario" o cualquier otro texto similar
                var altLinks = _driver.FindElements(By.LinkText("Crear Usuario"));
                if (altLinks.Count > 0)
                {
                    altLinks[0].Click();
                }
                else
                {
                    // Otra alternativa: buscar por clase de botón
                    var buttons = _driver.FindElements(By.CssSelector(".btn-primary, .btn-success"));
                    foreach (var button in buttons)
                    {
                        if (button.Text.Contains("New") || button.Text.Contains("Crear") || button.Text.Contains("Add"))
                        {
                            button.Click();
                            break;
                        }
                    }
                }
            }

            // Esperar a que aparezca el formulario
            _wait.Until(ExpectedConditions.ElementExists(By.Id("Name")));
        }

        public void FillUserForm(string name, string email, string password)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(_nameInput)).SendKeys(name);
            _driver.FindElement(_emailInput).SendKeys(email);
            _driver.FindElement(_passwordInput).SendKeys(password);
        }

        public void SubmitForm()
        {
            _driver.FindElement(_createButton).Click();
            // Esperar a que se complete la redirección
            _wait.Until(d => d.Url.Contains("/User"));
        }

        public bool IsUserCreatedSuccessfully()
        {
            try
            {
                // Buscar algún indicador de éxito (mensaje o tabla)
                return _driver.PageSource.Contains("created successfully") ||
                       _driver.FindElements(_usersTable).Count > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsUserInTable(string name)
        {
            try
            {
                // Verificar si el nombre de usuario aparece en la tabla
                var tables = _driver.FindElements(_usersTable);
                if (tables.Count > 0)
                {
                    return tables[0].Text.Contains(name);
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