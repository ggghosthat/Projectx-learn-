using Projectx.Contracts.Repository;
using Projectx.Entity.Models;

using Npgsql;
using System.Linq.Expressions;
using System.Transactions;

namespace Projectx.Repository;
public class MessageRepository : IRepository<Message>
{
    private static string _connectionString;
    private static NpgsqlConnection _psqlConnection;

    public MessageRepository()
    {}

    public async Task Seed(string connectionString)
    {
        _connectionString = connectionString;

        string database_create = @"CREATE DATABASE projectx;";

        string client_table_create = @"CREATE TABLE IF NOT EXISTS clients(
            ID SERIAL PRIMARY KEY,
            Name VARCHAR(160) );";

        string message_table_create = @"CREATE TABLE IF NOT EXISTS messages(
            ID SERIAL PRIMARY KEY,
            MessageId UUID,
            ClientId INTEGER,
            Created TIMESTAMP,
            Content VARCHAR(128) );";

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        {

            using var checkIfExistsCommand = new NpgsqlCommand($"SELECT 1 FROM pg_catalog.pg_database WHERE datname = 'projectx'", connection);
            var result = checkIfExistsCommand.ExecuteScalar();

            if (result == null)
            {
                using var command = new NpgsqlCommand($"CREATE DATABASE projectx;", connection);
                command.ExecuteNonQuery();
            }

            _connectionString += "Database=projectx;";
        }

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        {
            var client_table_cmd = new NpgsqlCommand(client_table_create, connection);
            client_table_cmd.ExecuteNonQuery();

            var message_table_cmd = new NpgsqlCommand(message_table_create, connection);
            message_table_cmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public async Task Create(Message entity)
    {
        string message_insert = @"INSERT INTO messages (MessageId, ClientId, Created, Content) VALUES (@messageId, @clientId, @created, @content)";
        
        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        using (var transaction = await connection.BeginTransactionAsync())
        {
            var cmd = new NpgsqlCommand(message_insert, connection);
            cmd.Parameters.AddWithValue("@messageId", entity.Id);
            cmd.Parameters.AddWithValue("@clientId", entity.ClientId);
            cmd.Parameters.AddWithValue("@created", entity.Created);
            cmd.Parameters.AddWithValue("@content", entity.Content);

            cmd.ExecuteNonQuery();

            transaction.Commit();
            connection.Close();
        }
    }

    public async Task Delete(Message entity)
    {
        string message_delete = @"DELETE FROM messages WHERE MessageId = @messageId";

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

    public async Task<IEnumerable<Message>> GetAll()
    {
        IList<Message> resultMessages = new List<Message>();
        string all_messages = @"SELECT MessageId, ClientId, Created, Content FROM messages;";
        
        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        {
            NpgsqlCommand cmd = new NpgsqlCommand(all_messages, connection);
            var reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                var message = new Message
                {
                    Id = (Guid)reader[0],
                    ClientId = (int)reader[1],
                    Created = (DateTime)reader[2],
                    Content = (string)reader[3]
                };

                resultMessages.Add(message);
            }

            connection.Close();
        }

        return resultMessages;
    }

    public async Task<IEnumerable<Message>> GetByTimeframe(DateTime startTime, DateTime endTime)
    {
        IList<Message> resultMessages = new List<Message>();
        string messages_time_interval = @$"SELECT MessageId, ClientId, Created, Content FROM messages
            WHERE Created BETWEEN @startTime and @endTime;";

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        {
            NpgsqlCommand cmd = new NpgsqlCommand(messages_time_interval, connection);
            cmd.Parameters.AddWithValue("@startTime", startTime);
            cmd.Parameters.AddWithValue("@endTime", endTime);
            var reader = await cmd.ExecuteReaderAsync();

            while (reader.Read())
            {
                var message = new Message
                {
                    Id = (Guid)reader[0],
                    ClientId = (int)reader[1],
                    Created = (DateTime)reader[2],
                    Content = (string)reader[3]
                };

                resultMessages.Add(message);
            }

            connection.Close();
        }

        return resultMessages;
    }
}