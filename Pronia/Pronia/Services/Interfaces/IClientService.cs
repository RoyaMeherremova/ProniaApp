﻿using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IClientService
    {
        Task<List<Client>> GetClients();
        
        Task<Client> GetClientById(int? id); 
    }
}
