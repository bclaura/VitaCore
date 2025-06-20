using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitaCore.Data;

namespace VitaCore.Controllers
{
    [Route("EcgSignal")]
    public class EcgSignalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EcgSignalController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("History/{patientId}")]
        public async Task<IActionResult> History(int patientId)
        {
            var signals = await _context.EcgSignal
                    .Where(e => e.PatientId == patientId)
                    .OrderByDescending(e => e.Timestamp)
                    .ToListAsync();

            ViewBag.PatientId = patientId;

            return View(signals);
        }

        // GET: /EcgSignal/Summaries/{patientId}
        [HttpGet("Summaries/{patientId}")]
        public async Task<IActionResult> GetSummaries(int patientId)
        {
            var summaries = await _context.EcgSignal
                .Where(e => e.PatientId == patientId)
                .OrderByDescending(e => e.Timestamp)
                .Select(e => new
                {
                    e.Id,
                    e.Timestamp
                })
                .ToListAsync();

            ViewBag.PatientId = patientId;

            return Ok(summaries);
        }

        // GET: /EcgSignal/GetById/{id}
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var signal = await _context.EcgSignal.FindAsync(id);
            if (signal == null)
                return NotFound();

            return Ok(new
            {
                signal.Id,
                signal.Timestamp,
                signal.Signal
            });
        }

        [HttpGet("GetSignalRaw/{id}")]
        public async Task<IActionResult> GetSignalRaw(int id)
        {
            var signal = await _context.EcgSignal.FindAsync(id);
            if (signal == null)
                return NotFound();

            return Json(signal.Signal);
        }
    }

}
