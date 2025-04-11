using System;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using UtilityPaymentSystem.Tests.Helpers;
using UtilityPaymentSystem.Tests.Pages;

namespace UtilityPaymentSystem.Tests.Tests
{
    [TestClass]
    public class BillTests : TestBase
    {
        private BillPage _billPage;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            try
            {
                // Inicializar BillPage después de que Driver esté configurado
                _billPage = new BillPage(Driver);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar BillPage: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        public void AddBill_ShouldCreateBillSuccessfully()
        {
            try
            {
                // Verifica que ExtentTest no sea null
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Iniciando prueba de creación de factura");
                }

                // 1. Navegar a la página principal
                Driver.Navigate().GoToUrl(BaseUrl);
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Navegando a la página principal");
                }
                LogScreenshot("HomePage");

                // 2. Navegar a la sección de facturas
                try
                {
                    // Intenta encontrar el enlace de Facturas en el menú
                    IWebElement facturasLink = Driver.FindElement(By.LinkText("Facturas"));
                    facturasLink.Click();
                    if (ExtentTest != null)
                    {
                        ExtentTest.Log(Status.Info, "Navegando a la sección de facturas");
                    }
                }
                catch (NoSuchElementException)
                {
                    // Intenta alternativas si no encuentra el enlace exacto
                    try
                    {
                        Driver.FindElement(By.PartialLinkText("actura")).Click();
                    }
                    catch
                    {
                        // Navega directamente a la URL como última opción
                        Driver.Navigate().GoToUrl(BaseUrl + "/Bill");
                    }

                    if (ExtentTest != null)
                    {
                        ExtentTest.Log(Status.Info, "Navegando a la sección de facturas (método alternativo)");
                    }
                }
                LogScreenshot("BillsPage");

                // 3. Hacer clic en "Crear Nueva Factura"
                _billPage.ClickCreateNew();
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Haciendo clic en Crear Nueva Factura");
                }
                LogScreenshot("CreateBillForm");

                // 4. Crear datos de prueba únicos
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string userId = "1"; // Asumiendo que existe un usuario con ID 1
                string serviceId = "1"; // Asumiendo que existe un servicio con ID 1
                string amount = $"{new Random().Next(1000, 5000)}.00";
                bool paid = false; // Crear como pendiente de pago
                string dueDate = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");

                // 5. Rellenar el formulario
                _billPage.FillBillForm(userId, serviceId, amount, paid, dueDate);
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, $"Rellenando formulario con usuario: {userId}, servicio: {serviceId}, monto: {amount}, fecha: {dueDate}");
                }
                LogScreenshot("FilledBillForm");

                // 6. Enviar el formulario
                _billPage.SubmitForm();
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Enviando formulario");
                }
                LogScreenshot("AfterSubmit");

                // 7. Verificar el resultado
                bool isBillCreated = _billPage.IsBillCreatedSuccessfully();
                LogScreenshot("BillCreationResult");

                // 8. Verificar si la factura aparece en la tabla
                bool isBillInTable = _billPage.IsBillInTable(amount);

                // 9. Aserciones
                Assert.IsTrue(isBillCreated, "No se pudo confirmar la creación exitosa de la factura");
                Assert.IsTrue(isBillInTable, $"La factura con monto '{amount}' no aparece en la tabla");

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Pass, "Factura creada correctamente y verificada");
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