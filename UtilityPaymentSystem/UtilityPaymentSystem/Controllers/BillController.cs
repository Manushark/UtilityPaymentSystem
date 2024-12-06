using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityPaymentSystem.Controllers
{
    public class BillController : Controller
    {
        // Simulación de base de datos en memoria
        private static List<Bill> Bills = new List<Bill>
        {
            new Bill { BillId = 1, UserId = 1, ServiceId = 1, Amount = 100.00m, DueDate = DateTime.Now.AddDays(30), IsPaid = false },
            new Bill { BillId = 2, UserId = 2, ServiceId = 2, Amount = 50.00m, DueDate = DateTime.Now.AddDays(15), IsPaid = true }
        };

        // Acción para listar facturas
        public IActionResult Index()
        {
            return View(Bills);
        }

        // Acción para ver detalles de una factura
        public IActionResult Details(int id)
        {
            var bill = Bills.FirstOrDefault(b => b.BillId == id);
            if (bill == null) return NotFound();
            return View(bill);
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar el envío del formulario de creación
        [HttpPost]
        public IActionResult Create(Bill newBill)
        {
            newBill.BillId = Bills.Count + 1; // Generar un nuevo ID
            Bills.Add(newBill);
            return RedirectToAction("Index");
        }

        // Acción para mostrar el formulario de edición
        public IActionResult Edit(int id)
        {
            var bill = Bills.FirstOrDefault(b => b.BillId == id);
            if (bill == null) return NotFound();
            return View(bill);
        }

        // Acción para manejar el envío del formulario de edición
        [HttpPost]
        public IActionResult Edit(Bill updatedBill)
        {
            var bill = Bills.FirstOrDefault(b => b.BillId == updatedBill.BillId);
            if (bill == null) return NotFound();

            bill.UserId = updatedBill.UserId;
            bill.ServiceId = updatedBill.ServiceId;
            bill.Amount = updatedBill.Amount;
            bill.DueDate = updatedBill.DueDate;
            bill.IsPaid = updatedBill.IsPaid;

            return RedirectToAction("Index");
        }
    }

    // Clase Bill (Modelo)
    public class Bill
    {
        public int BillId { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
