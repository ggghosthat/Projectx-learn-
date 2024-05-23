namespace Projectx.Contracts.Repository;

public interface IMessageRepository<T> : IRepository<T>
{
    Task<IEnumerable<T>> GetByTimeframe(DateTime startTime, DateTime endTime);
}