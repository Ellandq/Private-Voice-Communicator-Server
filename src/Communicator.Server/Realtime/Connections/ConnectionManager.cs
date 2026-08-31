using System.Collections.Concurrent;
using Communicator.Server.Utils.Collections;

namespace Communicator.Server.Realtime.Connections;

public sealed class ConnectionManager
{
    private readonly ConcurrentDictionary<Guid, IClientConnection>
        _connections = new();

    public IReadOnlyCollection<IClientConnection> Connections =>
        _connections.Values.AsReadOnly();

    public IClientConnection AddConnection(
        IClientConnection connection)
    {
        if (!_connections.TryAdd(connection.Id, connection))
        {
            throw new InvalidOperationException(
                $"Connection with ID {connection.Id} already exists.");
        }
        
        Console.WriteLine(
            $"Client connected: {connection.Id} " +
            $"Connections: {_connections.Count}");

        return connection;
    }

    public bool RemoveConnection(Guid connectionId)
    {
        var removed = _connections.TryRemove(connectionId, out _);
        
        if (removed)
        {
            Console.WriteLine(
                $"Client disconnected: {connectionId} " +
                $"Connections: {_connections.Count}");
        }

        return removed;
    }

    public bool TryGetConnection(
        Guid connectionId,
        out IClientConnection? connection)
    {
        return _connections.TryGetValue(
            connectionId,
            out connection);
    }
}