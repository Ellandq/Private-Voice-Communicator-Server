using Communicator.Server.Realtime;
using Communicator.Server.Realtime.Connections;
using Communicator.Server.Realtime.Messaging;

namespace Communicator.Server.Tests.Realtime;

public class ConnectionManagerTests
{
    [Fact]
    public void AddConnection_ConnectionCanBeRetrieved()
    {
        var manager = new ConnectionManager();
        var connection = CreateConnection();

        manager.AddConnection(connection);

        var found = manager.TryGetConnection(
            connection.Id,
            out var result);

        Assert.True(found);
        Assert.Same(connection, result);
    }

    [Fact]
    public void AddConnection_ConnectionAppearsInConnections()
    {
        var manager = new ConnectionManager();
        var connection = CreateConnection();

        manager.AddConnection(connection);

        Assert.Contains(
            connection,
            manager.Connections);
    }

    [Fact]
    public void AddConnection_DuplicateIdThrows()
    {
        var manager = new ConnectionManager();

        var first = CreateConnection();
        var second = new TestClientConnection(first.Id);

        manager.AddConnection(first);

        Assert.Throws<InvalidOperationException>(
            () => manager.AddConnection(second));
    }

    [Fact]
    public void RemoveConnection_ExistingConnectionIsRemoved()
    {
        var manager = new ConnectionManager();
        var connection = CreateConnection();

        manager.AddConnection(connection);

        var removed = manager.RemoveConnection(
            connection.Id);

        Assert.True(removed);

        Assert.False(
            manager.TryGetConnection(
                connection.Id,
                out _));

        Assert.DoesNotContain(
            connection,
            manager.Connections);
    }

    [Fact]
    public void RemoveConnection_UnknownIdReturnsFalse()
    {
        var manager = new ConnectionManager();

        var removed = manager.RemoveConnection(
            Guid.NewGuid());

        Assert.False(removed);
    }

    private static TestClientConnection CreateConnection()
    {
        return new TestClientConnection(
            Guid.NewGuid());
    }

    private sealed class TestClientConnection(Guid id)
        : IClientConnection
    {
        public Guid Id { get; } = id;

        public bool IsOpen => true;

        public Task<MessageReadResult?> ReceiveAsync(
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(
            string message,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync(
            System.Net.WebSockets.WebSocketCloseStatus status,
            string? description = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}