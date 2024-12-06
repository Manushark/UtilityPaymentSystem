using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Uti.Controllers
{
    public class PaymentController : Controller
    {
        // Simulación de base de datos en memoria
        private static List<Payment> Payments = new List<Payment>
        {
            new Payment { Id = 1, Name = "Pago de Luz", Amount = 50.00m },
            new Payment { Id = 2, Name = "Pago de Agua", Amount = 30.00m }
        };

        // Acción para listar pagos
        public IActionResult Index()
        {
            return View(Payments);
        }

        // Acción para ver detalles de un pago
        public IActionResult Details(int id)
        {
            var payment = Payments.Find(p => p.Id == id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar el envío del formulario de creación
        [HttpPost]
        public IActionResult Create(Payment newPayment)
        {
            newPayment.Id = Payments.Count + 1; // Generar un nuevo ID
            Payments.Add(newPayment);
            return RedirectToAction("Index");
        }
    }

    // Clase Payment (Modelo)
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
