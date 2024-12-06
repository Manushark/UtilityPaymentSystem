using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityPaymentSystem.Controllers
{
    public class ReportController : Controller
    {
        // Simulación de base de datos en memoria
        private static List<Report> Reports = new List<Report>
        {
            new Report { ReportId = 1, GeneratedDate = DateTime.Now.AddDays(-7), ReportData = "Reporte semanal de pagos" },
            new Report { ReportId = 2, GeneratedDate = DateTime.Now.AddDays(-1), ReportData = "Reporte diario de facturación" }
        };

        // Acción para listar reportes
        public IActionResult Index()
        {
            return View(Reports);
        }

        // Acción para ver detalles de un reporte
        public IActionResult Details(int id)
        {
            var report = Reports.FirstOrDefault(r => r.ReportId == id);
            if (report == null) return NotFound();
            return View(report);
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar el envío del formulario de creación
        [HttpPost]
        public IActionResult Create(Report newReport)
        {
            newReport.ReportId = Reports.Count + 1; // Generar un nuevo ID
            newReport.GeneratedDate = DateTime.Now; // Fecha de generación actual
            Reports.Add(newReport);
            return RedirectToAction("Index");
        }
    }

    // Clase Report (Modelo)
    public class Report
    {
        public int ReportId { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string ReportData { get; set; }
    }
}
