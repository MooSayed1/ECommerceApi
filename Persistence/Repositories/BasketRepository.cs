using System.IdentityModel.Tokens.Jwt;
using Domain.Contracts;
using Newtonsoft.Json;
using System.Text.Json;
using StackExchange.Redis;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Persistance.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IConnectionMultiplexer _redis;
    private IDatabase _database;

    public BasketRepository(IConnectionMultiplexer  redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    } 
    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var data = await _database.StringGetAsync(basketId);

        if (data.IsNullOrEmpty)
            return null;

        // return JsonSerializer.Serialize(data); // i have no idea why this is not working
        return JsonConvert.DeserializeObject<CustomerBasket>(data);
    }

    public async Task<bool> DeleteBasketItemAsync(string customerId) => await _database.KeyDeleteAsync(customerId);

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeout = null)
    {
        var jsonBasket = JsonConvert.SerializeObject(basket);
        bool isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, timeout??TimeSpan.FromDays(30));
        return isCreatedOrUpdated? await GetBasketAsync(basket.Id): null;
    }
}