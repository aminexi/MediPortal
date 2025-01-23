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
    public class RendezVousController : Controller
    {
        private readonly HopitaldbContext _context;
        private readonly ILogger<RendezVousController> _logger;

        public RendezVousController(HopitaldbContext context, ILogger<RendezVousController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: RendezVous
        public async Task<IActionResult> Index()
        {
            var hopitaldbContext = _context.RendezVous.Include(r => r.DossierMedical).Include(r => r.Medecin);
            return View(await hopitaldbContext.ToListAsync());
        }

        // GET: RendezVous/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVou = await _context.RendezVous
                .Include(r => r.DossierMedical)
                .Include(r => r.Medecin)
                .FirstOrDefaultAsync(m => m.RendezVousId == id);
            if (rendezVou == null)
            {
                return NotFound();
            }

            return View(rendezVou);
        }

        // GET: RendezVous/Create
        public IActionResult Create()
        {
            ViewData["DossierMedicalId"] = new SelectList(_context.DossierMedicals, "DossierMedicalId", "DossierMedicalId");
            ViewBag.MedecinId = new SelectList(
                _context.Medecins.Select(m => new { m.MedecinId, NomPrenom = m.Nom + " " + m.Prenom }),
                "MedecinId",
                "NomPrenom"
            );
            return View();
        }

        // POST: RendezVous/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RendezVousId,DossierMedicalId,MedecinId,DateHeure,Statut,Motif")] RendezVou rendezVou)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rendezVou);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DossierMedicalId"] = new SelectList(_context.DossierMedicals, "DossierMedicalId", "DossierMedicalId", rendezVou.DossierMedicalId);
            ViewData["MedecinId"] = new SelectList(_context.Medecins, "MedecinId", "MedecinId", rendezVou.MedecinId);
            return View(rendezVou);
        }

        // GET: RendezVous/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVou = await _context.RendezVous.FindAsync(id);
            if (rendezVou == null)
            {
                return NotFound();
            }
            ViewData["DossierMedicalId"] = new SelectList(_context.DossierMedicals, "DossierMedicalId", "DossierMedicalId", rendezVou.DossierMedicalId);
            ViewData["MedecinId"] = new SelectList(_context.Medecins, "MedecinId", "MedecinId", rendezVou.MedecinId);
            return View(rendezVou);
        }

        // POST: RendezVous/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RendezVousId,DossierMedicalId,MedecinId,DateHeure,Statut,Motif")] RendezVou rendezVou)
        {
            if (id != rendezVou.RendezVousId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rendezVou);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DossierMedicalId"] = new SelectList(_context.DossierMedicals, "DossierMedicalId", "DossierMedicalId", rendezVou.DossierMedicalId);
            ViewData["MedecinId"] = new SelectList(_context.Medecins, "MedecinId", "MedecinId", rendezVou.MedecinId);
            return View(rendezVou);
        }

        // GET: RendezVous/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rendezVou = await _context.RendezVous
                .Include(r => r.DossierMedical)
                .Include(r => r.Medecin)
                .FirstOrDefaultAsync(m => m.RendezVousId == id);
            if (rendezVou == null)
            {
                return NotFound();
            }

            return View(rendezVou);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var rendezVous = await _context.RendezVous
                    .Include(r => r.Consultations)
                    .FirstOrDefaultAsync(r => r.RendezVousId == id);

                if (rendezVous == null)
                {
                    return NotFound();
                }

                // Remove related consultations
                _context.Consultations.RemoveRange(rendezVous.Consultations);

                // Remove the rendez-vous
                _context.RendezVous.Remove(rendezVous);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting RendezVous with ID: {id}");
                return View("Error");
            }
        }
    }
}
