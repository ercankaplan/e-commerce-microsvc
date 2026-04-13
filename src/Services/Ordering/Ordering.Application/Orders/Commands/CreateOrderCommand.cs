using BuildingBlocks.CQRS;
using Ordering.Application.Dtos;
using FluentValidation;

namespace Ordering.Application.Orders.Commands;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;


public record CreateOrderResult(Order Order);


public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Order name cannot be empty");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("Customer ID cannot be null");
        RuleFor(x=> x.Order.Items).NotEmpty().WithMessage("Order must have at least one item");
    }
}