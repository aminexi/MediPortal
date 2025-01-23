using System;
using System.Collections.Generic;

namespace HOPITAL2.Models;

public partial class DossierMedical
{
    public int DossierMedicalId { get; set; }

    public int? PatientId { get; set; }

    public string? AntecedentsMedicaux { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<Examan> Examen { get; set; } = new List<Examan>();

    public virtual Patient? Patient { get; set; }

    public virtual ICollection<RendezVou> RendezVous { get; set; } = new List<RendezVou>();
}
