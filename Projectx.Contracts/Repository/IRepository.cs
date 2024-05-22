using System.Linq.Expressions;

namespace Projectx.Contracts.Repository;

public interface IRepository<T>
{
    Task Seed(string connectionString);

    Task Create(T entity);
    Task Delete(T entity);

    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetByTimeframe(DateTime startTime, DateTime endTime);
}