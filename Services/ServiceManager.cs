using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;
using Services.Abstraction.Interfaces;

namespace Services;

public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository,UserManager<User>userManager) : IServiceManager
{
    // defer execution of ProductService creation until it's actually necessary
    private readonly Lazy<IProductService> _productService = new(() => new ProductService(unitOfWork, mapper));
    private readonly Lazy<IBasketService> _basketService = new (()=>new BasketService(basketRepository, mapper));
    private readonly Lazy<IAuthenticationService> _authService = new (()=>new AuthenticationService(userManager));
    public IProductService ProductService => _productService.Value;
    public IBasketService BasketService =>  _basketService.Value;
    public IAuthenticationService AuthenticationService => _authService.Value;
}
