
using Pronia.Models;

namespace Pronia.Services.Interfaces
{
    public interface IColorService
    {
        Task<List<Color>> GetColors();

        Task<Color> GetColorById(int? id);
    }
}
