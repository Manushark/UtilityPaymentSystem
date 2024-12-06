using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace UtilityPaymentSystem.Controllers
{
    public class ServiceController : Controller
    {
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
        [HttpPost]
        public IActionResult Create(Service newService)
        {
            newService.ServiceId = Services.Count + 1; // Generar un nuevo ID
            Services.Add(newService);
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

    // Clase Service (Modelo)
    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
    }
}
