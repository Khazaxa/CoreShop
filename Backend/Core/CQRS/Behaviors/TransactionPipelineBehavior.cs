using Core.Database;
using MediatR;

namespace Core.CQRS.Behaviors;

public class TransactionPipelineBehavior<TRequest, TResponse>(IUnitOfWork _unitOfWork)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse> 
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                var response = await next();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(CancellationToken.None);
                return response;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(CancellationToken.None);
                throw;
            }
        }
    }
}