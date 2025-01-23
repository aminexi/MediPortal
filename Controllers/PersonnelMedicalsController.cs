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
    public class PersonnelMedicalsController : Controller
    {
        private readonly HopitaldbContext _context;

        public PersonnelMedicalsController(HopitaldbContext context)
        {
            _context = context;
        }

        // GET: PersonnelMedicals
        public async Task<IActionResult> Index()
        {
            return View(await _context.PersonnelMedicals.ToListAsync());
        }

        // GET: PersonnelMedicals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnelMedical = await _context.PersonnelMedicals
                .FirstOrDefaultAsync(m => m.PersonnelID == id);
            if (personnelMedical == null)
            {
                return NotFound();
            }

            return View(personnelMedical);
        }

        // GET: PersonnelMedicals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PersonnelMedicals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonnelID,Nom,Prenom,Fonction")] PersonnelMedical personnelMedical)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personnelMedical);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personnelMedical);
        }

        // GET: PersonnelMedicals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnelMedical = await _context.PersonnelMedicals.FindAsync(id);
            if (personnelMedical == null)
            {
                return NotFound();
            }
            return View(personnelMedical);
        }

        // POST: PersonnelMedicals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonnelID,Nom,Prenom,Fonction")] PersonnelMedical personnelMedical)
        {
            if (id != personnelMedical.PersonnelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personnelMedical);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonnelMedicalExists(personnelMedical.PersonnelID))
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
            return View(personnelMedical);
        }

        // GET: PersonnelMedicals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personnelMedical = await _context.PersonnelMedicals
                .FirstOrDefaultAsync(m => m.PersonnelID == id);
            if (personnelMedical == null)
            {
                return NotFound();
            }

            return View(personnelMedical);
        }

        // POST: PersonnelMedicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personnelMedical = await _context.PersonnelMedicals.FindAsync(id);
            if (personnelMedical != null)
            {
                _context.PersonnelMedicals.Remove(personnelMedical);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonnelMedicalExists(int id)
        {
            return _context.PersonnelMedicals.Any(e => e.PersonnelID == id);
        }
    }
}
