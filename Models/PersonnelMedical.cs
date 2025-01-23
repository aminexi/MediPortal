using System.ComponentModel.DataAnnotations;

namespace HOPITAL2.Models;
// Replace 'YourNamespace' with your actual project namespace

    public class PersonnelMedical
    {
        [Key]  // Specifies that this is the primary key
        public int PersonnelID { get; set; }

        [Required]  // Specifies that this field is required
        [MaxLength(100)]  // Optional: Limits the length of the string to 100 characters
        public string Nom { get; set; }

        [Required]
        [MaxLength(100)]
        public string Prenom { get; set; }

        [MaxLength(100)]  // Optional: Limits the length of the string to 100 characters
        public string Fonction { get; set; }
    }

