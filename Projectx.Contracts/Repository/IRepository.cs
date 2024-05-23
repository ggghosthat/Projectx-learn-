using System.Linq.Expressions;

namespace Projectx.Contracts.Repository;

public interface IRepository<T>
{
    Task Create(T entity);
    Task Delete(T entity);

    Task<IEnumerable<T>> GetAll();
}