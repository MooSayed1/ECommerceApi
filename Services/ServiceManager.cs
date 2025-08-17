using AutoMapper;
using Domain.Contracts;
using Services.Abstraction.Interfaces;

namespace Services;

public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository) : IServiceManager
{
    // defer execution of ProductService creation until it's actually necessary
    private readonly Lazy<IProductService> _productService = new(() => new ProductService(unitOfWork, mapper));
    private readonly Lazy<IBasketService> _basketService = new (()=>new BasketService(basketRepository, mapper));
    public IProductService ProductService => _productService.Value;
    public IBasketService BasketService =>  _basketService.Value;
}
