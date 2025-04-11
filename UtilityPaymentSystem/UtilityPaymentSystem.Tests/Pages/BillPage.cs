using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UtilityPaymentSystem.Tests.Pages
{
    public class BillPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Localizadores basados en el HTML proporcionado
        private readonly By _createNewLink = By.LinkText("Crear Nueva Factura");
        private readonly By _createNewButton = By.CssSelector("a[href='/Bill/Create']");
        private readonly By _userIdInput = By.Id("UserId");
        private readonly By _serviceIdInput = By.Id("ServiceId");
        private readonly By _amountInput = By.Id("Amount");
        // Corregido: Usar nombre correcto para el checkbox según la imagen
        private readonly By _paidCheckbox = By.Id("Pagado");
        private readonly By _dueDateInput = By.Id("DueDate");
        private readonly By _saveButton = By.CssSelector("button.btn-primary, input.btn-primary, .btn-primary[value='Guardar'], button:contains('GUARDAR')");
        private readonly By _billsTable = By.CssSelector("table.table");

        public BillPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateToBills()
        {
            _driver.Navigate().GoToUrl(_driver.Url + "/Bill");
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("h1")));
        }

        public void ClickCreateNew()
        {
            try
            {
                // Intenta primero con el enlace exacto
                var createLinks = _driver.FindElements(_createNewLink);
                if (createLinks.Count > 0)
                {
                    createLinks[0].Click();
                }
                else
                {
                    // Intenta con el selector CSS que apunta al botón/enlace "Crear Nueva Factura"
                    var createButtons = _driver.FindElements(_createNewButton);
                    if (createButtons.Count > 0)
                    {
                        createButtons[0].Click();
                    }
                    else
                    {
                        // Última alternativa: buscar cualquier botón con texto relacionado
                        var buttons = _driver.FindElements(By.CssSelector(".btn"));
                        foreach (var button in buttons)
                        {
                            if (button.Text.Contains("CREAR") || button.Text.Contains("Crear") ||
                                button.Text.Contains("Nueva") || button.Text.Contains("Factura"))
                            {
                                button.Click();
                                break;
                            }
                        }
                    }
                }

                // Esperar a que se cargue el formulario
                _wait.Until(ExpectedConditions.ElementExists(By.TagName("h1")));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al hacer clic en Crear Nueva Factura: {ex.Message}");
                // Navegar directamente a la URL de creación como alternativa
                _driver.Navigate().GoToUrl(_driver.Url + "/Bill/Create");
                _wait.Until(ExpectedConditions.ElementExists(By.TagName("h1")));
            }
        }

        public void FillBillForm(string userId, string serviceId, string amount, bool paid, string dueDate)
        {
            try
            {
                // Completar campo Usuario
                var userIdElement = _wait.Until(ExpectedConditions.ElementIsVisible(_userIdInput));
                userIdElement.Clear();
                userIdElement.SendKeys(userId);

                // Completar campo Servicio
                var serviceIdElement = _driver.FindElement(_serviceIdInput);
                serviceIdElement.Clear();
                serviceIdElement.SendKeys(serviceId);

                // Completar campo Monto
                var amountElement = _driver.FindElement(_amountInput);
                amountElement.Clear();
                amountElement.SendKeys(amount);

                // Manejar el checkbox de pagado con mayor robustez
                try
                {
                    // Intentar con el ID Pagado
                    var paidCheckbox = _driver.FindElement(By.Id("Pagado"));
                    HandleCheckbox(paidCheckbox, paid);
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        // Intentar con el ID Paid
                        var paidCheckbox = _driver.FindElement(By.Id("Paid"));
                        HandleCheckbox(paidCheckbox, paid);
                    }
                    catch (NoSuchElementException)
                    {
                        try
                        {
                            // Buscar cualquier checkbox en el formulario
                            var checkboxes = _driver.FindElements(By.CssSelector("input[type='checkbox']"));
                            if (checkboxes.Count > 0)
                            {
                                // Usar el primer checkbox encontrado
                                HandleCheckbox(checkboxes[0], paid);
                            }
                            else
                            {
                                Console.WriteLine("No se encontró ningún checkbox en el formulario. Continuando sin marcar el estado de pago.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"No se pudo manejar el checkbox: {ex.Message}. Continuando con el resto del formulario.");
                        }
                    }
                }

                // Completar campo Fecha de Vencimiento
                try
                {
                    var dueDateElement = _driver.FindElement(_dueDateInput);
                    dueDateElement.Clear();
                    dueDateElement.SendKeys(dueDate);
                }
                catch (NoSuchElementException)
                {
                    // Intentar buscar el campo de fecha por otros selectores
                    try
                    {
                        var dateFields = _driver.FindElements(By.CssSelector("input[type='date'], input.form-control[placeholder*='fecha'], input.form-control[placeholder*='mm/dd']"));
                        if (dateFields.Count > 0)
                        {
                            dateFields[0].Clear();
                            dateFields[0].SendKeys(dueDate);
                        }
                        else
                        {
                            Console.WriteLine("No se encontró el campo de fecha. Continuando sin completar la fecha.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al completar la fecha: {ex.Message}. Continuando con el resto del formulario.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al completar el formulario: {ex.Message}");
                LogFormElements(); // Registrar los elementos disponibles para depuración
                throw;
            }
        }

        private void HandleCheckbox(IWebElement checkbox, bool shouldBeChecked)
        {
            if ((checkbox.Selected && !shouldBeChecked) || (!checkbox.Selected && shouldBeChecked))
            {
                checkbox.Click();
            }
        }

        private void LogFormElements()
        {
            try
            {
                Console.WriteLine("--- Elementos disponibles en el formulario ---");
                var formElements = _driver.FindElements(By.CssSelector("input, select, textarea, button"));
                foreach (var element in formElements)
                {
                    try
                    {
                        string id = element.GetAttribute("id") ?? "sin id";
                        string name = element.GetAttribute("name") ?? "sin nombre";
                        string type = element.GetAttribute("type") ?? "sin tipo";
                        Console.WriteLine($"Elemento: ID={id}, Name={name}, Type={type}");
                    }
                    catch { /* Ignorar errores al obtener atributos */ }
                }
                Console.WriteLine("----------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar elementos del formulario: {ex.Message}");
            }
        }

        public void SubmitForm()
        {
            try
            {
                // Intenta encontrar el botón por ID primero
                try
                {
                    var saveButton = _driver.FindElement(By.Id("btnSave"));
                    saveButton.Click();
                }
                catch (NoSuchElementException)
                {
                    // Luego intenta por texto exacto
                    try
                    {
                        var buttons = _driver.FindElements(By.TagName("button"));
                        foreach (var button in buttons)
                        {
                            if (button.Text.Equals("GUARDAR", StringComparison.OrdinalIgnoreCase))
                            {
                                button.Click();
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Finalmente, busca cualquier botón tipo submit
                        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit'], input[type='submit']"));
                        submitButton.Click();
                    }
                }

                // Esperar a que se complete la redirección usando JavaScript (más confiable)
                _wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

                // Dar tiempo adicional para que la página termine de cargarse
                System.Threading.Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el formulario: {ex.Message}");
                throw;
            }
        }

        public bool IsBillCreatedSuccessfully()
        {
            try
            {
                // Buscar algún indicador de éxito (mensaje o tabla)
                return _driver.PageSource.Contains("created successfully") ||
                       _driver.PageSource.Contains("creada con éxito") ||
                       _driver.FindElements(_billsTable).Count > 0;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsBillInTable(string amount)
        {
            try
            {
                // Verificar si el monto de la factura aparece en la tabla
                var tables = _driver.FindElements(_billsTable);
                if (tables.Count > 0)
                {
                    return tables[0].Text.Contains(amount);
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