using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Interfaces;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IProductService> _productService;
    private readonly Lazy<IBasketService> _basketService;
    private readonly Lazy<IAuthService> _authService;

    public ServiceManager(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IBasketRepository basketRepository,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
        _basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
        _authService = new Lazy<IAuthService>(() => new AuthService(userManager, signInManager, roleManager, configuration, mapper));
    }

    public IProductService ProductService => _productService.Value;
    public IBasketService BasketService => _basketService.Value;
    public IAuthService AuthService => _authService.Value;
}
