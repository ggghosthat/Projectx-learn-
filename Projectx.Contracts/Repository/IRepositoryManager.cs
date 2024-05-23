using Projectx.Entity.Models;

namespace Projectx.Contracts.Repository;

public interface IRepositoryManager
{
    public IClientRepository<Client> Clients { get; }

    public IMessageRepository<Message> Messages { get; }
}