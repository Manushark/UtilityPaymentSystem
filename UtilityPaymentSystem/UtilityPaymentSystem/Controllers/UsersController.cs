using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UtilityPaymentSystem.Domain.Entities;

namespace UtilityPaymentSystem.Controllers
{
    public class UserController : Controller
    {
        // Simulación de base de datos en memoria
        private static List<User> Users = new List<User>
        {
            new User { UserId = 1, Name = "Juan Pérez", Email = "juan@example.com", PasswordHash = "hashedpassword1" },
            new User { UserId = 2, Name = "Ana López", Email = "ana@example.com", PasswordHash = "hashedpassword2" }
        };

        // Acción para listar usuarios
        public IActionResult Index()
        {
            return View(Users);
        }

        // Acción para ver detalles de un usuario
        public IActionResult Details(int id)
        {
            var user = Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar el envío del formulario de creación
        [HttpPost]
        public IActionResult Create(User newUser)
        {
            newUser.UserId = Users.Count > 0 ? Users.Max(u => u.UserId) + 1 : 1; // Generar un nuevo ID
            Users.Add(newUser);
            return RedirectToAction("Index");
        }

        // Acción para mostrar el formulario de edición
        public IActionResult Edit(int id)
        {
            var user = Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Acción para manejar el envío del formulario de edición
        [HttpPost]
        public IActionResult Edit(User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.UserId == updatedUser.UserId);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.PasswordHash = updatedUser.PasswordHash;

            return RedirectToAction("Index");
        }

        // Acción para mostrar el formulario de eliminación
        public IActionResult Delete(int id)
        {
            var user = Users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Acción para manejar la confirmación de eliminación
        [HttpPost]
        public IActionResult DeleteConfirmed(int UserId)
        {
            var user = Users.FirstOrDefault(u => u.UserId == UserId);
            if (user == null) return NotFound();

            Users.Remove(user);
            return RedirectToAction("Index");
        }
    }
}
