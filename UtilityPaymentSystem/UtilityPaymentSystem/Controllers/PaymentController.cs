using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UtilityPaymentSystem.Domain.Entities;
using UtilityPaymentSystem.Infrastructure;

namespace UtilityPaymentSystem.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }
        // Simulación de base de datos en memoria
        private static List<Payment> Payments = new List<Payment>
        {
            new Payment { Id = 1, Name = "Pago de Luz", Amount = 50.00m },
            new Payment { Id = 2, Name = "Pago de Agua", Amount = 30.00m }
        };

        // Acción para listar pagos
        public IActionResult Index()
        {
            Payments = _context.Payment.ToList();
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
		// Acción para eliminar un pago
		public IActionResult Delete(int id)
		{
			var payment = Payments.Find(p => p.Id == id);
			if (payment == null) return NotFound();

			Payments.Remove(payment);
			return RedirectToAction("Index");
		}
        // Acción para buscar pagos
        public IActionResult Search(string query, decimal? minAmount, decimal? maxAmount)
        {
            var results = Payments;

            if (!string.IsNullOrEmpty(query))
                results = results.FindAll(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase));

            if (minAmount.HasValue)
                results = results.FindAll(p => p.Amount >= minAmount);

            if (maxAmount.HasValue)
                results = results.FindAll(p => p.Amount <= maxAmount);

            return View("Index", results);
        }


        // Acción para manejar el envío del formulario de creación
   
        [HttpPost]
        public async Task<ActionResult<Payment>> Create(Payment payment)
        {
            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
