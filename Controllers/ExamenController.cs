using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HOPITAL2.Models;

namespace HOPITAL2.Controllers
{
    public class ExamenController : Controller
    {
        private readonly HopitaldbContext _context;

        public ExamenController(HopitaldbContext context)
        {
            _context = context;
        }

        // GET: Examen
        public async Task<IActionResult> Index()
        {
            var hopitaldbContext = _context.Examen.Include(e => e.DossierMedical);
            return View(await hopitaldbContext.ToListAsync());
        }

        // GET: Examen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examan = await _context.Examen
                .Include(e => e.DossierMedical)
                .FirstOrDefaultAsync(m => m.ExamenId == id);
            if (examan == null)
            {
                return NotFound();
            }

            return View(examan);
        }

        // GET: Examen/Create
        public IActionResult Create()
        {
            ViewData["DossierMedicalId"] = new SelectList(_context.DossierMedicals, "DossierMedicalId", "DossierMedicalId");
            return View();
        }

        // POST: Examen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExamenId,DossierMedicalId,TypeExamen,DateExamen,Resultats")] Examan examan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DossierMedicalId"] = new SelectList(_context.DossierMedicals, "DossierMedicalId", "DossierMedicalId", examan.DossierMedicalId);
            return View(examan);
        }

        // GET: Examen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examan = await _context.Examen.FindAsync(id);
            if (examan == null)
            {
                return NotFound();
            }
            ViewData["DossierMedicalId"] = new SelectList(_context.DossierMedicals, "DossierMedicalId", "DossierMedicalId", examan.DossierMedicalId);
            return View(examan);
        }

        // POST: Examen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExamenId,DossierMedicalId,TypeExamen,DateExamen,Resultats")] Examan examan)
        {
            if (id != examan.ExamenId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamanExists(examan.ExamenId))
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
            ViewData["DossierMedicalId"] = new SelectList(_context.DossierMedicals, "DossierMedicalId", "DossierMedicalId", examan.DossierMedicalId);
            return View(examan);
        }

        // GET: Examen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examan = await _context.Examen
                .Include(e => e.DossierMedical)
                .FirstOrDefaultAsync(m => m.ExamenId == id);
            if (examan == null)
            {
                return NotFound();
            }

            return View(examan);
        }

        // POST: Examen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examan = await _context.Examen.FindAsync(id);
            if (examan != null)
            {
                _context.Examen.Remove(examan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamanExists(int id)
        {
            return _context.Examen.Any(e => e.ExamenId == id);
        }
    }
}
