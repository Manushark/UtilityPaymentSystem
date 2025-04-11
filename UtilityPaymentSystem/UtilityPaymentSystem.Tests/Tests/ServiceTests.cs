using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using UtilityPaymentSystem.Tests.Pages;

namespace UtilityPaymentSystem.Tests.Tests
{
    [TestClass]
    public class ServiceTests
    {
        protected IWebDriver Driver;
        protected ExtentTest ExtentTest;
        protected string BaseUrl = "https://localhost:7046/"; // Ajusta según tu configuración
        private ServicePage _servicePage;
        private static ExtentReports _extentReports;
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassSetup(TestContext testContext)
        {
            // Configurar reporte
            string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "ServiceTestReport.html");
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

            var htmlReporter = new ExtentHtmlReporter(reportPath);
            htmlReporter.Config.DocumentTitle = "UtilityPaymentSystem - Service Test Report";
            htmlReporter.Config.ReportName = "Service Test Automation Report";

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(htmlReporter);
            _extentReports.AddSystemInfo("Environment", "QA");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            try
            {
                // Inicializar el driver
                var options = new ChromeOptions();
                Driver = new ChromeDriver(options);
                Driver.Manage().Window.Maximize();
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                // Inicializar la página
                _servicePage = new ServicePage(Driver);

                // Inicializar el test report
                ExtentTest = _extentReports?.CreateTest(TestContext?.TestName ?? "Service Test");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en inicialización: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        public void AddService_ShouldCreateServiceSuccessfully()
        {
            try
            {
                // Registrar inicio del test
                ExtentTest?.Log(Status.Info, "Iniciando prueba de creación de servicio");

                // 1. Navegar a la página principal
                Driver.Navigate().GoToUrl(BaseUrl);
                ExtentTest?.Log(Status.Info, "Navegando a la página principal");
                TakeScreenshot("HomePage");

                // 2. Navegar a la sección de servicios
                try
                {
                    // Buscar el enlace de Servicios
                    var serviciosLinks = Driver.FindElements(By.LinkText("Servicios"));
                    if (serviciosLinks.Count > 0)
                    {
                        serviciosLinks[0].Click();
                    }
                    else
                    {
                        // Intentar con texto parcial
                        var altLinks = Driver.FindElements(By.PartialLinkText("ervicio"));
                        if (altLinks.Count > 0)
                        {
                            altLinks[0].Click();
                        }
                        else
                        {
                            // Ir directamente a la URL
                            Driver.Navigate().GoToUrl(BaseUrl + "/Service");
                        }
                    }
                }
                catch (Exception)
                {
                    Driver.Navigate().GoToUrl(BaseUrl + "/Service");
                }

                ExtentTest?.Log(Status.Info, "Navegando a la sección de servicios");
                TakeScreenshot("ServicesPage");

                // 3. Hacer clic en "Crear Nuevo Servicio"
                _servicePage.ClickCreateNew();
                ExtentTest?.Log(Status.Info, "Haciendo clic en Crear Nuevo Servicio");
                TakeScreenshot("CreateServiceForm");

                // 4. Crear un nombre de servicio único con timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string serviceName = $"Servicio Test {timestamp}";

                // 5. Rellenar el formulario
                _servicePage.FillServiceForm(serviceName);
                ExtentTest?.Log(Status.Info, $"Rellenando formulario con nombre: {serviceName}");
                TakeScreenshot("FilledServiceForm");

                // 6. Enviar el formulario
                _servicePage.SubmitForm();
                ExtentTest?.Log(Status.Info, "Enviando formulario");
                TakeScreenshot("AfterSubmit");

                // 7. Verificar que el servicio se ha creado correctamente
                bool isServiceCreated = _servicePage.IsServiceCreatedSuccessfully();
                TakeScreenshot("ServiceCreationResult");

                // 8. Verificar que el servicio aparece en la tabla
                bool isServiceInTable = _servicePage.IsServiceInTable(serviceName);

                // 9. Aserciones
                Assert.IsTrue(isServiceCreated, "El mensaje de éxito no se muestra o no se redirigió a la lista de servicios");
                Assert.IsTrue(isServiceInTable, $"El servicio '{serviceName}' no aparece en la tabla");

                ExtentTest?.Log(Status.Pass, "Servicio creado correctamente y verificado en la tabla");
            }
            catch (Exception ex)
            {
                TakeScreenshot("Error");
                ExtentTest?.Log(Status.Fail, $"Error en la prueba: {ex.Message}");
                throw;
            }
        }

        // Método para tomar capturas de pantalla
        protected string TakeScreenshot(string screenshotName)
        {
            try
            {
                string screenshotPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
                Directory.CreateDirectory(screenshotPath);

                string fileName = $"{screenshotName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string fullPath = Path.Combine(screenshotPath, fileName);

                // Método alternativo para tomar screenshot
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();

                // Guardar directamente como Base64 y luego convertir a archivo
                var screenshotBytes = Convert.FromBase64String(screenshot.AsBase64EncodedString);
                File.WriteAllBytes(fullPath, screenshotBytes);

                Console.WriteLine($"Screenshot saved at: {fullPath}");
                return fullPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error taking screenshot: {ex.Message}");
                return null;
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                if (Driver != null)
                {
                    Driver.Quit();
                    Driver.Dispose();
                    Driver = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar el driver: {ex.Message}");
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _extentReports?.Flush();
        }
    }
}