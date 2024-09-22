using MediatR;

namespace Core.CQRS;

public interface IQueryHandler<T, TE> : IRequestHandler<T, TE> where T : IQuery<TE> { }