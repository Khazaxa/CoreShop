using MediatR;

namespace Core.CQRS;

public interface ICommandHandler<T, TE> : IRequestHandler<T, TE> where T : ICommand<TE> { }