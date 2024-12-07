using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityPaymentSystem.Domain.Entities;
using UtilityPaymentSystem.Infrastructure;

namespace UtilityPaymentSystem.Controllers
{
    public class BillController : Controller
    {
        private readonly AppDbContext _context;

        public BillController(AppDbContext context)
        {
            _context = context;
        }
        private static List<Bill> Bills = new List<Bill>
        {
            new Bill { BillId = 1, UserId = 1, ServiceId = 1, Amount = 100.50m, DueDate = DateTime.Now.AddDays(3), IsPaid = false },
            new Bill { BillId = 2, UserId = 2, ServiceId = 2, Amount = 200.00m, DueDate = DateTime.Now.AddDays(-1), IsPaid = false },
            new Bill { BillId = 3, UserId = 1, ServiceId = 1, Amount = 150.00m, DueDate = DateTime.Now.AddDays(10), IsPaid = true }
        };
        // Constructor para inyectar el contexto
     
        // Acción para listar facturas con filtros
        public IActionResult Index(string filter = "all", int? userId = null)
        {
            var filteredBills = Bills.AsQueryable();

            if (filter == "paid")
                filteredBills = filteredBills.Where(b => b.IsPaid);
            else if (filter == "pending")
                filteredBills = filteredBills.Where(b => !b.IsPaid);

            if (userId.HasValue)
                filteredBills = filteredBills.Where(b => b.UserId == userId.Value);

            return View(filteredBills.ToList());
        }
    

        [HttpPost]
        public IActionResult Create(Bill newBill)
        {
            newBill.BillId = Bills.Count + 1;
            Bills.Add(newBill);
            return RedirectToAction("Index");
        }

        // Acción para crear nueva factura
        public IActionResult Create()
        {
            return View();
        }

      
        // Acción para ver detalles de una factura
        public IActionResult Details(int id)
        {
            var bill = Bills.FirstOrDefault(b => b.BillId == id);
            if (bill == null) return NotFound();
            return View(bill);
        }

        // Acción para eliminar una factura
        public IActionResult Delete(int id)
        {
            var bill = Bills.FirstOrDefault(b => b.BillId == id);
            if (bill == null) return NotFound();
            return View(bill);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var bill = Bills.FirstOrDefault(b => b.BillId == id);
            if (bill != null)
                Bills.Remove(bill);

            return RedirectToAction("Index");
        }

        // Acción para marcar como pagada
        public IActionResult MarkAsPaid(int id)
        {
            var bill = Bills.FirstOrDefault(b => b.BillId == id);
            if (bill == null) return NotFound();

            bill.IsPaid = true;
            return RedirectToAction("Index");
        }

        // Acción para agrupar por usuario
        public IActionResult GroupByUser()
        {
            var groupedBills = Bills.GroupBy(b => b.UserId)
                .Select(group => new
                {
                    UserId = group.Key,
                    TotalAmount = group.Sum(b => b.Amount),
                    Bills = group.ToList()
                }).ToList();

            return View(groupedBills);
        }

        // Acción para mostrar facturas próximas a vencer
        public IActionResult UpcomingDue()
        {
            var upcomingBills = Bills.Where(b => !b.IsPaid && b.DueDate <= DateTime.Now.AddDays(7));
            return View(upcomingBills.ToList());
        }
    }
}
