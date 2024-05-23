using Projectx.Contracts.Repository;
using Projectx.Entity.Models;

using Npgsql;

namespace Projectx.Repository;

public class MessageRepository : IMessageRepository<Message>
{
    private static string _connectionString;

    public MessageRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task Create(Message entity)
    {
        string message_insert = @"INSERT INTO messages (MessageId, ClientId, Created, Content) VALUES (@messageId, @clientId, @created, @content)";

        using (var dataSource = NpgsqlDataSource.Create(_connectionString))
        using (var connection = await dataSource.OpenConnectionAsync())
        using (var transaction = await connection.BeginTransactionAsync())
        {
            var cmd = new NpgsqlCommand(message_insert, connection);
            cmd.Parameters.AddWithValue("@messageId", entity.MessageId);
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
            cmd.Parameters.AddWithValue("@messageId", entity.MessageId.ToString());
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
                    MessageId = (Guid)reader[0],
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
                    MessageId = (Guid)reader[0],
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