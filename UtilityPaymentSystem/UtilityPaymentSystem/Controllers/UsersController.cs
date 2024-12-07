using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UtilityPaymentSystem.Domain.Entities;
using UtilityPaymentSystem.Infrastructure;

namespace UtilityPaymentSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // Simulación de base de datos en memoria
        private static List<User> Users = new List<User>
        {
            new User { Id = 1, Name = "Juan Pérez", Email = "juan@example.com", PasswordHash = "hashedpassword1" },
            new User { Id = 2, Name = "Ana López", Email = "ana@example.com", PasswordHash = "hashedpassword2" }
        };


        // Acción para listar usuarios
        public IActionResult Index()
        {
            Users = _context.Users.ToList();
            return View(Users);
        }

        // Acción para ver detalles de un usuario
        public IActionResult Details(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
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
        public async Task<ActionResult<User>> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            


            return RedirectToAction("Index");
        }

        //public IActionResult Create(User newUser)
        //{
        //    _context.Users.Add(newUser);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        // Acción para mostrar el formulario de edición
        public IActionResult Edit(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Acción para manejar el envío del formulario de edición
        [HttpPost]
        public IActionResult Edit(User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user == null) return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.PasswordHash = updatedUser.PasswordHash;

            return RedirectToAction("Index");
        }

        // Acción para mostrar el formulario de eliminación
        public IActionResult Delete(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Acción para manejar la confirmación de eliminación
        [HttpPost]
        public IActionResult DeleteConfirmed(int Id)
        {
            var user = _context.Users.Find(Id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
