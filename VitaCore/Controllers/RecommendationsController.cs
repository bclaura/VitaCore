using Microsoft.AspNetCore.Mvc;
using VitaCore.Data;
using VitaCore.Models;

namespace VitaCore.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecommendationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int patientId)
        {
            return View(new RecommendationModel { PatientId = patientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RecommendationModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Recommendation.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Details", "Patients", new { id = model.PatientId });
            }

            return View(model);
        }
    }
}