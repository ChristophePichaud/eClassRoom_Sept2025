using EFModel;
using EFModel.Models;
using Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class UtilisateurService
    {
        private readonly EClassRoomDbContext _db;

        public UtilisateurService(EClassRoomDbContext db)
        {
            _db = db;
        }

        public async Task<List<UtilisateurDto>> GetAllAsync()
        {
            return await _db.Utilisateurs
                .Select(u => new UtilisateurDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Nom = u.Nom,
                    Prenom = u.Prenom,
                    MotDePasse = u.MotDePasse,
                    Role = u.Role,
                    ClientId = u.ClientId
                })
                .ToListAsync();
        }

        public async Task<UtilisateurDto> GetByIdAsync(int id)
        {
            var u = await _db.Utilisateurs.FindAsync(id);
            if (u == null) return null;
            return new UtilisateurDto
            {
                Id = u.Id,
                Email = u.Email,
                Nom = u.Nom,
                Prenom = u.Prenom,
                MotDePasse = u.MotDePasse,
                Role = u.Role,
                ClientId = u.ClientId
            };
        }

        public async Task AddAsync(UtilisateurDto dto)
        {
            var u = new Utilisateur
            {
                Email = dto.Email,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                MotDePasse = dto.MotDePasse,
                Role = dto.Role,
                ClientId = dto.ClientId
            };
            _db.Utilisateurs.Add(u);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UtilisateurDto dto)
        {
            var u = await _db.Utilisateurs.FindAsync(id);
            if (u == null) return;
            u.Email = dto.Email;
            u.Nom = dto.Nom;
            u.Prenom = dto.Prenom;
            u.MotDePasse = dto.MotDePasse;
            u.Role = dto.Role;
            u.ClientId = dto.ClientId;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var u = await _db.Utilisateurs.FindAsync(id);
            if (u == null) return;
            _db.Utilisateurs.Remove(u);
            await _db.SaveChangesAsync();
        }
    }
}
