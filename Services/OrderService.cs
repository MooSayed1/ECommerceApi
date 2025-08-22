using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderEntities;
using Services.Abstraction.Interfaces;
using Services.SpecificationsFolder;
using Shared.Dtos.OrderDtos;

namespace Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
    {
        var order = await _unitOfWork.GetRepo<Order, Guid>().GetByIdAsync(new OrderSpecifications(id));
        var mappedOrder= _mapper.Map<OrderResultDto>(order); // I want to make profile
        return mappedOrder;
    }

    public async Task<IEnumerable<OrderResultDto>> GetAllOrdersByEmailAsync(string? email)
    {
        var orders = await _unitOfWork.GetRepo<Order, Guid>().GetAllAsync(new OrderSpecifications(email!));
        var mappedOrders = _mapper.Map<IEnumerable<OrderResultDto>>(orders);
        return mappedOrders;
    }

    public Task<OrderResultDto> AddOrderAsync(OrderRequest order, string email)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethodsAsync()
    {
        throw new NotImplementedException();
    }
}