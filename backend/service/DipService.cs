using infrastructure; 
using api.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace service;

public class DipService : IDipService
{
    private readonly DipRepository _dipRepository;

    public DipService(DipRepository dipRepository)
    {
        _dipRepository = dipRepository ?? throw new ArgumentNullException(nameof(dipRepository), "DipRepository is null");
    }

    public async Task<Dip> CreateDip(Dip dip)
    {
        if (dip == null)
        {
            throw new ArgumentNullException(nameof(dip), "Dip data is null");
        }

        if (string.IsNullOrEmpty(dip.DipName))
        {
            throw new ArgumentException("Dip name must be provided", nameof(dip.DipName));
        }

        if (dip.DipPrice <= 0)
        {
            throw new ArgumentException("Price must be a positive value", nameof(dip.DipPrice));
        }

        try
        {
            return await _dipRepository.CreateDip(dip);
        }
        catch (NpgsqlException ex)
        {
            // Log database-specific exceptions here
            // Example: _logger.LogError(ex, "Database error occurred while creating a dip.");
            throw new InvalidOperationException($"Database error occurred while creating a dip: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Log general exceptions here
            // Example: _logger.LogError(ex, "An error occurred while creating a dip.");
            throw new InvalidOperationException($"An error occurred while creating a dip: {ex.Message}", ex);
        }
    }


    public async Task<IEnumerable<Dip>> GetAllDips()
    {
        try
        {
            return await Task.Run(() => _dipRepository.GetAllDips());
        }
        catch (NpgsqlException ex)
        {
            throw new Exception("A database error occurred while trying to get all dips.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while trying to get all dips.", ex);
        }
    }

    public async Task<Dip> GetDipById(int id)
    {
        try
        {
            var dip = await Task.Run(() => _dipRepository.GetDipById(id));
            if (dip == null) throw new KeyNotFoundException("Dip not found");
            return dip;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Could not get the dip by ID");
        }
    }

    public async Task<Dip> UpdateDip(int id, Dip dipUpdateInfo)
    {
        if (dipUpdateInfo == null)
        {
            throw new ArgumentNullException(nameof(dipUpdateInfo), "Update information is null");
        }

        try
        {
            var dip = await Task.Run(() => _dipRepository.UpdateDip(id, dipUpdateInfo));
            if (dip == null) throw new KeyNotFoundException("Dip not found");

            return dip;
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Could not update the dip");
        }
    }

    public async Task<bool> DeleteDip(int id)
    {
        try
        {
            return await Task.Run(() => _dipRepository.DeleteDip(id));
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new Exception("Could not delete the dip");
        }
    }
}
