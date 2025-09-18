using EFModel;
using EFModel.Models;
using Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class MachineVirtuelleService
    {
        private readonly EClassRoomDbContext _db;

        public MachineVirtuelleService(EClassRoomDbContext db)
        {
            _db = db;
        }

        public async Task<List<MachineVirtuelleDto>> GetAllAsync()
        {
            return await _db.MachinesVirtuelles
                .Select(vm => new MachineVirtuelleDto
                {
                    Id = vm.Id,
                    Nom = vm.Nom,
                    TypeOs = vm.TypeOs,
                    TypeVm = vm.TypeVm,
                    Sku = vm.Sku,
                    Offer = vm.Offer,
                    Version = vm.Version,
                    DiskIso = vm.DiskIso,
                    NomMarketing = vm.NomMarketing
                })
                .ToListAsync();
        }

        public async Task<MachineVirtuelleDto> GetByIdAsync(int id)
        {
            var vm = await _db.MachinesVirtuelles.FindAsync(id);
            if (vm == null) return null;
            return new MachineVirtuelleDto
            {
                Id = vm.Id,
                Nom = vm.Nom,
                TypeOs = vm.TypeOs,
                TypeVm = vm.TypeVm,
                Sku = vm.Sku,
                Offer = vm.Offer,
                Version = vm.Version,
                DiskIso = vm.DiskIso,
                NomMarketing = vm.NomMarketing
            };
        }

        public async Task AddAsync(MachineVirtuelleDto dto)
        {
            var vm = new MachineVirtuelle
            {
                Nom = dto.Nom,
                TypeOs = dto.TypeOs,
                TypeVm = dto.TypeVm,
                Sku = dto.Sku,
                Offer = dto.Offer,
                Version = dto.Version,
                DiskIso = dto.DiskIso,
                NomMarketing = dto.NomMarketing
            };
            _db.MachinesVirtuelles.Add(vm);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, MachineVirtuelleDto dto)
        {
            var vm = await _db.MachinesVirtuelles.FindAsync(id);
            if (vm == null) return;
            vm.Nom = dto.Nom;
            vm.TypeOs = dto.TypeOs;
            vm.TypeVm = dto.TypeVm;
            vm.Sku = dto.Sku;
            vm.Offer = dto.Offer;
            vm.Version = dto.Version;
            vm.DiskIso = dto.DiskIso;
            vm.NomMarketing = dto.NomMarketing;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vm = await _db.MachinesVirtuelles.FindAsync(id);
            if (vm == null) return;
            _db.MachinesVirtuelles.Remove(vm);
            await _db.SaveChangesAsync();
        }
    }
}
