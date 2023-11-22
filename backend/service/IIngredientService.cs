using api.Models;

namespace service;

public interface IIngredientService
{
    Task<IEnumerable<Ingredient>> GetIngredientByBurgerId(int burgerId);
    Task<Ingredient> CreateIngredient(Ingredient ingredient);
    Task<bool> DeleteIngredient(int ingredientId);
    Task<bool> AddIngredientToBurger(int burgerId, int ingredientId);
}