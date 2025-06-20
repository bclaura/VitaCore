using System.Text.Json;
using VitaCore.Models;

namespace VitaCore.Services
{
    public static class WebSessionManager
    {
        private const string SessionKey = "LoggedInUser";

        public static void SetLoggedInUser(HttpContext context, User user)
        {
            var json = JsonSerializer.Serialize(user);
            context.Session.SetString(SessionKey, json);
        }

        public static User? GetLoggedInUser(HttpContext context)
        {
            var json = context.Session.GetString(SessionKey);
            return json != null ? JsonSerializer.Deserialize<User>(json) : null;
        }

        public static void Logout(HttpContext context)
        {
            context.Session.Remove(SessionKey);
        }

        public static bool IsLoggedIn(HttpContext context)
        {
            return GetLoggedInUser(context) != null;
        }
    }
}
