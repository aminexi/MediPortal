using System;
using System.Collections.Generic;

namespace HOPITAL2.Models;

public partial class Examan
{
    public int ExamenId { get; set; }

    public int? DossierMedicalId { get; set; }

    public string TypeExamen { get; set; } = null!;

    public DateOnly? DateExamen { get; set; }

    public string? Resultats { get; set; }

    public virtual DossierMedical? DossierMedical { get; set; }
}
