using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFModel;

namespace Server.Services
{
    public class AuthService
    {
        private readonly EClassRoomDbContext _db;

        public AuthService(EClassRoomDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            // À adapter pour le hashage de mot de passe en production !
            return await _db.Utilisateurs.AnyAsync(u => u.Email == email && u.MotDePasse == password);
        }
    }
}