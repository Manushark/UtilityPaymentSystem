using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UtilityPaymentSystem.Domain.Entities;
using UtilityPaymentSystem.Infrastructure;

namespace UtilityPaymentSystem.Controllers
{

    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        // Simulación de base de datos en memoria
        private static List<Service> Services = new List<Service>
        {
            new Service { ServiceId = 1, Name = "Electricidad" },
            new Service { ServiceId = 2, Name = "Agua" },
            new Service { ServiceId = 3, Name = "Gas" }
        };

        // Acción para listar servicios
        public IActionResult Index()
        {
            Services = _context.Services.ToList();
            return View(Services);
        }

        // Acción para ver detalles de un servicio
        public IActionResult Details(int id)
        {
            var service = Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null) return NotFound();
            return View(service);
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar el envío del formulario de creación
        //[HttpPost]
        //public IActionResult Create(Service newService)
        //{

        //    _context.Services.Add(newService);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public async Task<ActionResult<Service>> Create(Service newService)
        {
            _context.Services.Add(newService);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        // Acción para mostrar el formulario de edición
        public IActionResult Edit(int id)
        {
            var service = Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null) return NotFound();
            return View(service);
        }

        // Acción para manejar el envío del formulario de edición
        [HttpPost]
        public IActionResult Edit(Service updatedService)
        {
            var service = Services.FirstOrDefault(s => s.ServiceId == updatedService.ServiceId);
            if (service == null) return NotFound();

            service.Name = updatedService.Name;
            return RedirectToAction("Index");
        }

        // Acción para mostrar el formulario de confirmación de eliminación
        public IActionResult Delete(int id)
        {
            var service = Services.FirstOrDefault(s => s.ServiceId == id);
            if (service == null) return NotFound();
            return View(service);
        }

        // Acción para manejar la eliminación
        [HttpPost]
        public IActionResult DeleteConfirmed(int Serviceid)
        {
            var service = Services.FirstOrDefault(s => s.ServiceId == Serviceid);
            if (service == null) return NotFound();

            Services.Remove(service);
            return RedirectToAction("Index");
        }
      

    }
}
