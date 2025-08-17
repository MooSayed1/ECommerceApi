namespace Shared.Dtos.CustomerBasketDtos;

public class BasketDto
{
    public string Id { get; set; }
    public IEnumerable<BasketItemDto> Items { get; set; }
}