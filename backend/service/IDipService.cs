using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace service
{
    public interface IDipService
    {
        Task<Dip> CreateDip(Dip dip);
        Task<IEnumerable<Dip>> GetAllDips();
        Task<Dip> GetDipById(int id);
        Task<Dip> UpdateDip(int id, Dip dipUpdateInfo);
        Task<bool> DeleteDip(int id);
    }
}