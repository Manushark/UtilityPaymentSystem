using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UtilityPaymentSystem.Tests.Pages
{
    public class ReportPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Localizadores basados en el HTML proporcionado
        private readonly By _createNewLink = By.CssSelector("a.btn.btn-primary[href='/Report/Create']");
        private readonly By _createNewButton = By.CssSelector(".btn-primary"); // Alternativa
        private readonly By _descriptionInput = By.Id("ReportDescription");  // Este ID es una suposición, ajústalo según tu HTML real
        private readonly By _generateButton = By.CssSelector("button.btn.btn-primary");
        private readonly By _reportsTable = By.CssSelector("table.table");
        private readonly By _detailsButton = By.LinkText("Detalles");
        private readonly By _detailsButtonAlt = By.CssSelector("a.btn.btn-info");

        public ReportPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateToReports()
        {
            try
            {
                // Intentar navegar haciendo clic en el enlace "Reportes"
                var reportesLink = _driver.FindElement(By.LinkText("Reportes"));
                reportesLink.Click();
            }
            catch (NoSuchElementException)
            {
                try
                {
                    // Alternativa: Intenta con "Report"
                    _driver.FindElement(By.LinkText("Report")).Click();
                }
                catch (NoSuchElementException)
                {
                    // Como último recurso, navegar directamente por URL
                    _driver.Navigate().GoToUrl(_driver.Url.Split('/')[0] + "/Report");
                }
            }

            // Esperar a que la página cargue
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("h1")));
        }

        public void ClickCreateNew()
        {
            try
            {
                // Intentar encontrar el botón "Crear Nuevo Reporte"
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(), 'Crear Nuevo Reporte')]"))).Click();
            }
            catch (WebDriverTimeoutException)
            {
                try
                {
                    // Intentar con el texto "CREAR NUEVO REPORTE"
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[contains(text(), 'CREAR NUEVO REPORTE')]"))).Click();
                }
                catch (WebDriverTimeoutException)
                {
                    try
                    {
                        // Intentar con el selector CSS específico que vimos en la imagen
                        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".btn.btn-primary"))).Click();
                    }
                    catch (WebDriverTimeoutException)
                    {
                        // Como último recurso, navegar directamente a la URL de creación
                        _driver.Navigate().GoToUrl(_driver.Url.Split('?')[0] + "/Create");
                    }
                }
            }

            // Esperar a que el formulario de creación se cargue
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("form")));
        }

        public void FillReportForm(string description)
        {
            // Intentar diferentes selectores para el campo de descripción
            try
            {
                // Por ID específico si existe
                var descInput = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ReportDescription")));
                descInput.Clear();
                descInput.SendKeys(description);
            }
            catch (WebDriverTimeoutException)
            {
                try
                {
                    // Por etiqueta textarea
                    var textArea = _wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("textarea")));
                    textArea.Clear();
                    textArea.SendKeys(description);
                }
                catch (WebDriverTimeoutException)
                {
                    // Por cualquier input visible
                    var inputs = _driver.FindElements(By.TagName("input"));
                    foreach (var input in inputs)
                    {
                        if (input.Displayed && input.GetAttribute("type") != "hidden" && input.GetAttribute("type") != "submit")
                        {
                            input.Clear();
                            input.SendKeys(description);
                            break;
                        }
                    }
                }
            }
        }

        public void SubmitForm()
        {
            try
            {
                // Buscar el botón GENERAR que vimos en la captura
                _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'GENERAR')]"))).Click();
            }
            catch (WebDriverTimeoutException)
            {
                try
                {
                    // Alternativa: buscar por clase btn-primary
                    _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.btn.btn-primary"))).Click();
                }
                catch (WebDriverTimeoutException)
                {
                    // Último recurso: cualquier botón de tipo submit
                    _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
                }
            }

            // Esperar redirección a la lista de reportes
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("table")));
        }

        public void ClickViewDetails(string reportDescription)
        {
            // Intentar encontrar la fila que contiene la descripción del reporte
            var rows = _driver.FindElements(By.CssSelector("table.table tbody tr"));

            foreach (var row in rows)
            {
                if (row.Text.Contains(reportDescription))
                {
                    // Encontrar el botón Detalles en esta fila
                    try
                    {
                        row.FindElement(By.LinkText("Detalles")).Click();
                        break;
                    }
                    catch (NoSuchElementException)
                    {
                        try
                        {
                            // Alternativa: intentar con DETALLES
                            row.FindElement(By.LinkText("DETALLES")).Click();
                            break;
                        }
                        catch (NoSuchElementException)
                        {
                            // Alternativa: buscar cualquier botón info en la fila
                            row.FindElement(By.CssSelector(".btn.btn-info")).Click();
                            break;
                        }
                    }
                }
            }

            // Esperar a que la página de detalles se cargue
            _wait.Until(d => d.Url.Contains("/Details/"));
        }

        public bool IsReportInTable(string description)
        {
            try
            {
                // Verificar si la descripción aparece en la tabla
                var table = _driver.FindElement(_reportsTable);
                return table.Text.Contains(description);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsOnDetailsPage()
        {
            return _driver.Url.Contains("/Details/");
        }
    }
}