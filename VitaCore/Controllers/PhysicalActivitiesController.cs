using Microsoft.AspNetCore.Mvc;
using VitaCore.Data;
using VitaCore.Models;

namespace VitaCore.Controllers
{
    public class PhysicalActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhysicalActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int patientId)
        {
            return View(new PhysicalActivityModel { PatientId = patientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhysicalActivityModel model)
        {
            if (ModelState.IsValid)
            {
                model.StartTime = DateTime.SpecifyKind((DateTime)model.StartTime, DateTimeKind.Utc);
                model.EndTime = DateTime.SpecifyKind((DateTime)model.EndTime, DateTimeKind.Utc);
                _context.PhysicalActivities.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Details", "Patients", new { id = model.PatientId });
            }

            return View(model);
        }
    }
}
