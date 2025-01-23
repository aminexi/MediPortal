using System;
using System.Collections.Generic;

namespace HOPITAL2.Models;

public partial class Consultation
{
    public int ConsultationId { get; set; }

    public int? DossierMedicalId { get; set; }

    public int? RendezVousId { get; set; }

    public string? Notes { get; set; }

    public DateTime DateConsultation { get; set; }

    public decimal Prix { get; set; }

    public virtual DossierMedical? DossierMedical { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual RendezVou? RendezVous { get; set; }

}
