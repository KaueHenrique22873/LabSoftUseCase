using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppTaskZero.Models;

namespace AppTaskZero.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly DbTasksContext _context;

        public FuncionarioController(DbTasksContext context)
        {
            _context = context;
        }

        // GET: Funcionario
        public async Task<IActionResult> Index()
        {
            var funcionarios = _context.Funcionarios.Include(f => f.Gerente);
            return View(await funcionarios.ToListAsync());
        }

        // GET: Funcionario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .Include(f => f.Gerente)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // GET: Funcionario/Create
        public IActionResult Create()
        {
            ViewData["GerenteId"] = new SelectList(_context.Gerentes, "Codigo", "Nome");
            return View();
        }

        // POST: Funcionario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nome,Cargo,GerenteId")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GerenteId"] = new SelectList(_context.Gerentes, "Codigo", "Nome", funcionario.GerenteId);
            return View(funcionario);
        }

        // GET: Funcionario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            ViewData["GerenteId"] = new SelectList(_context.Gerentes, "Codigo", "Nome", funcionario.GerenteId);
            return View(funcionario);
        }

        // POST: Funcionario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Cargo,GerenteId")] Funcionario funcionario)
        {
            if (id != funcionario.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Codigo))
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

            ViewData["GerenteId"] = new SelectList(_context.Gerentes, "Codigo", "Nome", funcionario.GerenteId);
            return View(funcionario);
        }

        // GET: Funcionario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .Include(f => f.Gerente)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // POST: Funcionario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = await _context.Funcionarios
                .Include(f => f.Tarefas)
                .Include(f => f.Incidentes)
                .FirstOrDefaultAsync(f => f.Codigo == id);

            if (funcionario != null)
            {
                _context.Tarefas.RemoveRange(funcionario.Tarefas);
                _context.Incidentes.RemoveRange(funcionario.Incidentes);
                _context.Funcionarios.Remove(funcionario);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Adicionado para resolver o erro de compilação do Edit
        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.Codigo == id);
        }
    }
}