using System;
using System.Collections.Generic;

namespace HOPITAL2.Models;

public partial class Medecin
{
    public int MedecinId { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string? Specialite { get; set; }

    public string NomPrenom => $"{Nom} {Prenom}";

    public virtual ICollection<RendezVou> RendezVous { get; set; } = new List<RendezVou>();
}
