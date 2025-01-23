using System;
using System.Collections.Generic;

namespace HOPITAL2.Models;

public partial class RendezVou
{
    public int RendezVousId { get; set; }

    public int? DossierMedicalId { get; set; }

    public int? MedecinId { get; set; }

    public DateTime DateHeure { get; set; }

    public string? Statut { get; set; }

    public string Motif { get; set; } = null!;

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual DossierMedical? DossierMedical { get; set; }

    public virtual Medecin? Medecin { get; set; }
}
