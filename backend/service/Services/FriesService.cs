using infrastructure.Models;
using infrastructure.Repositories;
using service.Interfaces;

namespace service.Services;

public class FriesService : IFriesService
{
    private readonly FriesRepository _friesRepository;

    public FriesService(FriesRepository friesRepository)
    {
        _friesRepository = friesRepository ?? throw new ArgumentNullException(nameof(friesRepository), "FriesRepository is null");
    }

    public async Task<Fries> CreateFries(Fries fries)
    {
        return await _friesRepository.CreateFries(fries);
    }

    public async Task<IEnumerable<Fries>> GetAllFries()
    {
        return await Task.FromResult(_friesRepository.GetAllFries());
    }

    public async Task<Fries> GetFriesById(int id)
    {
        var fries = _friesRepository.GetFriesById(id);
        if (fries == null) 
        {
            throw new KeyNotFoundException("Fries not found");
        }
        return await Task.FromResult(fries);
    }

    public async Task<Fries> UpdateFries(int id, Fries friesUpdateInfo)
    {
        var updatedFries = _friesRepository.UpdateFries(id, friesUpdateInfo);
        if (updatedFries == null) 
        {
            throw new KeyNotFoundException("Fries not found");
        }
        return await Task.FromResult(updatedFries);
    }

    public async Task<bool> DeleteFries(int id)
    {
        try
        {
            return await Task.Run(() => _friesRepository.DeleteFries(id));
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Could not delete the fries");
        }
    }
}