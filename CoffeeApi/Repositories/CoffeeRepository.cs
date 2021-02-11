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
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly ISqlServerConnectionProvider sqlServerConnectionProvider;

        public CoffeeRepository(ISqlServerConnectionProvider sqlServerConnectionProvider)
        {
            this.sqlServerConnectionProvider = sqlServerConnectionProvider ?? throw new ArgumentNullException(nameof(SqlServerConnectionProvider));
        }

        public async Task<IEnumerable<CoffeeItem>> GetAllItems()
        {
            var sql = @"SELECT c.*, r.*
                        FROM Coffee c
                        LEFT JOIN Rating r
                        ON c.id = r.coffeeId";

            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                var results = await conn.QueryAsync<CoffeeItem, CoffeeRating, CoffeeItem>
                (sql, (coffeeItem, coffeeRating) =>
                {
                    coffeeItem.Ratings.Add(coffeeRating);
                    return coffeeItem;
                });

                return MapRowsToRatingProperty(results);
            }
        }
        
        public async Task<CoffeeItem> FindById(int id)
        {
            var sql = @"SELECT c.*, r.*
                        FROM Coffee c
                        LEFT JOIN Rating r
                        ON c.id = r.coffeeId
                        WHERE c.id  = @CoffeeId";

            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                var results = await conn.QueryAsync<CoffeeItem, CoffeeRating, CoffeeItem>
                (sql, (coffeeItem, coffeeRating) =>
                {
                    coffeeItem.Ratings.Add(coffeeRating);
                    return coffeeItem;
                }, new { CoffeeId = id });

                if (!results.Any())
                {
                    return null;
                }
                
                return MapRowsToRatingPropertyToSingleItem(results);
            }
        }

        public async Task<int> Create(CoffeeItemDto coffeeItem)
        {
            var sql = @"INSERT INTO Coffee (name, description, caffeineContent)
                            Values (@Name, @Description, @CaffeineContent);
                            SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                var result = await conn.QueryAsync<int>(sql,
                    new
                    {
                        coffeeItem.Name,
                        coffeeItem.Description,
                        coffeeItem.CaffeineContent
                    });

                return result.Single();
            }
        }

        public async Task<int> Update(CoffeeItem coffeeItem)
        {
            await ThrowExceptionIfCoffeeDoesNotExist(coffeeItem.Id);
            
            var sql = @"UPDATE Coffee
                        SET name = @Name, description = @Description, caffeineContent = @CaffeineContent
                        WHERE Id = @Id;";

            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                return await conn.ExecuteAsync(sql, new
                {
                    coffeeItem.Id,
                    coffeeItem.Name,
                    coffeeItem.Description,
                    coffeeItem.CaffeineContent
                });
            }
        }

        public async Task<int> Delete(CoffeeItem coffeeItem)
        {
            await ThrowExceptionIfCoffeeDoesNotExist(coffeeItem.Id);
            
            var sql = @"DELETE FROM Coffee
                        WHERE Id = @Id;";

            using (var conn = sqlServerConnectionProvider.GetDbConnection())
            {
                return await conn.ExecuteAsync(sql, new
                {
                    coffeeItem.Id,
                    coffeeItem.Name,
                    coffeeItem.Description,
                    coffeeItem.CaffeineContent
                });
            }
        }

        public async Task ThrowExceptionIfCoffeeDoesNotExist(int coffeeId)
        {
            var existingCoffeeItem = await FindById(coffeeId);

            if (existingCoffeeItem == null)
            {
                throw new EntityNotFoundException();
            }
        }

        private List<CoffeeItem> MapRowsToRatingProperty(IEnumerable<CoffeeItem> coffeeItems)
        {
            return coffeeItems
                .GroupBy(i => i.Id)
                .Select(i =>
                {
                    var coffeeItem = i.First();

                    coffeeItem.Ratings = i
                        .Select(r => r.Ratings.Single())
                        .Where(r => r != null)
                        .ToList();

                    return coffeeItem;
                })
                .ToList();
        }

        private CoffeeItem MapRowsToRatingPropertyToSingleItem(IEnumerable<CoffeeItem> coffeeItems)
        {
            return coffeeItems
                .GroupBy(i => i.Id)
                .Select(i =>
                {
                    var coffeeItem = i.First();

                    coffeeItem.Ratings = i
                        .Select(r => r.Ratings.Single())
                        .Where(r => r != null)
                        .ToList();

                    return coffeeItem;
                }).First();
        }
    }
}
