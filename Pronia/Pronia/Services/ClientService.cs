﻿using Microsoft.EntityFrameworkCore;
using Pronia.Data;
using Pronia.Models;
using Pronia.Services.Interfaces;

namespace Pronia.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Client> GetClientById(int? id) =>  await _context.Clients.Where(m => !m.SofDelete).FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<Client>> GetClients() => await _context.Clients.ToListAsync();

    }
}
