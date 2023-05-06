using Pronia.Models;
using System.Composition;

namespace Pronia.Services.Interfaces
{
    public interface ISocialService
    {
        Task<List<Social>> GetAllSocials();
    }
}
