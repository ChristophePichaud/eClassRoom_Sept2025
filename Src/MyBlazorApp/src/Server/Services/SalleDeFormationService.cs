using EFModel;
using EFModel.Models;
using Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class SalleDeFormationService
    {
        private readonly EClassRoomDbContext _db;

        public SalleDeFormationService(EClassRoomDbContext db)
        {
            _db = db;
        }

        public async Task<List<SalleDeFormationDto>> GetAllAsync()
        {
            return await _db.SallesDeFormation
                .Select(s => new SalleDeFormationDto
                {
                    Id = s.Id,
                    Nom = s.Nom,
                    Formateur = s.Formateur,
                    DateDebut = s.DateDebut,
                    DateFin = s.DateFin
                })
                .ToListAsync();
        }

        public async Task<SalleDeFormationDto> GetByIdAsync(int id)
        {
            var s = await _db.SallesDeFormation.FindAsync(id);
            if (s == null) return null;
            return new SalleDeFormationDto
            {
                Id = s.Id,
                Nom = s.Nom,
                Formateur = s.Formateur,
                DateDebut = s.DateDebut,
                DateFin = s.DateFin
            };
        }

        public async Task AddAsync(SalleDeFormationDto dto)
        {
            var s = new SalleDeFormation
            {
                Nom = dto.Nom,
                Formateur = dto.Formateur,
                DateDebut = dto.DateDebut,
                DateFin = dto.DateFin
            };
            _db.SallesDeFormation.Add(s);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, SalleDeFormationDto dto)
        {
            var s = await _db.SallesDeFormation.FindAsync(id);
            if (s == null) return;
            s.Nom = dto.Nom;
            s.Formateur = dto.Formateur;
            s.DateDebut = dto.DateDebut;
            s.DateFin = dto.DateFin;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var s = await _db.SallesDeFormation.FindAsync(id);
            if (s == null) return;
            _db.SallesDeFormation.Remove(s);
            await _db.SaveChangesAsync();
        }
    }
}
