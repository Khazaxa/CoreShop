using System.Linq.Expressions;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Core.Database;

public abstract class EntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
	where TEntity : EntityBase
{
	protected readonly DbSet<TEntity> _dbSet;

	public EntityRepositoryBase(IUnitOfWork unitOfWork)
	{
		_dbSet = unitOfWork.GetDbSet<TEntity>();
	}

	protected abstract IQueryable<TEntity> GetQuery();

	public async Task<TEntity> FindAsync(int id, CancellationToken cancellationToken)
	{
		return (await GetQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken))
		       ?? throw new DomainException($"Entity with id {id} does not exist.",
			       (int)CommonErrorCode.EntityNotFound);
	}

	public Task<IList<TEntity>> FindAsync(IList<int> ids, CancellationToken cancellationToken)
		=> FindAsync(ids, GetQuery(), cancellationToken);
	
	public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
		=> GetQuery().AnyAsync(predicate, cancellationToken: cancellationToken);

	public async Task<IList<TResult>> FindAsync<TResult>(
		Expression<Func<TEntity, bool>> predicate,
		Expression<Func<TEntity, TResult>> projection,
		CancellationToken cancellationToken)
	{
		return await GetQuery()
			.Where(predicate)
			.Select(projection)
			.ToListAsync(cancellationToken);
	}

	public async Task<IList<TResult>> FindAsync<TResult, TSort>(
		IEnumerable<Expression<Func<TEntity, bool>>?> predicates,
		Expression<Func<TEntity, TSort>> orderSelector,
		Expression<Func<TEntity, TResult>> projection,
		CancellationToken cancellationToken,
		bool ascending = true)
	{
		var query = GetQuery();

		foreach (var predicate in predicates.Where(x => x != null))
			query = query.Where(predicate!);

		query = ascending
			? query.OrderBy(orderSelector)
			: query.OrderByDescending(orderSelector);

		return await query
			.Select(projection)
			.ToListAsync(cancellationToken);
	}

	public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
		CancellationToken cancellationToken)
	{
		return await GetQuery()
			.Where(predicate)
			.ToListAsync(cancellationToken);
	}

	public void Add(TEntity entity)
	{
		_dbSet.Add(entity);
	}

	private static async Task<IList<TEntity>> FindAsync(IList<int> ids, IQueryable<TEntity> query,
		CancellationToken cancellationToken)
	{
		ids = ids.Distinct().ToList();
		var entities = await query.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

		if (entities.Count != ids.Count)
		{
			var notFoundIds = ids.Except(entities.Select(x => x.Id)).ToList();
			throw new DomainException(
				$"Entities of type {typeof(TEntity).Name} with ids {string.Join(", ", notFoundIds)} do not exist.",
				(int)CommonErrorCode.EntityNotFound);
		}

		return entities;
	}
}