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
    public class AgendamentoController : Controller
    {
        private readonly DbPetShopContext _context;

        public AgendamentoController(DbPetShopContext context)
        {
            _context = context;
        }

        // GET: Agendamento
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            if (isAdmin)
            {
                // Admin vê agendamentos de todos os pets
                var todosAgendamentos = await _context.Agendamento
                    .Include(a => a.CodigoPetNavigation)
                        .ThenInclude(p => p.CodigoTutorNavigation)
                    .ToListAsync();

                return View(todosAgendamentos);
            }
            else
            {
                // Usuário comum vê apenas agendamentos dos seus próprios pets
                var meusAgendamentos = await _context.Agendamento
                    .Include(a => a.CodigoPetNavigation)
                        .ThenInclude(p => p.CodigoTutorNavigation)
                    .Where(a => a.CodigoPetNavigation.CodigoTutor == userId)
                    .ToListAsync();

                return View(meusAgendamentos);
            }
        }

        // GET: Agendamento/Create
        public IActionResult Create()
        {
            CarregarSelectPets();
            return View();
        }

        // POST: Agendamento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,MotivoAgendamento,DataHora,CodigoPet")] Agendamento agendamento)
        {
            // Remove da validação as propriedades de navegação para não bloquear o ModelState
            ModelState.Remove("CodigoPetNavigation");

            if (ModelState.IsValid)
            {
                _context.Add(agendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CarregarSelectPets(agendamento.CodigoPet);
            return View(agendamento);
        }

        // GET: Agendamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var agendamento = await _context.Agendamento.FindAsync(id);
            if (agendamento == null) return NotFound();

            CarregarSelectPets(agendamento.CodigoPet);
            return View(agendamento);
        }

        // POST: Agendamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,MotivoAgendamento,DataHora,CodigoPet")] Agendamento agendamento)
        {
            if (id != agendamento.Codigo) return NotFound();

            ModelState.Remove("CodigoPetNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agendamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendamentoExists(agendamento.Codigo)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            CarregarSelectPets(agendamento.CodigoPet);
            return View(agendamento);
        }

        // GET: Agendamento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var agendamento = await _context.Agendamento
                .Include(a => a.CodigoPetNavigation)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (agendamento == null) return NotFound();

            return View(agendamento);
        }

        // POST: Agendamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agendamento = await _context.Agendamento.FindAsync(id);
            if (agendamento != null)
            {
                _context.Agendamento.Remove(agendamento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AgendamentoExists(int id)
        {
            return _context.Agendamento.Any(e => e.Codigo == id);
        }

        // Método auxiliar para popular a lista de seleção de pets (exibe o nome do pet)
        private void CarregarSelectPets(object petSelecionado = null)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return;

            var userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("admin");

            // Se for Admin mostra todos os pets, se for Usuário comum mostra apenas os seus pets
            var queryPets = isAdmin
                ? _context.Pets.AsQueryable()
                : _context.Pets.Where(p => p.CodigoTutor == userId);

            ViewData["CodigoPet"] = new SelectList(queryPets, "Codigo", "Nome", petSelecionado);
        }
    }
}