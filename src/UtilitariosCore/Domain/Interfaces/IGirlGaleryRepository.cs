using UtilitariosCore.Application.Features.GirlGaleries.Dtos;
using UtilitariosCore.Domain.Models;

namespace UtilitariosCore.Domain.Interfaces;

public interface IGirlGaleryRepository
{
    Task<int> CreateGirlGalery(GirlGalery item);
    Task<bool> UpdateGirlGalery(GirlGalery item);
    Task<bool> DeleteGirlGalery(int id);
    Task<GirlGalery?> GetGirlGaleryById(int id);
    Task<IEnumerable<GirlGalery>> GetAllGirlGaleries();
    Task<IEnumerable<GirlGaleryDto>> GetAllGirlGaleriesWithFirstImage();
}
