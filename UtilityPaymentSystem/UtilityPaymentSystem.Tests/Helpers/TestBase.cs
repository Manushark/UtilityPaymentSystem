// 1. Primero, creamos una clase base para las pruebas
// Ruta: UtilityPaymentSystem.Tests/Helpers/TestBase.cs

using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace UtilityPaymentSystem.Tests.Helpers
{
    [TestClass] // Asegúrate de que la clase base tenga esta anotación
    public class TestBase
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;
        protected static ExtentReports ExtentReports;
        protected ExtentTest ExtentTest;
        protected string BaseUrl = "https://localhost:7046/"; // Ajusta según tu configuración

        // Añade esta propiedad para TestContext
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // Configurar el reporte HTML
            string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "TestReport.html");
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

            var htmlReporter = new ExtentHtmlReporter(reportPath);
            htmlReporter.Config.DocumentTitle = "UtilityPaymentSystem - Test Report";
            htmlReporter.Config.ReportName = "Selenium Test Automation Report";
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;

            ExtentReports = new ExtentReports();
            ExtentReports.AttachReporter(htmlReporter);
            ExtentReports.AddSystemInfo("Environment", "QA");
            ExtentReports.AddSystemInfo("Application", "UtilityPaymentSystem");
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            try
            {
                // Asegúrate de que ExtentReports esté inicializado
                if (ExtentReports == null)
                {
                    string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "TestReport.html");
                    Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

                    var htmlReporter = new ExtentHtmlReporter(reportPath);
                    htmlReporter.Config.DocumentTitle = "UtilityPaymentSystem - Test Report";
                    htmlReporter.Config.ReportName = "Selenium Test Automation Report";
                    htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;

                    ExtentReports = new ExtentReports();
                    ExtentReports.AttachReporter(htmlReporter);
                    ExtentReports.AddSystemInfo("Environment", "QA");
                    ExtentReports.AddSystemInfo("Application", "UtilityPaymentSystem");
                }

                var options = new ChromeOptions();
                // options.AddArguments("--headless"); // Descomenta si quieres ejecutar en modo headless
                Driver = new ChromeDriver(options);
                Driver.Manage().Window.Maximize();
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

                // Iniciar el test para el reporte - Verifica que TestContext no sea null
                if (TestContext != null)
                {
                    ExtentTest = ExtentReports.CreateTest(TestContext.TestName);
                }
                else
                {
                    ExtentTest = ExtentReports.CreateTest("Test");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en TestInitialize: {ex.Message}");
                throw;
            }
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            try
            {
                // Verifica que Driver no sea null antes de usarlo
                if (Driver != null)
                {
                    Driver.Quit();
                    Driver.Dispose();
                    Driver = null;
                }

                // Verifica que ExtentReports no sea null
                if (ExtentReports != null)
                {
                    ExtentReports.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en TestCleanup: {ex.Message}");
                // No relanzamos la excepción para evitar ocultar errores de prueba
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Verifica que ExtentReports no sea null
            if (ExtentReports != null)
            {
                ExtentReports.Flush();
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

        // Método para adjuntar capturas al reporte
        protected void LogScreenshot(string stepName)
        {
            try
            {
                // Verifica que ExtentTest no sea null
                if (ExtentTest == null)
                {
                    Console.WriteLine("ExtentTest is null, cannot log screenshot");
                    return;
                }

                string screenshotPath = TakeScreenshot(stepName);
                if (!string.IsNullOrEmpty(screenshotPath))
                {
                    ExtentTest.Log(Status.Info, stepName, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging screenshot: {ex.Message}");
            }
        }
    }
}