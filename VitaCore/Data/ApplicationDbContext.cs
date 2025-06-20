using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using VitaCore.Models;
using System.Reflection.Emit;

namespace VitaCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<AlarmModel> Alarms { get; set; }
        public DbSet<ChartDataModel> ChartData { get; set; }
        public DbSet<DoctorModel> Doctors { get; set; }
        public DbSet<EcgSignalModel> EcgSignal { get; set; }
        public DbSet<LocationMapModel> LocationMap { get; set; }
        public DbSet<MedicalHistoryModel> MedicalHistory { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<PatientModel> Patients { get; set; }
        public DbSet<PhysicalActivityModel> PhysicalActivities { get; set; }
        public DbSet<RecommendationModel> Recommendation { get; set; }
        public DbSet<SensorDataModel> SensorData { get; set; }
        public DbSet<UserFavoritesModel> UserFavorites { get; set; }
        public DbSet<User> Users { get; set; }


        //public DbSet<UserFavorite> UserFavorites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AlarmModel>().ToTable("alarms");
            builder.Entity<ChartDataModel>().ToTable("chart_data");
            builder.Entity<DoctorModel>().ToTable("doctors");
            builder.Entity<EcgSignalModel>().ToTable("ecg_signals");
            builder.Entity<LocationMapModel>().ToTable("location_map");
            builder.Entity<MedicalHistoryModel>().ToTable("medical_history");
            builder.Entity<MessageModel>().ToTable("messages");
            builder.Entity<PatientModel>().ToTable("patients");
            builder.Entity<PhysicalActivityModel>().ToTable("physical_activities");
            builder.Entity<RecommendationModel>().ToTable("recommendations");
            builder.Entity<SensorDataModel>().ToTable("sensor_data");
            builder.Entity<UserFavoritesModel>().ToTable("user_favorites");
            builder.Entity<User>().ToTable("users");
            builder.Entity<AppointmentModel>().ToTable("appointments");


        }



    }
}
