using infrastructure;
using api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

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
        if (fries == null)
        {
            throw new ArgumentNullException(nameof(fries), "Fries data is null");
        }

        if (string.IsNullOrEmpty(fries.FriesName))
        {
            throw new ArgumentException("Fries name must be provided", nameof(fries.FriesName));
        }

        if (fries.FriesPrice <= 0)
        {
            throw new ArgumentException("Price must be a positive value", nameof(fries.FriesPrice));
        }

        try
        {
            return await _friesRepository.CreateFries(fries);
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException($"Database error occurred while creating fries: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while creating fries: {ex.Message}", ex);
        }
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
        return await Task.FromResult(_friesRepository.DeleteFries(id));
    }
}