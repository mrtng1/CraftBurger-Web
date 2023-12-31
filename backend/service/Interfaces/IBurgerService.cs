using infrastructure.Models;

namespace service.Interfaces;

public interface IBurgerService
{
    Task<Burger> CreateBurger(Burger burger);
    Task<IEnumerable<Burger>> GetAllBurgers();
    Task<Burger> GetBurgerById(int id);
    Task<Burger> UpdateBurger(int id, Burger burgerUpdateInfo);
    Task<bool> DeleteBurger(int id);
}