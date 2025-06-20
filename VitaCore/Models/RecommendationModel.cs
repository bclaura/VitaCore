using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitaCore.Models
{
    [Table("recommendations")]
    public class RecommendationModel
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

        [Required]
        [Column("recommendation_type")]
        public string RecommendationType { get; set; }

        [Required]
        [Column("daily_duration")]
        public int? DailyDuration { get; set; }

        [Required]
        [Column("additional_instructions")]
        public string? AdditionalInstructions { get; set; }
    }

    public class RecommendationDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string RecommendationType { get; set; }
        public int? DailyDuration { get; set; }
        public string? AdditionalInstructions { get; set; }
    }

}