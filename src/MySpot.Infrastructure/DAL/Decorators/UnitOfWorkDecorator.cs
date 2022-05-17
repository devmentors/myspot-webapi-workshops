using MySpot.Application.Abstractions;

namespace MySpot.Infrastructure.DAL.Decorators;

internal class UnitOfWorkDecorator<T> : ICommandHandler<T> where T : class, ICommand
{
    private readonly ICommandHandler<T> _handler;
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkDecorator(ICommandHandler<T> handler, IUnitOfWork unitOfWork)
    {
        _handler = handler;
        _unitOfWork = unitOfWork;
    }
    
    public async Task HandleAsync(T command)
    {
        await _unitOfWork.ExecuteAsync(() => _handler.HandleAsync(command));
    }
}