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
    public class TutorController : Controller
    {
        private readonly DbPetShopContext _context;

        public TutorController(DbPetShopContext context)
        {
            _context = context;
        }

        // GET: Tutor
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            if (isAdmin)
            {
                // Admin vê todos os tutores
                var todosTutores = await _context.Tutor.ToListAsync();
                return View(todosTutores);
            }
            else
            {
                // Usuário comum vê apenas o seu próprio perfil
                var meuPerfil = await _context.Tutor.Where(t => t.Codigo == userId).ToListAsync();
                return View(meuPerfil);
            }
        }

        // GET: Tutor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            // Bloqueia se o usuário comum tentar ver detalhes de outro tutor
            if (!isAdmin && id != userId)
            {
                return Forbid();
            }

            var tutor = await _context.Tutor
                .Include(t => t.Pets) // Inclui a lista de pets do tutor se necessário na View
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // GET: Tutor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tutor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nome,Cpf,Senha,PerfilAcesso,DataCriacao")] Tutor tutor)
        {
            // Garante valores padrão se não forem preenchidos no form
            if (string.IsNullOrEmpty(tutor.PerfilAcesso))
            {
                tutor.PerfilAcesso = "user";
            }

            if (tutor.DataCriacao == default)
            {
                tutor.DataCriacao = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                _context.Add(tutor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tutor);
        }

        // GET: Tutor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            // Impede usuário comum de editar cadastro alheio
            if (!isAdmin && id != userId)
            {
                return Forbid();
            }

            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }
            return View(tutor);
        }

        // POST: Tutor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Cpf,Senha,PerfilAcesso,DataCriacao")] Tutor tutor)
        {
            if (id != tutor.Codigo)
            {
                return NotFound();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            // Validação adicional no POST para evitar alteração não autorizada
            if (!isAdmin && id != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorExists(tutor.Codigo))
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
            return View(tutor);
        }

        // GET: Tutor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            // Apenas admin ou o próprio usuário podem deletar a conta
            if (!isAdmin && id != userId)
            {
                return Forbid();
            }

            var tutor = await _context.Tutor
                .Include(t => t.Pets)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // POST: Tutor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            if (!isAdmin && id != userId)
            {
                return Forbid();
            }

            var tutor = await _context.Tutor.FindAsync(id);
            if (tutor != null)
            {
                _context.Tutor.Remove(tutor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorExists(int id)
        {
            return _context.Tutor.Any(e => e.Codigo == id);
        }
    }
}