using infrastructure.Models;

namespace service.Interfaces;

public interface IFriesService
{
    Task<Fries> CreateFries(Fries fries);
    Task<IEnumerable<Fries>> GetAllFries();
    Task<Fries> GetFriesById(int id);
    Task<Fries> UpdateFries(int id, Fries fries);
    Task<bool> DeleteFries(int id);
}