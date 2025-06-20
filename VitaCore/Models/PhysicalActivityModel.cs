using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VitaCore.Models
{
    [Table("physical_activities")]
    public class PhysicalActivityModel
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
        [Column("activity_type")]
        public string ActivityType { get; set; }

        [Required]
        [Column("start_time")]
        public DateTime? StartTime  { get; set; }

        [Required]
        [Column("end_time")]
        public DateTime? EndTime { get; set; }

        [Required]
        [Column("duration")]
        public int? Duration { get; set; }
    }
}