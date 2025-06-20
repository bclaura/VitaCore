using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitaCore.Data;
using VitaCore.Models;
using VitaCore.Services;

namespace VitaCore.Models.ViewComponents
{
    public class InboxNavViewComponent : ViewComponent
    {
        
        private readonly ApplicationDbContext _context;

        public InboxNavViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = WebSessionManager.GetLoggedInUser(HttpContext);
            if (user == null) return View(0);

            var doctor = _context.Doctors.FirstOrDefault(d => d.UserId == user.Id);
            var patient = _context.Patients.FirstOrDefault(p => p.UserId == user.Id);
            int userId = doctor?.id ?? patient?.id ?? 0;

            int unreadCount = _context.Messages.Count(m => m.receiver_id == userId && m.is_read == false);
            return View(unreadCount);
        }
        */
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = WebSessionManager.GetLoggedInUser(HttpContext);
            if (user == null) return Content(string.Empty);

            //Afișează doar pentru doctori
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == user.Id);
            if (doctor == null) return Content(string.Empty); // Nu e doctor → nimic

            int userId = doctor.id;

            int unreadCount = await _context.Messages
                .CountAsync(m => m.receiver_id == userId && m.is_read == false);

            return View(unreadCount);
        }
        
        
    }
}