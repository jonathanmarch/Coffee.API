using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffee.API.Exceptions;
using Coffee.API.Models;
using Coffee.API.Providers;
using Dapper;

namespace Coffee.API.Repositories
{
    public class CoffeeRatingRepository : ICoffeeRatingRepository
    {
        private readonly ISqlServerConnectionProvider sqlServerConnectionProvider;
        private readonly ICoffeeRepository coffeeRepository;
        
        public CoffeeRatingRepository(ISqlServerConnectionProvider sqlServerConnectionProvider, ICoffeeRepository coffeeRepository)
        {
            this.sqlServerConnectionProvider = sqlServerConnectionProvider ?? throw new ArgumentNullException(nameof(SqlServerConnectionProvider));
            this.coffeeRepository = coffeeRepository ?? throw new ArgumentNullException(nameof(CoffeeRepository));
        }

        public async Task<IEnumerable<CoffeeRating>> GetAllItems()
        {
            var sql = "SELECT * FROM Rating";

            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                return await conn.QueryAsync<CoffeeRating>(sql);
            }
        }

        public async Task<CoffeeRating> FindById(int id)
        {
            var sql = @"SELECT * FROM Rating
                        WHERE id = @CoffeeRatingId";
            
            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<CoffeeRating>(sql, new { CoffeeRatingId = id });
            }
        }
        
        public async Task<int> Create(CoffeeRatingDto coffeeRating)
        {
            var sql = @"INSERT INTO Rating (coffeeId, comment, rating)
                            Values (@CoffeeId, @Comment, @Rating);
                            SELECT CAST(SCOPE_IDENTITY() as int)";

            await this.coffeeRepository.ThrowExceptionIfCoffeeDoesNotExist(coffeeRating.CoffeeId);
            
            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                var result = await conn.QueryAsync<int>(sql,
                    new
                    {
                        coffeeRating.CoffeeId,
                        coffeeRating.Comment,
                        coffeeRating.Rating
                    });

                return result.Single();
            }
        }

        public async Task<int> Update(UpdateCoffeeRatingDto coffeeRating)
        {
            var sql = @"UPDATE Rating
                        SET comment = @Comment, rating = @Rating
                        WHERE Id = @Id;";

            await ThrowExceptionIfCoffeeRatingDoesNotExist(coffeeRating.Id);
            
            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                return await conn.ExecuteAsync(sql, new
                {
                    coffeeRating.Id,
                    coffeeRating.Comment,
                    coffeeRating.Rating
                });
            }
        }

        public async Task ThrowExceptionIfCoffeeRatingDoesNotExist(int coffeeRatingId)
        {
            var existingCoffeeItem = await FindById(coffeeRatingId);

            if (existingCoffeeItem == null)
            {
                throw new EntityNotFoundException();
            }
        }
    }
}
