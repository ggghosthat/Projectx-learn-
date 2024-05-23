using Npgsql;
using Projectx.Contracts.Repository;
using Projectx.Entity.Models;

namespace Projectx.Repository.RepositoryManager;

public class RepositoryManager : IRepositoryManager
{
    private static string _connectionString;

    private IClientRepository<Client> _clientRepository;
    private IMessageRepository<Message> _messageRepository;

    public RepositoryManager(string connectionString)
    {
        _connectionString = connectionString;
        SeedDatabase();
    }


    public IClientRepository<Client> Clients
    {
        get
        {
            if (_clientRepository == null)
                _clientRepository = new ClientRepository(_connectionString);

            return _clientRepository;
        }
    }

    public IMessageRepository<Message> Messages
    {
        get
        {
            if (_messageRepository == null)
                _messageRepository = new MessageRepository(_connectionString);

            return _messageRepository;
        }
    }

    private void SeedDatabase()
    {
        string database_create = @"CREATE DATABASE projectx;";

        string client_table_create = @"
            CREATE TABLE IF NOT EXISTS clients(
            ID SERIAL PRIMARY KEY,
            ClientId INTEGER,
            Name VARCHAR(160) );";

        string message_table_create = @"
            CREATE TABLE IF NOT EXISTS messages(
            ID SERIAL PRIMARY KEY,
            MessageId UUID,
            ClientId INTEGER,
            Created TIMESTAMP,
            Content VARCHAR(128) );";

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = dataSource.OpenConnection())
        {

            using var checkIfExistsCommand = new NpgsqlCommand($"SELECT 1 FROM pg_catalog.pg_database WHERE datname = 'projectx'", connection);
            var result = checkIfExistsCommand.ExecuteScalar();

            if (result == null)
            {
                var command = new NpgsqlCommand($"CREATE DATABASE projectx;", connection);
                command.ExecuteNonQuery();
            }

            _connectionString += "Database=projectx;";
        }

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = dataSource.OpenConnection())
        {
            var client_table_cmd = new NpgsqlCommand(client_table_create, connection);
            client_table_cmd.ExecuteNonQuery();

            var message_table_cmd = new NpgsqlCommand(message_table_create, connection);
            message_table_cmd.ExecuteNonQuery();

            connection.Close();
        }
    }
}