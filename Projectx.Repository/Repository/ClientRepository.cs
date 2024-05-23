using Projectx.Contracts.Repository;
using Projectx.Entity.Models;

using Npgsql;

namespace Projectx.Repository;

public class ClientRepository : IClientRepository<Client>
{
    private static string _connectionString;
    private static NpgsqlConnection _psqlConnection;

    public ClientRepository()
    { }

    public async Task Create(Client entity)
    {
        string message_insert = @"INSERT INTO clients (ClientId, Name) VALUES (@clientId, @name)";

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        using (var transaction = await connection.BeginTransactionAsync())
        {
            var cmd = new NpgsqlCommand(message_insert, connection);
            cmd.Parameters.AddWithValue("@clientId", entity.ClientId);
            cmd.Parameters.AddWithValue("@name", entity.Name);

            cmd.ExecuteNonQuery();

            transaction.Commit();
            connection.Close();
        }
    }

    public async Task Delete(Client entity)
    {
        string message_delete = @"DELETE FROM clients WHERE ClientId = @clientId";

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        using (var transaction = await connection.BeginTransactionAsync())
        {
            var cmd = new NpgsqlCommand(message_delete, connection);
            cmd.Parameters.AddWithValue("@messageId", entity.Id.ToString());
            cmd.ExecuteNonQuery();

            transaction.Commit();
            connection.Close();
        }
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
        IList<Client> resultClients = new List<Client>();
        string all_messages = @"SELECT ClientId, Name FROM clients;";

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        {
            var cmd = new NpgsqlCommand(all_messages, connection);
            var reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                var client = new Client
                {
                    ClientId = (int)reader[0],
                    Name = (string)reader[1]
                };

                resultClients.Add(client);
            }

            connection.Close();
        }

        return resultClients;
    }
}
