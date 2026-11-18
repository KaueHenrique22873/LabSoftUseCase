using AppClinicaOdontoSLN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppClinicaOdontoSLN.Controllers
{
    [Authorize]
    public class AgendamentoController : Controller
    {
        private readonly DbodontoContext _context;

        public AgendamentoController(DbodontoContext context)
        {
            _context = context;
        }

        // GET: Agendamento
        public async Task<IActionResult> Index()
        {
            var dbodontoContext = _context.Agendamentos.Include(a => a.CodigoMedicoNavigation).Include(a => a.CodigoPacienteNavigation).Include(a => a.CodigoProcedimentoNavigation);
            return View(await dbodontoContext.ToListAsync());
        }

        // GET: Agendamento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.CodigoMedicoNavigation)
                .Include(a => a.CodigoPacienteNavigation)
                .Include(a => a.CodigoProcedimentoNavigation)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamento/Create
        public IActionResult Create()
        {
            ViewData["CodigoMedico"] = new SelectList(_context.Medicos, "Codigo", "Codigo");
            ViewData["CodigoPaciente"] = new SelectList(_context.Pacientes, "Codigo", "Codigo");
            ViewData["CodigoProcedimento"] = new SelectList(_context.Procedimentos, "Codigo", "Codigo");
            return View();
        }

        // POST: Agendamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,DataHora,CodigoProcedimento,CodigoPaciente,CodigoMedico")] Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoMedico"] = new SelectList(_context.Medicos, "Codigo", "Codigo", agendamento.CodigoMedico);
            ViewData["CodigoPaciente"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", agendamento.CodigoPaciente);
            ViewData["CodigoProcedimento"] = new SelectList(_context.Procedimentos, "Codigo", "Codigo", agendamento.CodigoProcedimento);
            return View(agendamento);
        }

        // GET: Agendamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound();
            }
            ViewData["CodigoMedico"] = new SelectList(_context.Medicos, "Codigo", "Codigo", agendamento.CodigoMedico);
            ViewData["CodigoPaciente"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", agendamento.CodigoPaciente);
            ViewData["CodigoProcedimento"] = new SelectList(_context.Procedimentos, "Codigo", "Codigo", agendamento.CodigoProcedimento);
            return View(agendamento);
        }

        // POST: Agendamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,DataHora,CodigoProcedimento,CodigoPaciente,CodigoMedico")] Agendamento agendamento)
        {
            if (id != agendamento.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agendamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendamentoExists(agendamento.Codigo))
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
            ViewData["CodigoMedico"] = new SelectList(_context.Medicos, "Codigo", "Codigo", agendamento.CodigoMedico);
            ViewData["CodigoPaciente"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", agendamento.CodigoPaciente);
            ViewData["CodigoProcedimento"] = new SelectList(_context.Procedimentos, "Codigo", "Codigo", agendamento.CodigoProcedimento);
            return View(agendamento);
        }

        // GET: Agendamento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.CodigoMedicoNavigation)
                .Include(a => a.CodigoPacienteNavigation)
                .Include(a => a.CodigoProcedimentoNavigation)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgendamentoExists(int id)
        {
            return _context.Agendamentos.Any(e => e.Codigo == id);
        }
    }
}
