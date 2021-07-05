using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.UseCases.Orders.Commands.CheckoutOrder;

namespace Ordering.API.Mapping
{
    public class OrderingApiMappingProfile : Profile
    {
        public OrderingApiMappingProfile()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
