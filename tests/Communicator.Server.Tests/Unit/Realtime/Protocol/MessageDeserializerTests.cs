using System.Text;
using System.Text.Json;
using Communicator.Server.Realtime.Protocol;

namespace Communicator.Server.Tests.Realtime.Protocol;

public sealed class MessageDeserializerTests
{
    private readonly MessageDeserializer _deserializer = new();

    [Fact]
    public void Deserialize_ValidMessage_ReturnsMessage()
    {
        const string json = """
                            {
                                "type": "message.send",
                                "requestId": "11111111-1111-1111-1111-111111111111",
                                "payload": {
                                    "conversationId": "22222222-2222-2222-2222-222222222222",
                                    "content": "Hello server"
                                }
                            }
                            """;

        var data = Encoding.UTF8.GetBytes(json);

        var result = _deserializer.Deserialize(data);

        Assert.Equal(
            "message.send",
            result.Type);

        Assert.Equal(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            result.RequestId);

        Assert.NotNull(result.Payload);
    }

    [Fact]
    public void Deserialize_EmptyJson_Throws()
    {
        var data = Encoding.UTF8.GetBytes("");

        Assert.Throws<JsonException>(() =>
            _deserializer.Deserialize(data));
    }

    [Fact]
    public void Deserialize_InvalidJson_Throws()
    {
        var data = Encoding.UTF8.GetBytes(
            "{ this is not valid json }");

        Assert.Throws<JsonException>(() =>
            _deserializer.Deserialize(data));
    }
}