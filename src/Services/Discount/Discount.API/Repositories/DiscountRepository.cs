using System.Threading.Tasks;
using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private const string SelectSqlScript = "SELECT Id, ProductName, Description, Amount FROM Coupon WHERE @ProductName = ProductName";

        private const string InsertSqlScript =
            "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)";

        private const string UpdateSqlScript =
            "UPDATE Coupon SET ProductName = @ProductName, @Description = Description, Amount = @Amount WHERE ProductName = @ProductName";

        private const string DeleteSqlScript = "DELETE FROM Coupon WHERE ProductName = @ProductName";

        private readonly string _connectionString;


        public DiscountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Coupon> Get(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var coupon =
                await connection.QueryFirstOrDefaultAsync<Coupon>(SelectSqlScript,
                    new {ProductName = productName});


            return coupon ?? new Coupon
            {
                ProductName = "No Discount", Amount = 0, Description = "No Description"
            };
        }

        public async Task Create(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(InsertSqlScript,
                new {coupon.ProductName, coupon.Description, coupon.Amount});
        }

        public async Task Update(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(UpdateSqlScript,
                new { coupon.ProductName, coupon.Description, coupon.Amount });
        }

        public async Task Delete(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            await connection.ExecuteAsync(DeleteSqlScript,
                new { ProductName = productName });
        }
    }
}
