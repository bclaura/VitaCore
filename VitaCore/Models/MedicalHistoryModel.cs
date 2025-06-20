using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitaCore.Models
{
    [Table("medical_history")]
    public class MedicalHistoryModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [ValidateNever]
        [ForeignKey("PatientId")]
        public PatientModel Patient { get; set; }

        [Column("history")]
        public string? History { get; set; }

        [Column("allergies")]
        public string? Allergies { get; set; }

        [Column("cardiology_consultations")]
        public string? CardiologyConsultations { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    public class MedicalHistoryDto
    {
        public int PatientId { get; set; }
        public string? History { get; set; }
        public string? Allergies { get; set; }
        public string? CardiologyConsultations { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}