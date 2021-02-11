using System.Collections.Generic;
using System.Threading.Tasks;
using Coffee.API.Models;

namespace Coffee.API.Repositories
{
    public interface ICoffeeRatingRepository
    {
        Task<IEnumerable<CoffeeRating>> GetAllItems();

        Task<CoffeeRating> FindById(int id);
        
        Task<int> Create(CoffeeRatingDto coffeeRating);

        Task<int> Update(UpdateCoffeeRatingDto coffeeRating);

        Task ThrowExceptionIfCoffeeRatingDoesNotExist(int coffeeRatingId);
    }
}
