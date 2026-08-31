## 1. Transport

The Communicator server exposes a WebSocket endpoint at:

```text
/ws
```

The connection uses the standard WebSocket protocol over HTTP(S):

```text
ws://<server>/ws
```

or, when the server is hosted over TLS:

```text
wss://<server>/ws
```

### Current transport

The current implementation supports:

* WebSocket connections
* Text WebSocket messages
* Fragmented WebSocket messages
* Graceful WebSocket closure
* Multiple simultaneous client connections

Binary WebSocket messages are currently rejected by the application-level protocol.

### Important: WebSocket is not the media transport

WebSocket is intended for application-level realtime communication:

* Text messages
* Conversation events
* User/presence events
* Authentication/session-related messages
* Call control/signaling
* Other realtime application events

Voice, camera video, and screen sharing will be handled separately through LiveKit.

The expected architecture is therefore:

```text
Client
 │
 ├── WebSocket ──────────────── Server
 │      │
 │      └── Application protocol
 │
 └── LiveKit connection ────── LiveKit
        │
        ├── Voice
        ├── Camera
        └── Screen sharing
```

---

# 2. Message format

All application messages currently use UTF-8 encoded JSON.

Every message is wrapped in a common envelope:

```json
{
  "type": "message.send",
  "requestId": "11111111-1111-1111-1111-111111111111",
  "payload": {}
}
```

The envelope contains three fields.

| Field       | Type   |           Required | Description                            |
| ----------- | ------ | -----------------: | -------------------------------------- |
| `type`      | string |                Yes | Identifies the operation or event      |
| `requestId` | UUID   |                 No | Correlates a request with its response |
| `payload`   | object | Depends on message | Data associated with the message       |

---

## 3. Message types

Message types are strings rather than numeric IDs or enums.

They use a dotted naming convention:

```text
message.send
message.created
conversation.join
user.presence.changed
```

The server currently defines:

```text
message.send
message.created
```

More message types will be added as functionality is implemented.

### Naming convention

The general convention is:

```text
<domain>.<action>
```

for client requests:

```text
message.send
conversation.join
conversation.leave
```

and:

```text
<domain>.<event>
```

for server events:

```text
message.created
conversation.updated
user.presence.changed
```

The exact set of message types is expected to grow with the application.

---

# 4. Requests

Requests are messages sent from the client to the server.

For example:

```json
{
  "type": "message.send",
  "requestId": "11111111-1111-1111-1111-111111111111",
  "payload": {
    "conversationId": "22222222-2222-2222-2222-222222222222",
    "content": "Hello!"
  }
}
```

The `requestId` allows the client to associate a server response with the operation that caused it.

Clients should generate a new UUID for each request that requires correlation.

---

# 5. Current request: `message.send`

## Client → Server

Message type:

```text
message.send
```

Payload:

```json
{
  "conversationId": "22222222-2222-2222-2222-222222222222",
  "content": "Hello!"
}
```

Full example:

```json
{
  "type": "message.send",
  "requestId": "11111111-1111-1111-1111-111111111111",
  "payload": {
    "conversationId": "22222222-2222-2222-2222-222222222222",
    "content": "Hello!"
  }
}
```

### Payload fields

| Field            | Type   | Description                        |
| ---------------- | ------ | ---------------------------------- |
| `conversationId` | UUID   | Conversation receiving the message |
| `content`        | string | Message contents                   |

The server currently receives this request and passes it into the application layer.

Persistence and distribution to other clients will be implemented as the messaging system develops.

---

# 6. Events

Events are messages sent from the server to clients when something happens.

For example, once message persistence and distribution are implemented, the server will be able to broadcast:

```text
message.created
```

A future example might look like:

```json
{
  "type": "message.created",
  "requestId": null,
  "payload": {
    "id": "33333333-3333-3333-3333-333333333333",
    "conversationId": "22222222-2222-2222-2222-222222222222",
    "senderId": "44444444-4444-4444-4444-444444444444",
    "content": "Hello!",
    "createdAt": "2026-08-31T18:00:00Z"
  }
}
```

Events generally do not need a `requestId` because they are unsolicited notifications.

A request may nevertheless result in both:

1. A response confirming the request.
2. One or more events caused by the request.

For example:

```text
Client                         Server
  │                              │
  │ message.send                 │
  │ requestId = A                │
  ├─────────────────────────────>│
  │                              │
  │       response for A         │
  │<─────────────────────────────┤
  │                              │
  │       message.created        │
  │<─────────────────────────────┤
  │                              │
```

Other clients in the conversation may receive only the event.

---

# 7. Request IDs and ordering

Clients should not assume that requests are processed or responses are received in the same order they were sent.

For example:

```text
Request A → message.send
Request B → conversation.join
Request C → user.update
```

may complete in a different order.

The `requestId` exists to allow the client to correlate responses without relying on ordering.

Events should also be treated as independently arriving realtime notifications.

The client should therefore dispatch incoming messages based on `type`, not based on their position in a sequence.

---

# 8. Unknown message types

Clients should expect new message types to be introduced as the protocol evolves.

A client encountering an unknown server event should preferably ignore it rather than terminating the WebSocket connection.

Similarly, the server will reject unsupported client message types.

Example:

```json
{
  "type": "some.future.message",
  "requestId": "11111111-1111-1111-1111-111111111111",
  "payload": {}
}
```

A future protocol error response will communicate the failure to the client.

---

# 9. Error handling

The error protocol is not finalized yet.

The intended design is that protocol/application errors should normally result in an error response rather than immediately closing the WebSocket.

For example, a future error response could look like:

```json
{
  "type": "error",
  "requestId": "11111111-1111-1111-1111-111111111111",
  "payload": {
    "code": "conversation.not_found",
    "message": "The requested conversation does not exist."
  }
}
```

The `requestId` allows the client to determine which request failed.

Connection-level failures are different from application errors.

Examples of conditions that may close the connection include:

* Invalid WebSocket protocol
* Authentication failure
* Protocol version incompatibility
* Malformed communication that cannot safely be processed
* Client/server shutdown
* Network failure

The exact close/error policy will be defined as authentication and error handling are implemented.

---

# 10. Authentication

Authentication is not currently implemented.

The planned protocol will authenticate the client before allowing access to protected application functionality.

The eventual connection lifecycle is expected to resemble:

```text
Connect WebSocket
      │
      ▼
Authenticate
      │
      ├── failure ──> close connection
      │
      ▼
Authenticated connection
      │
      ▼
Normal realtime communication
```

Authentication details will be documented here once implemented.

Clients should not assume that simply establishing a WebSocket connection grants access to conversations or user data.

---

# 11. Connection lifecycle

A client should generally maintain one WebSocket connection to the server for its realtime application communication.

The connection lifecycle is:

```text
Disconnected
    │
    │ connect
    ▼
Connected
    │
    │ authenticate
    ▼
Authenticated
    │
    ├── send requests
    ├── receive events
    └── receive responses
    │
    │ close/network failure
    ▼
Disconnected
```

The client should be prepared for the connection to disappear unexpectedly and should eventually implement reconnection behavior.

Reconnection behavior and session recovery are not implemented yet.

---

# 12. Future protocol areas

The protocol will expand as functionality is added.

Likely future message categories include:

## Conversations

```text
conversation.create
conversation.delete
conversation.rename
conversation.join
conversation.leave
conversation.updated
```

## Messages

```text
message.send
message.created
message.edited
message.deleted
```

## Users

```text
user.update
user.presence.changed
user.typing
```

## Calls

```text
call.join
call.leave
call.participant.joined
call.participant.left
```

The actual message definitions will be added as those features are implemented.

---

# 13. LiveKit integration

LiveKit will handle realtime media rather than the application WebSocket.

The WebSocket protocol will therefore control the application state surrounding a call.

For example:

```text
Client A
   │
   │ call.join
   ├──────────────────> Server
   │                       │
   │                       │ create/authorize LiveKit access
   │                       │
   │<──────────────────────┤
   │
   │ LiveKit connection
   ├──────────────────────────────> LiveKit
   │
   │ audio/video/screen
   │<─────────────────────────────>
```

The application server should not proxy the actual audio/video streams.

This keeps the responsibilities separated:

```text
Communicator Server
├── Users
├── Authentication
├── Conversations
├── Messages
├── Permissions
├── Presence
├── Call state
└── LiveKit authorization/signaling

LiveKit
├── Voice
├── Camera video
└── Screen sharing
```

---

# 14. Current implementation status

At the current development stage:

| Feature                 | Status          |
| ----------------------- | --------------- |
| WebSocket connection    | Implemented     |
| Multiple connections    | Implemented     |
| Text messages           | Implemented     |
| JSON message envelope   | Implemented     |
| Message type routing    | Implemented     |
| `message.send`          | Implemented     |
| Message persistence     | Not implemented |
| Message broadcasting    | Not implemented |
| Responses               | Not implemented |
| Error protocol          | Not finalized   |
| Authentication          | Not implemented |
| Presence                | Not implemented |
| Conversation management | Not implemented |
| LiveKit integration     | Not implemented |
