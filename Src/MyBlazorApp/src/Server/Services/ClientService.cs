using EFModel;
using EFModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class ClientService
    {
        private readonly EClassRoomDbContext _db;

        public ClientService(EClassRoomDbContext db)
        {
            _db = db;
        }

        public async Task<List<Client>> GetClientsAsync() =>
            await _db.Clients.ToListAsync();

        public async Task AddClientAsync(Client client)
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateClientAsync(int id, Client client)
        {
            var existing = await _db.Clients.FindAsync(id);
            if (existing != null)
            {
                existing.NomSociete = client.NomSociete;
                existing.Adresse = client.Adresse;
                existing.EmailAdministrateur = client.EmailAdministrateur;
                existing.MotDePasseAdministrateur = client.MotDePasseAdministrateur;
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client != null)
            {
                _db.Clients.Remove(client);
                await _db.SaveChangesAsync();
            }
        }
    }
}