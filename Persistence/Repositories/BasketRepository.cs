using System.IdentityModel.Tokens.Jwt;
using Domain.Contracts;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Persistance.Repositories;

public class BasketRepository(IConnectionMultiplexer redis)
    : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<CustomerBasket?> GetBasketAsync(string basketId)
    {
        var data = await _database.StringGetAsync(basketId);

        if (data.IsNullOrEmpty)
            return null;

        // return JsonSerializer.DeSerialize<CustomerBasket>(data); // i have no idea why this is not working
        var res = JsonConvert.DeserializeObject<CustomerBasket>(data);
        return res;
    }

    public async Task<bool> DeleteBasketItemAsync(string customerId) => await _database.KeyDeleteAsync(customerId);

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeout = null)
    {
        var jsonBasket = JsonConvert.SerializeObject(basket);
        bool isCreatedOrUpdated =
            await _database.StringSetAsync(basket.Id, jsonBasket, timeout ?? TimeSpan.FromDays(30));

        return isCreatedOrUpdated ? await GetBasketAsync(basket.Id) : null;
    }
}