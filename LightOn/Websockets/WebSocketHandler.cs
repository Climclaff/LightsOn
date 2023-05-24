using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace LightOn.Websockets
{
    public class WebSocketHandler
    {
        private static readonly ConcurrentDictionary<int, WebSocket> _connectedClients = new ConcurrentDictionary<int, WebSocket>();

        public static async Task HandleWebSocketConnection(WebSocket webSocket, int transformerId)
        {
            // Store the WebSocket instance for the client
            _connectedClients.TryAdd(transformerId, webSocket);

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    byte[] buffer = new byte[1024];
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    // Handle the received message
                    await HandleReceivedMessage(transformerId, message);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
            }
            finally
            {
                // Remove the WebSocket instance when the client disconnects
                _connectedClients.TryRemove(transformerId, out _);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
            }
        }

        private static async Task HandleReceivedMessage(int transformerId, string message)
        {
            // Implement your logic to handle the received message
            // You can access other services or repositories as needed

            // Example: Send a response message back to the client
            string response = $"Received message: {message}";
            await SendMessageToClient(transformerId, response);
        }

        public static async Task SendMessageToClient(int transformerId, string message)
        {
            if (_connectedClients.TryGetValue(transformerId, out WebSocket webSocket))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
