using System.Collections.Generic;
using System.Threading.Tasks;
using Coffee.API.Models;

namespace Coffee.API.Repositories
{
    public interface ICoffeeRepository
    {
        Task<IEnumerable<CoffeeItem>> GetAllItems();
        
        Task<CoffeeItem> FindById(int id);

        Task<int> Create(CoffeeItemDto coffeeItem);

        Task<int> Update(CoffeeItem coffeeItem);

        Task<int> Delete(CoffeeItem coffeeItem);

        Task ThrowExceptionIfCoffeeDoesNotExist(int coffeeId);
    }
}
