using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using NUnit.Framework;

namespace UserCreationTests
{
    public class UserCreateTest
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            // Configura las opciones de Edge
            var options = new EdgeOptions();
            // No es necesario usar "UseChromium" explícitamente si ya estás usando una versión reciente de Edge
            driver = new EdgeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void TestCreateUser()
        {
            // Abre la URL de la página de creación de usuario
            driver.Navigate().GoToUrl("https://localhost:7046/User/Create");

            // Encuentra los elementos del formulario y llena los campos
            var nameField = driver.FindElement(By.Id("Name"));
            nameField.SendKeys("Juan Perez");

            var emailField = driver.FindElement(By.Id("Email"));
            emailField.SendKeys("juan.perez@example.com");

            var passwordField = driver.FindElement(By.Id("PasswordHash"));
            passwordField.SendKeys("ContraseñaSegura123");

            // Encuentra y hace clic en el botón "Guardar"
            var saveButton = driver.FindElement(By.CssSelector("button[type='submit']"));
            saveButton.Click();

            // Espera que la página se recargue o que aparezca un mensaje de confirmación
            // Dependiendo de cómo esté configurado tu sistema, esto podría necesitar ajustes
            var successMessage = driver.FindElement(By.CssSelector(".alert-success"));

           // Verifica que el texto del mensaje contenga "Usuario creado"
        }

        [TearDown]
        public void TearDown()
        {
            // Cierra el navegador después de la prueba
            driver.Quit();
        }
    }
}
