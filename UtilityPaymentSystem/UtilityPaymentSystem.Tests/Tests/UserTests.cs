// 3. Creamos la clase de prueba para agregar un usuario
// Ruta: UtilityPaymentSystem.Tests/Tests/UserTests.cs
using System;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using UtilityPaymentSystem.Tests.Helpers;
using UtilityPaymentSystem.Tests.Pages;

namespace UtilityPaymentSystem.Tests.Tests
{
    [TestClass]
    public class UserTests : TestBase
    {
        private UserPage _userPage;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            try
            {
                // Inicializar UserPage después de que Driver esté configurado
                _userPage = new UserPage(Driver);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al inicializar UserPage: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        public void AddUser_ShouldCreateUserSuccessfully()
        {
            try
            {
                // Verifica que ExtentTest no sea null
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Iniciando prueba de creación de usuario");
                }

                // 1. Navegar a la página principal
                Driver.Navigate().GoToUrl(BaseUrl);
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Navegando a la página principal");
                }
                LogScreenshot("HomePage");

                // Asegúrate de que encuentras el elemento antes de hacer clic
                try
                {
                    // 2. Navegar a la sección de usuarios (con manejo de excepciones)
                    IWebElement usuariosLink = Driver.FindElement(By.LinkText("Usuarios"));
                    usuariosLink.Click();
                    if (ExtentTest != null)
                    {
                        ExtentTest.Log(Status.Info, "Navegando a la sección de usuarios");
                    }
                    LogScreenshot("UsersPage");
                }
                catch (NoSuchElementException)
                {
                    // Intenta encontrar el elemento de otra manera si no lo encuentra por LinkText
                    Console.WriteLine("No se encontró el enlace 'Usuarios', intentando buscar por XPath o parcial");

                    try
                    {
                        // Intenta por LinkText parcial
                        Driver.FindElement(By.PartialLinkText("suario")).Click();
                    }
                    catch (NoSuchElementException)
                    {
                        // Intenta otra forma de navegación (por ejemplo, URL directa)
                        Driver.Navigate().GoToUrl(BaseUrl + "/User/Create");
                    }

                    if (ExtentTest != null)
                    {
                        ExtentTest.Log(Status.Info, "Navegando a la sección de usuarios (método alternativo)");
                    }
                    LogScreenshot("UsersPageAlt");
                }

                // El resto de tu código para la prueba...
                // 3. Hacer clic en "Create New" o equivalente
                _userPage.ClickCreateNew();
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Haciendo clic en Crear Usuario");
                }
                LogScreenshot("CreateUserForm");

                // 4. Crear datos de prueba únicos
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string name = $"Test User {timestamp}";
                string email = $"testuser_{timestamp}@example.com";
                string password = "Test@123";

                // 5. Rellenar el formulario con los nuevos campos
                _userPage.FillUserForm(name, email, password);
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, $"Rellenando formulario con nombre: {name}, email: {email}");
                }
                LogScreenshot("FilledUserForm");

                // 6. Enviar el formulario
                _userPage.SubmitForm();
                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Info, "Enviando formulario");
                }
                LogScreenshot("AfterSubmit");

                // 7. Verificar el resultado
                bool isUserCreated = _userPage.IsUserCreatedSuccessfully();
                LogScreenshot("UserCreationResult");

                // 8. Verificar si el usuario aparece en la tabla
                bool isUserInTable = _userPage.IsUserInTable(name);

                // 9. Aserciones
                Assert.IsTrue(isUserCreated, "No se pudo confirmar la creación exitosa del usuario");
                Assert.IsTrue(isUserInTable, $"El usuario '{name}' no aparece en la tabla");

                if (ExtentTest != null)
                {
                    ExtentTest.Log(Status.Pass, "Usuario creado correctamente y verificado");
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