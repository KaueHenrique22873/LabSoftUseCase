using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetShop.Controllers
{
    public class PetController : Controller
    {
        private readonly DbPetShopContext _context;

        public PetController(DbPetShopContext context)
        {
            _context = context;
        }

        // GET: Pet
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            if (isAdmin)
            {
                // Admin: Traz TODOS os pets carregando a relação com o Tutor
                var todosPets = await _context.Pets
                    .Include(p => p.CodigoTutorNavigation)
                    .ToListAsync();

                return View(todosPets);
            }
            else
            {
                // Usuário comum: Traz apenas os PETS dele
                var meusPets = await _context.Pets
                    .Include(p => p.CodigoTutorNavigation)
                    .Where(p => p.CodigoTutor == userId)
                    .ToListAsync();

                return View(meusPets);
            }
        }

        // GET: Pet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.CodigoTutorNavigation)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pet/Create
        public IActionResult Create()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("admin");

            if (isAdmin)
            {
                // Admin pode escolher qualquer Tutor pelo Nome
                ViewData["CodigoTutor"] = new SelectList(_context.Tutor, "Codigo", "Nome");
            }
            else if (!string.IsNullOrEmpty(userIdString))
            {
                // Usuário comum tem o seu próprio Tutor pré-selecionado pelo Nome
                int userId = int.Parse(userIdString);
                ViewData["CodigoTutor"] = new SelectList(_context.Tutor.Where(t => t.Codigo == userId), "Codigo", "Nome", userId);
            }

            return View();
        }

        // POST: Pet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nome,CodigoTutor")] Pet pet)
        {
            // Se for usuário comum, garante que o pet seja associado ao seu próprio ID
            if (!User.IsInRole("admin"))
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userIdString))
                {
                    pet.CodigoTutor = int.Parse(userIdString);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CodigoTutor"] = new SelectList(_context.Tutor, "Codigo", "Nome", pet.CodigoTutor);
            return View(pet);
        }

        // GET: Pet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            // Exibe o Nome do Tutor no campo de seleção
            ViewData["CodigoTutor"] = new SelectList(_context.Tutor, "Codigo", "Nome", pet.CodigoTutor);
            return View(pet);
        }

        // POST: Pet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,CodigoTutor")] Pet pet)
        {
            if (id != pet.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.Codigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CodigoTutor"] = new SelectList(_context.Tutor, "Codigo", "Nome", pet.CodigoTutor);
            return View(pet);
        }

        // GET: Pet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.CodigoTutorNavigation)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.Codigo == id);
        }
    }
}