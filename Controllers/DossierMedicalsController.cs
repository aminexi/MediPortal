using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HOPITAL2.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
using Microsoft.Extensions.Logging;

namespace HOPITAL2.Controllers
{
    public class DossierMedicalsController : Controller
    {
        private readonly HopitaldbContext _context;
        private readonly ILogger<DossierMedicalsController> _logger;

        public DossierMedicalsController(HopitaldbContext context, ILogger<DossierMedicalsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DossierMedicals
     public async Task<IActionResult> Index()
{
    try
    {
        var dossierMedicals = await _context.DossierMedicals
            .Include(d => d.Patient)
            .Include(d => d.Consultations)
            .Include(d => d.Examen)
            .Include(d => d.RendezVous)
            .Select(d => new DossierMedicalViewModel
            {
                DossierMedicalId = d.DossierMedicalId,
                PatientName = d.Patient.Nom + " " + d.Patient.Prenom,
                AntecedentsMedicaux = d.AntecedentsMedicaux,
                ConsultationCount = d.Consultations.Count,
                ExamenCount = d.Examen.Count,
                RendezVousCount = d.RendezVous.Count
            })
            .ToListAsync();

        return View(dossierMedicals);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while retrieving dossier medicals");
        return View("Error");
    }
}

        // GET: DossierMedicals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var dossierMedical = await _context.DossierMedicals
                    .Include(d => d.Patient)
                    .Include(d => d.Consultations)
                        .ThenInclude(c => c.RendezVous)
                            .ThenInclude(r => r.Medecin)
                    .Include(d => d.Examen)
                    .Include(d => d.RendezVous)
                    .FirstOrDefaultAsync(m => m.DossierMedicalId == id);

                if (dossierMedical == null)
                {
                    return NotFound();
                }

                return View(dossierMedical);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving dossier medical details for ID: {id}");
                return View("Error");
            }
        }

        // GET: DossierMedicals/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "FullName");
            return View();
        }

        // POST: DossierMedicals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DossierMedicalId,PatientId,AntecedentsMedicaux")] DossierMedical dossierMedical)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(dossierMedical);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating a new dossier medical");
                    ModelState.AddModelError("", "An error occurred while saving the dossier medical. Please try again.");
                }
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "FullName", dossierMedical.PatientId);
            return View(dossierMedical);
        }

        // GET: DossierMedicals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dossierMedical = await _context.DossierMedicals.FindAsync(id);
            if (dossierMedical == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "FullName", dossierMedical.PatientId);
            return View(dossierMedical);
        }

        // POST: DossierMedicals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DossierMedicalId,PatientId,AntecedentsMedicaux")] DossierMedical dossierMedical)
        {
            if (id != dossierMedical.DossierMedicalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dossierMedical);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!DossierMedicalExists(dossierMedical.DossierMedicalId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, $"A concurrency error occurred while updating dossier medical ID: {id}");
                        ModelState.AddModelError("", "The record you attempted to edit was modified by another user. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while updating dossier medical ID: {id}");
                    ModelState.AddModelError("", "An error occurred while updating the dossier medical. Please try again.");
                }
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "FullName", dossierMedical.PatientId);
            return View(dossierMedical);
        }

        // GET: DossierMedicals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dossierMedical = await _context.DossierMedicals
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.DossierMedicalId == id);
            if (dossierMedical == null)
            {
                return NotFound();
            }

            return View(dossierMedical);
        }

        // POST: DossierMedicals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var dossierMedical = await _context.DossierMedicals.FindAsync(id);
                if (dossierMedical != null)
                {
                    _context.DossierMedicals.Remove(dossierMedical);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting dossier medical ID: {id}");
                return View("Error");
            }
        }

        // Action to export the dossier to PDF
        [HttpGet]
        public async Task<IActionResult> ExportToPdf(int id)
        {
            try
            {
                QuestPDF.Settings.License = LicenseType.Community;
                var dossierMedical = await _context.DossierMedicals
                    .Include(d => d.Patient)
                    .Include(d => d.Consultations)
                        .ThenInclude(c => c.RendezVous)
                            .ThenInclude(r => r.Medecin)
                    .Include(d => d.Examen)
                    .Include(d => d.RendezVous)
                    .FirstOrDefaultAsync(m => m.DossierMedicalId == id);

                if (dossierMedical == null)
                {
                    return NotFound();
                }
                
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        
                        page.Content().Element(content => ComposeContent(content, dossierMedical));
                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                    });
                });

                byte[] pdfBytes = document.GeneratePdf();
                return File(pdfBytes, "application/pdf", $"DossierMedical_{id}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while generating PDF for dossier medical ID: {id}");
                return View("Error");
            }
        }

        
       private void ComposeContent(IContainer container, DossierMedical dossierMedical)
{
    container.PaddingVertical(30).Column(column =>
    {
        column.Spacing(25);

        // Header with Logo and Title
        column.Item().Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("MediPortal")
                    .FontSize(24)
                    .FontColor(Colors.Blue.Darken3)
                    .Bold();
                col.Item().Text("Dossier Médical")
                    .FontSize(16)
                    .FontColor(Colors.Grey.Darken2);
            });
            
            row.RelativeItem().AlignRight().Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                .FontSize(10)
                .FontColor(Colors.Grey.Medium);
        });

        // Divider
        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

        // Patient Information Section
        column.Item().Background(Colors.Blue.Lighten5).Padding(20).Column(patientInfo =>
        {
            patientInfo.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Informations du Patient")
                        .FontSize(18)
                        .Bold()
                        .FontColor(Colors.Blue.Darken3);
                });
            });

            patientInfo.Spacing(12);

            var infoStyle = TextStyle.Default
                .FontSize(11)
                .FontColor(Colors.Grey.Darken2);

            patientInfo.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Nom Complet")
                        .Bold()
                        .FontColor(Colors.Grey.Darken3);
                    col.Item().Text($"{dossierMedical.Patient.Nom} {dossierMedical.Patient.Prenom}")
                        .Style(infoStyle);
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Date de Naissance")
                        .Bold()
                        .FontColor(Colors.Grey.Darken3);
                    col.Item().Text(dossierMedical.Patient.DateNaissance?.ToString("dd/MM/yyyy") ?? "N/A")
                        .Style(infoStyle);
                });
            });

            patientInfo.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Adresse")
                        .Bold()
                        .FontColor(Colors.Grey.Darken3);
                    col.Item().Text(dossierMedical.Patient.Adresse)
                        .Style(infoStyle);
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Téléphone")
                        .Bold()
                        .FontColor(Colors.Grey.Darken3);
                    col.Item().Text(dossierMedical.Patient.Telephone)
                        .Style(infoStyle);
                });
            });
        });

        // Antécédents Médicaux Section
        column.Item().Column(antecedents =>
        {
            antecedents.Item().Background(Colors.Green.Lighten5).Padding(20).Column(content =>
            {
                content.Item().Text("Antécédents Médicaux")
                    .FontSize(18)
                    .Bold()
                    .FontColor(Colors.Green.Darken3);

                content.Item().PaddingTop(10)
                    .Text(dossierMedical.AntecedentsMedicaux)
                    .FontSize(11)
                    .FontColor(Colors.Grey.Darken2);
            });
        });

        // Consultations Section
        if (dossierMedical.Consultations.Any())
        {
            column.Item().Column(consultations =>
            {
                consultations.Item().Text("Historique des Consultations")
                    .FontSize(18)
                    .Bold()
                    .FontColor(Colors.Orange.Darken3);

                consultations.Item().Table(table =>
                {
                    // Table styling
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(80);  // Date
                        columns.ConstantColumn(100); // Heure
                        columns.RelativeColumn();    // Médecin
                        columns.RelativeColumn(2);   // Notes
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyleHeader).Text("Date");
                        header.Cell().Element(CellStyleHeader).Text("Heure");
                        header.Cell().Element(CellStyleHeader).Text("Médecin");
                        header.Cell().Element(CellStyleHeader).Text("Notes");
                    });

                    foreach (var consultation in dossierMedical.Consultations.OrderByDescending(c => c.RendezVous.DateHeure))
                    {
                        table.Cell().Element(CellStyle)
                            .Text(consultation.RendezVous.DateHeure.ToString("dd/MM/yyyy"));
                        table.Cell().Element(CellStyle)
                            .Text(consultation.RendezVous.DateHeure.ToString("HH:mm"));
                        table.Cell().Element(CellStyle)
                            .Text($"Dr. {consultation.RendezVous.Medecin.Nom} {consultation.RendezVous.Medecin.Prenom}");
                        table.Cell().Element(CellStyle)
                            .Text(consultation.Notes);
                    }
                });
            });
        }

        // Examens Section
        if (dossierMedical.Examen.Any())
        {
            column.Item().Column(examens =>
            {
                examens.Item().Text("Examens Médicaux")
                    .FontSize(18)
                    .Bold()
                    .FontColor(Colors.Purple.Darken3);

                examens.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(100); // Date
                        columns.RelativeColumn();    // Type
                        columns.RelativeColumn(2);   // Résultats
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyleHeader).Text("Date");
                        header.Cell().Element(CellStyleHeader).Text("Type d'Examen");
                        header.Cell().Element(CellStyleHeader).Text("Résultats");
                    });

                    foreach (var examen in dossierMedical.Examen.OrderByDescending(e => e.DateExamen))
                    {
                        table.Cell().Element(CellStyle)
                            .Text(examen.DateExamen?.ToString("dd/MM/yyyy") ?? "N/A");
                        table.Cell().Element(CellStyle)
                            .Text(examen.TypeExamen);
                        table.Cell().Element(CellStyle)
                            .Text(examen.Resultats);
                    }
                });
            });
        }

        // Footer
        column.Item().PaddingTop(20).Row(footer =>
        {
            footer.RelativeItem().Column(col =>
            {
                col.Item().Text("Document généré par MediPortal")
                    .FontSize(8)
                    .FontColor(Colors.Grey.Medium);
                col.Item().Text($"Date d'impression: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .FontSize(8)
                    .FontColor(Colors.Grey.Medium);
            });

            footer.RelativeItem().AlignRight().Text("Page ")
                .FontSize(8)
                .FontColor(Colors.Grey.Medium);
        });
    });
}

private static IContainer CellStyle(IContainer container)
{
    return container
        .Border(0.5f)
        .BorderColor(Colors.Grey.Lighten3)
        .Background(Colors.White)
        .Padding(8)
        .DefaultTextStyle(x => x
            .FontSize(10)
            .FontColor(Colors.Grey.Darken2)
            .LineHeight(1.2f));
}

private static IContainer CellStyleHeader(IContainer container)
{
    return container
        .Border(0.5f)
        .BorderColor(Colors.Grey.Lighten3)
        .Background(Colors.Blue.Lighten4)
        .Padding(8)
        .DefaultTextStyle(x => x
            .FontSize(10)
            .Bold()
            .FontColor(Colors.Blue.Darken3)
            .LineHeight(1.2f));
}

        private bool DossierMedicalExists(int id)
        {
            return _context.DossierMedicals.Any(e => e.DossierMedicalId == id);
        }
    }
}