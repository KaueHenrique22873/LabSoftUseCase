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
    public class GerenteController : Controller
    {
        private readonly DbTasksContext _context;

        public GerenteController(DbTasksContext context)
        {
            _context = context;
        }

        // GET: Gerente
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gerentes.ToListAsync());
        }

        // GET: Gerente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerente = await _context.Gerentes
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (gerente == null)
            {
                return NotFound();
            }

            return View(gerente);
        }

        // GET: Gerente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gerente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nome,Setor")] Gerente gerente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gerente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gerente);
        }

        // GET: Gerente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerente = await _context.Gerentes.FindAsync(id);
            if (gerente == null)
            {
                return NotFound();
            }
            return View(gerente);
        }

        // POST: Gerente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nome,Setor")] Gerente gerente)
        {
            if (id != gerente.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gerente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GerenteExists(gerente.Codigo))
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
            return View(gerente);
        }

        // GET: Gerente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gerente = await _context.Gerentes
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (gerente == null)
            {
                return NotFound();
            }

            return View(gerente);
        }

        // POST: Gerente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gerente = await _context.Gerentes.FindAsync(id);
            if (gerente != null)
            {
                _context.Gerentes.Remove(gerente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GerenteExists(int id)
        {
            return _context.Gerentes.Any(e => e.Codigo == id);
        }
    }
}
