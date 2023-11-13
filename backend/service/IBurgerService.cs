using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace service
{
    public interface IBurgerService
    {
        Task<Burger> CreateBurger(Burger burger);
        Task<IEnumerable<Burger>> GetAllBurgers();
        Task<Burger> GetBurgerById(int id);
        Task<Burger> UpdateBurger(int id, Burger burgerUpdateInfo);
        Task<bool> DeleteBurger(int id);
        Task<IEnumerable<Ingredient>> GetIngredientsByBurgerId(int burgerId);
    }
}