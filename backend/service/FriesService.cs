using infrastructure;
using api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace service;

public class FriesService : IFriesService
{
    private readonly FriesRepository _friesRepository;

    public FriesService(FriesRepository friesRepository)
    {
        _friesRepository = friesRepository ?? throw new ArgumentNullException(nameof(friesRepository), "FriesRepository is null");
    }

    public async Task<Fries> CreateFries(Fries fries)
    {
        // Add validation logic here as needed.
        return await _friesRepository.CreateFries(fries);
    }

    public async Task<IEnumerable<Fries>> GetAllFries()
    {
        // You could wrap this in Task.FromResult if GetAllFries is not made async in the repository
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
        // Add validation logic here as needed.
        var updatedFries = _friesRepository.UpdateFries(id, friesUpdateInfo);
        if (updatedFries == null) 
        {
            throw new KeyNotFoundException("Fries not found");
        }
        return await Task.FromResult(updatedFries);
    }

    public async Task<bool> DeleteFries(int id)
    {
        // The DeleteFries method in the repository is not asynchronous, wrap it with Task.FromResult if not changed to async
        return await Task.FromResult(_friesRepository.DeleteFries(id));
    }
}