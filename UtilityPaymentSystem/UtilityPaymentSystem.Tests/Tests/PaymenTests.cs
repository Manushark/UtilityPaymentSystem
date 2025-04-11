// 2. Ahora, añadimos la prueba para la página de pagos
// Ruta: UtilityPaymentSystem.Tests/Tests/PaymentTests.cs
using System;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using UtilityPaymentSystem.Tests.Helpers;
using UtilityPaymentSystem.Tests.Pages;

namespace UtilityPaymentSystem.Tests.Tests
{
    [TestClass]
    public class PaymentTests : TestBase
    {
        private PaymentPage _paymentPage;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            try
            {
                // Inicializar PaymentPage después de que Driver esté configurado
                _paymentPage = new PaymentPage(Driver);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar PaymentPage: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        public void ViewPaymentPage_ShouldDisplayPaymentHistory()
        {
            try
            {
                // Verifica que ExtentTest no sea null
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Iniciando prueba de visualización de página de pagos");
                }

                // 1. Navegar a la página principal
                Driver.Navigate().GoToUrl(BaseUrl);
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Navegando a la página principal");
                }
                LogScreenshot("HomePage");

                // 2. Navegar a la sección de pagos
                _paymentPage.NavigateToPayments();
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Navegando a la sección de pagos");
                }
                LogScreenshot("PaymentPage");

                // 3. Verificar que la página de pagos está cargada correctamente
                bool isPageLoaded = _paymentPage.IsPaymentPageLoaded();
                bool isTableVisible = _paymentPage.IsPaymentTableVisible();

                // 4. Aserciones
                Assert.IsTrue(isPageLoaded, "La página de pagos no se cargó correctamente");
                Assert.IsTrue(isTableVisible, "La tabla de pagos no está visible");

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Pass, "Página de pagos visualizada correctamente");
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