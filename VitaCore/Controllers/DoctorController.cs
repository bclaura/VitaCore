// DoctorsController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;
using VitaCore.Data;
using VitaCore.Models;
using VitaCore.Models.ViewModel;
using VitaCore.Services;

namespace VitaCore.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = WebSessionManager.GetLoggedInUser(HttpContext);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == user.Id);

            if (doctor == null)
                return NotFound("Doctor profile not found.");

            var today = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
            var tomorrow = today.AddDays(1);

            //implement maybe after
            /*var todaysAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.id && a.StartTime >= today && a.StartTime < tomorrow)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .OrderBy(a => a.StartTime)
                .Select(a => new AppointmentViewModel
                {
                    Id = a.Id,
                    PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
                    StartTime = DateTime.SpecifyKind(a.StartTime, DateTimeKind.Utc),
                    EndTime = a.EndTime.HasValue
                                ? DateTime.SpecifyKind(a.EndTime.Value, DateTimeKind.Utc)
                                : DateTime.SpecifyKind(a.StartTime.AddMinutes(30), DateTimeKind.Utc), // Fallback if EndTime is null
                    Notes = a.Notes
                })
                .ToListAsync();
            */
            /*
            var patientList = await _context.Appointments
                .Where(a => a.DoctorId == doctor.id)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .GroupBy(a => a.PatientId)
                .Select(g => new PatientViewModel
                {
                    Id = g.Key,
                    FullName = g.First().Patient.User.FirstName + " " + g.First().Patient.User.LastName,
                    LastVisit = g.Max(a => a.StartTime),
                    Status = "Active" // You can customize this based on logic
                })
                .ToListAsync();
            */


            var patientList = await _context.Patients
                .Include(p => p.User)
                .Select(p => new PatientViewModel
                {
                    Id = p.id,
                    FullName = p.User.FirstName + " " + p.User.LastName,
                    LastVisit = DateTime.UtcNow, // sau un DateTime fictiv dacă vrei
                    Status = "Active" // sau altceva static pentru acum
                })
                .ToListAsync();


            var startOfWeek = DateTime.SpecifyKind(today.AddDays(-(int)today.DayOfWeek), DateTimeKind.Utc);
            var endOfWeek = startOfWeek.AddDays(7);

            /*var weeklyAppointments = await _context.Appointments
                .Where(a => a.DoctorId == doctor.id && a.StartTime >= startOfWeek && a.StartTime < endOfWeek)
                .CountAsync();*/

            var model = new DoctorDashboardViewModel
            {
                FullName = "Dr. " + doctor.User.FirstName + " " + doctor.User.LastName,
                //TodaysAppointments = todaysAppointments,
                TodaysAppointments = null,
                Patients = patientList,
                Stats = new DashboardStats
                {
                    //WeeklyAppointments = weeklyAppointments,
                    WeeklyAppointments = 0,
                    TotalPatients = patientList.Count,
                    UnreadMessages = 0 // Add logic if you implement messaging
                }
            };

            return View(model);
        }

        // Create - GET
        public IActionResult Create()
        {
            return View();
        }
        /*
        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorModel model)
        {
            ModelState.Remove("User");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // The user is not logged in
            }
            var newDoc = new DoctorModel()
                {
                    UserId = userId, // Set from the logged-in user, not from the model
                    honorific_title = model.honorific_title,
                    gender = model.gender,
                    bio = model.bio,
                    availability_hours = model.availability_hours,
                    clinic_address = model.clinic_address,
                    Specialization = model.Specialization,
                    is_favorite = model.is_favorite
                };

                await _context.Doctors.AddAsync(newDoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            return View(model);
        }
        */

        // EDIT - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _context.Doctors
                            .Include(d => d.User)
                            .FirstOrDefaultAsync(d => d.id == id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }


        //Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var doctor = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.id == id);
            if (doctor == null)
                return NotFound();

            doctor.Specialization = model.Specialization;
            doctor.clinic_address = model.clinic_address;
            doctor.honorific_title = model.honorific_title;
            doctor.bio = model.bio;
            doctor.gender = model.gender;
            doctor.availability_hours = model.availability_hours;
            doctor.profile_picture_base64 = model.profile_picture_base64;
            doctor.is_favorite = false; //temp

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id = doctor.id });
        }

        //DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var doctors = await _context.Doctors.FindAsync(id);

                var user = await _context.Users.FindAsync(doctors.UserId);

                if (doctors == null)
                {
                    return NotFound();
                }
               
                _context.Doctors.Remove(doctors);

                _context.Users.Remove(user);


                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}