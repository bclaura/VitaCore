using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VitaCore.Data;
using VitaCore.Models;

namespace VitaCore.Controllers
{
    
    public class MedicalHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;


        public MedicalHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }


        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateMedicalHistory(MedicalHistoryModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.MedicalHistory
                    .FirstOrDefault(m => m.PatientId == model.PatientId);

                if (existing != null)
                {
                    existing.History = model.History;
                    existing.Allergies = model.Allergies;
                    existing.CardiologyConsultations = model.CardiologyConsultations;
                    _context.Update(existing);
                }
                else
                {
                    _context.Add(model);
                }

                _context.SaveChanges();
            }

      
            return RedirectToAction("Index", "Patients", new { id = model.PatientId });
        }
        */

        /*
        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int patientId)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.id == patientId);
            if (patient == null)
                return NotFound();

            var medicalHistory = _context.MedicalHistory.FirstOrDefault(m => m.PatientId == patientId);

            var model = medicalHistory ?? new MedicalHistoryModel
            {
                PatientId = patientId
            };

            return View("EditMedicalHistory", model);
        }
        */

        [HttpGet]
        public IActionResult Edit(int patientId)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.id == patientId);
            if (patient == null)
                return NotFound();

            var medicalHistory = _context.MedicalHistory.FirstOrDefault(m => m.PatientId == patientId);

            var model = medicalHistory ?? new MedicalHistoryModel
            {
                PatientId = patientId
            };

            return View("EditMedicalHistory", model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MedicalHistoryModel model)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.MedicalHistory
                    .FirstOrDefault(m => m.PatientId == model.PatientId);

                if (existing != null)
                {
                    existing.History = model.History;
                    existing.Allergies = model.Allergies;
                    existing.CardiologyConsultations = model.CardiologyConsultations;
                    existing.CreatedAt = DateTime.UtcNow.ToUniversalTime();
                    _context.Update(existing);
                }
                else
                {
                    model.CreatedAt = DateTime.UtcNow.ToUniversalTime();
                    _context.Add(model);
                }

                _context.SaveChanges();

                return RedirectToAction("Details", "Patients", new { id = model.PatientId });
            }

            return View("EditMedicalHistory", model);
        }




    }

}
