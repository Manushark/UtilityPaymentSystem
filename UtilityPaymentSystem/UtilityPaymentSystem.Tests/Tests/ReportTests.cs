using System;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using UtilityPaymentSystem.Tests.Helpers;
using UtilityPaymentSystem.Tests.Pages;

namespace UtilityPaymentSystem.Tests.Tests
{
    [TestClass]
    public class ReportTests : TestBase
    {
        private ReportPage _reportPage;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            try
            {
                // Inicializar ReportPage después de que Driver esté configurado
                _reportPage = new ReportPage(Driver);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar ReportPage: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        public void CreateReport_AndViewDetails_ShouldWorkCorrectly()
        {
            try
            {
                // Verificar que ExtentTest no sea null
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Iniciando prueba de creación y visualización de reportes");
                }

                // 1. Navegar a la página principal
                Driver.Navigate().GoToUrl(BaseUrl);
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Navegando a la página principal");
                }
                LogScreenshot("HomePage");

                // 2. Navegar a la sección de reportes
                try
                {
                    // Intentar navegar haciendo clic en el enlace "Reportes"
                    IWebElement reportesLink = Driver.FindElement(By.LinkText("Reportes"));
                    reportesLink.Click();
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        // Alternativa: Intentar con "Report"
                        Driver.FindElement(By.LinkText("Report")).Click();
                    }
                    catch (NoSuchElementException)
                    {
                        // Como último recurso, navegar directamente por URL
                        Driver.Navigate().GoToUrl(BaseUrl + "Report");
                    }
                }

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Navegando a la sección de reportes");
                }
                LogScreenshot("ReportsPage");

                // 3. Hacer clic en "Crear Nuevo Reporte"
                try
                {
                    // Intentar con el texto exacto
                    var createButton = Driver.FindElement(By.LinkText("Crear Nuevo Reporte"));
                    createButton.Click();
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        // Intentar con XPath que contiene el texto
                        Driver.FindElement(By.XPath("//a[contains(text(), 'Crear')]")).Click();
                    }
                    catch (NoSuchElementException)
                    {
                        // Intentar con el selector de la clase CSS que vimos en la imagen
                        Driver.FindElement(By.CssSelector(".btn.btn-primary")).Click();
                    }
                }

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Haciendo clic en Crear Nuevo Reporte");
                }
                LogScreenshot("CreateReportForm");

                // 4. Crear datos de prueba únicos
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string description = $"Reporte de prueba automatizado {timestamp}";

                // 5. Rellenar el formulario
                try
                {
                    // Intentar encontrar el campo de descripción
                    var descInput = Driver.FindElement(By.TagName("textarea"));
                    descInput.Clear();
                    descInput.SendKeys(description);
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        // Alternativa: buscar por ID (ajustar según tu HTML real)
                        Driver.FindElement(By.Id("ReportDescription")).SendKeys(description);
                    }
                    catch (NoSuchElementException)
                    {
                        // Último recurso: cualquier input visible que no sea botón
                        var inputs = Driver.FindElements(By.TagName("input"));
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

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, $"Rellenando formulario con descripción: {description}");
                }
                LogScreenshot("FilledReportForm");

                // 6. Enviar el formulario
                try
                {
                    // Buscar el botón GENERAR que vimos en la captura
                    Driver.FindElement(By.XPath("//button[contains(text(), 'GENERAR')]")).Click();
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        // Alternativa: buscar por clase btn-primary
                        Driver.FindElement(By.CssSelector("button.btn.btn-primary")).Click();
                    }
                    catch (NoSuchElementException)
                    {
                        // Último recurso: cualquier botón de tipo submit
                        Driver.FindElement(By.CssSelector("button[type='submit']")).Click();
                    }
                }

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Enviando formulario");
                }
                LogScreenshot("AfterSubmit");

                // 7. Esperar a que la tabla de reportes esté visible
                Wait.Until(ExpectedConditions.ElementExists(By.TagName("table")));
                LogScreenshot("ReportsList");

                // 8. Verificar que el reporte aparezca en la tabla
                bool isReportInTable = Driver.PageSource.Contains(description);

                // 9. Hacer clic en "Detalles" para el reporte recién creado
                try
                {
                    // Encontrar todas las filas de la tabla
                    var rows = Driver.FindElements(By.CssSelector("table.table tbody tr"));

                    // Buscar la fila que contiene nuestra descripción
                    foreach (var row in rows)
                    {
                        if (row.Text.Contains(description))
                        {
                            // Encontrar y hacer clic en el botón "Detalles"
                            row.FindElement(By.LinkText("Detalles")).Click();
                            break;
                        }
                    }
                }
                catch (NoSuchElementException)
                {
                    // Si no podemos encontrar el botón específico, intentemos con el primer botón de detalles
                    Driver.FindElement(By.CssSelector("a.btn.btn-info")).Click();
                }

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Haciendo clic en Detalles del reporte");
                }

                // 10. Esperar a que la página de detalles cargue
                Wait.Until(d => d.Url.Contains("/Details/"));
                LogScreenshot("ReportDetails");

                // 11. Aserciones
                Assert.IsTrue(isReportInTable, $"El reporte '{description}' no aparece en la tabla");
                Assert.IsTrue(Driver.Url.Contains("/Details/"), "No se navegó correctamente a la página de detalles");

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Pass, "Reporte creado correctamente y detalles verificados");
                }
            }
            catch (Exception ex)
            {
                LogScreenshot("Error");
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Fail, $"Error en la prueba: {ex.Message}");
                }
                throw;
            }
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }
    }
}