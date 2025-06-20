using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitaCore.Data;
using VitaCore.Models;
using VitaCore.Models.ViewModel;
using VitaCore.Services;

namespace VitaCore.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Send message
        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageModel model)
        {
            var user = WebSessionManager.GetLoggedInUser(HttpContext);
            if (user == null) return Unauthorized();

            var doc = await _context.Doctors
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.UserId == user.Id);

            int currentUserId = doc.id;

            model.sender_id = currentUserId;
            model.sent_at = DateTime.UtcNow;
            model.is_read = false;

            _context.Messages.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Inbox");
        }

        // List of conversations
        public async Task<IActionResult> Inbox()
        {
            var user = WebSessionManager.GetLoggedInUser(HttpContext);
            if (user == null) return Unauthorized();

            var doc = await _context.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == user.Id);

            int currentUserId = doc.id;
            ViewBag.CurrentUserId = currentUserId;

            var messages = await _context.Messages
                .Where(m => m.sender_id == currentUserId || m.receiver_id == currentUserId)
                .OrderByDescending(m => m.sent_at)
                .ToListAsync();

            var userIds = messages.Select(m => m.sender_id)
                                  .Concat(messages.Select(m => m.receiver_id))
                                  .Distinct()
                                  .ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            string GetUserName(int id)
            {
                var u = users.FirstOrDefault(u => u.Id == id);
                return u != null ? $"{u.FirstName} {u.LastName}" : "Unknown";
            }

            // Group messages by conversation partner (excluding self)
            var grouped = messages
                .GroupBy(m => m.sender_id == currentUserId ? m.receiver_id : m.sender_id)
                .Select(g => g.OrderByDescending(m => m.sent_at).First())
                .ToList();

            var messageViewModels = grouped.Select(m => new MessageViewModel
            {
                Id = m.id,
                Message = m.message,
                SentAt = m.sent_at,
                IsRead = m.is_read,
                SenderId = m.sender_id,
                ReceiverId = m.receiver_id,
                SenderName = GetUserName(m.sender_id),
                ReceiverName = GetUserName(m.receiver_id)
            }).ToList();

            return View(messageViewModels);
        }

        // Conversation thread with a specific user
        public async Task<IActionResult> Conversation(int withUserId)
        {
            var user = WebSessionManager.GetLoggedInUser(HttpContext);
            if (user == null) return Unauthorized();

            var doc = await _context.Doctors
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.UserId == user.Id);

            int currentUserId = doc.id;

            var messages = await _context.Messages
                .Where(m =>
                    (m.sender_id == currentUserId && m.receiver_id == withUserId) ||
                    (m.sender_id == withUserId && m.receiver_id == currentUserId))
                .OrderBy(m => m.sent_at)
                .ToListAsync();

            // Mark as read
            var unread = messages.Where(m => m.receiver_id == currentUserId && m.is_read == false);
            foreach (var msg in unread)
            {
                msg.is_read = true;
            }
            await _context.SaveChangesAsync();

            var withUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == withUserId);
            string withUserName = withUser != null ? $"{withUser.FirstName} {withUser.LastName}" : "Unknown";

            ViewBag.CurrentUserId = currentUserId;
            ViewBag.WithUserId = withUserId;
            ViewBag.WithUserName = withUserName;

            return View(messages);
        }
    }
}
