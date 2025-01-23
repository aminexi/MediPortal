namespace HOPITAL2.Controllers
{
   public class DossierMedicalViewModel
    {
        public int DossierMedicalId { get; set; }
        public string PatientName { get; set; }
        public string AntecedentsMedicaux { get; set; }
        public int ConsultationCount { get; set; }
        public int ExamenCount { get; set; }
        public int RendezVousCount { get; set; }
    }
}