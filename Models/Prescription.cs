using System;
using System.Collections.Generic;

namespace HOPITAL2.Models;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int? ConsultationId { get; set; }

    public string Medicament { get; set; } = null!;

    public string? Posologie { get; set; }

    public virtual Consultation? Consultation { get; set; }
}
