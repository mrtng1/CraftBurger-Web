using api.Models;
using infrastructure;

namespace service;

public class IngredientService : IIngredientService
{
    private readonly IngredientRepository _ingredientRepository;

    public IngredientService(IngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository ?? throw new ArgumentNullException(nameof(ingredientRepository), "IngredientRepository is null");
    }
    
    public async Task<IEnumerable<Ingredient>> GetIngredientByBurgerId(int burgerId)
    {
        return await _ingredientRepository.GetIngredientByBurgerId(burgerId);
    }
    
    public async Task<Ingredient> CreateIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
        {
            throw new ArgumentNullException(nameof(ingredient), "Ingredient data is null");
        }

        if (string.IsNullOrEmpty(ingredient.Name))
        {
            throw new ArgumentException("Ingredient name must be provided", nameof(ingredient.Name));
        }

        try
        {
            return await _ingredientRepository.CreateIngredient(ingredient);
        }
        catch (Exception)
        {
            throw new Exception("Could not create the ingredient");
        }
    }
    
    public async Task<bool> DeleteIngredient(int ingredientId)
    {
        try
        {
            return await _ingredientRepository.DeleteIngredient(ingredientId);
        }
        catch (Exception)
        {
            throw new Exception("Could not delete the ingredient");
        }
    }
    
    public async Task<bool> AddIngredientToBurger(int burgerId, int ingredientId)
    {
        try
        {
            return await _ingredientRepository.AddIngredientToBurger(burgerId, ingredientId);
        }
        catch (Exception)
        {
            throw new Exception("Could not add ingredient to burger");
        }
    }
}