using infrastructure; 
using api.Models;
using Npgsql;

namespace service;

public class BurgerService : IBurgerService
    {
        private readonly BurgerRepository _burgerRepository;

        public BurgerService(BurgerRepository burgerRepository)
        {
            _burgerRepository = burgerRepository ?? throw new ArgumentNullException(nameof(burgerRepository), "BurgerRepository is null");
        }

        public async Task<Burger> CreateBurger(Burger burger)
        {
            if (burger == null)
            {
                throw new ArgumentNullException(nameof(burger), "Burger data is null");
            }

            if (string.IsNullOrEmpty(burger.BurgerName))
            {
                throw new ArgumentException("Burger name must be provided", nameof(burger.BurgerName));
            }

            if (burger.BurgerPrice <= 0)
            {
                throw new ArgumentException("Price must be a positive value", nameof(burger.BurgerPrice));
            }

            try
            {
                Burger createdBurger = await Task.Run(() => _burgerRepository.CreateBurger(burger));
                return createdBurger;
            }
            catch (Exception)
            {
                throw new Exception("Could not create the burger");
            }
        }

        public async Task<IEnumerable<Burger>> GetAllBurgers()
        {
            try
            {
                return await Task.Run(() => _burgerRepository.GetAllBurgers());
            }
            catch (NpgsqlException ex)
            {
                throw new Exception("A database error occurred while trying to get all burgers.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while trying to get all burgers.", ex);
            }
        }

        public async Task<Burger> GetBurgerById(int id)
        {
            try
            {
                var burger = await Task.Run(() => _burgerRepository.GetBurgerById(id));
                if (burger == null) throw new KeyNotFoundException("Burger not found");
                return burger;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("Could not get the burger by ID");
            }
        }

        public async Task<Burger> UpdateBurger(int id, Burger burgerUpdateInfo)
        {
            if (burgerUpdateInfo == null)
            {
                throw new ArgumentNullException(nameof(burgerUpdateInfo), "Update information is null");
            }

            try
            {
                var burger = await Task.Run(() => _burgerRepository.UpdateBurger(id, burgerUpdateInfo));
                if (burger == null) throw new KeyNotFoundException("Burger not found");

                return burger;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("Could not update the burger");
            }
        }

        public async Task<bool> DeleteBurger(int id)
        {
            try
            {
                return await Task.Run(() => _burgerRepository.DeleteBurger(id));
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("Could not delete the burger");
            }
        }
    }