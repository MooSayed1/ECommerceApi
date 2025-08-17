namespace Services.Abstraction.Interfaces;

public interface IServiceManager
{
    public IProductService ProductService { get; }
    public IBasketService BasketService { get; }
}