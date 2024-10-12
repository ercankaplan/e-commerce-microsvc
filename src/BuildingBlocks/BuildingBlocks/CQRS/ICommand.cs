using MediatR;

namespace BuildingBlocks.CQRS
{
    /// <summary>
    /// return void
    /// </summary>
    public interface ICommand : ICommand<Unit> { } 
    public interface ICommand<out TResponse>: IRequest<TResponse>
    {
    }
}
