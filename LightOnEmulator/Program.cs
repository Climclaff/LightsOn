using System.Net.WebSockets;
using System.Text;

namespace LightOnEmulator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (ClientWebSocket webSocket = new ClientWebSocket())
            {
                Thread.Sleep(6000);
                Uri serverUri = new Uri("wss://localhost:7014/ws?id=1");
                
                try
                {
                    await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                    Console.WriteLine("WebSocket connected.");

                    string message = "Test";
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    // Handle the WebSocket connection
                    await HandleWebSocketConnection(webSocket);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"WebSocket error: {ex.Message}");
                }
                
            }
        }

        static async Task HandleWebSocketConnection(ClientWebSocket webSocket)
        {
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    byte[] buffer = new byte[1024];
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    
                    // Handle the received message
                    Console.WriteLine($"Received message: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
            }
        }

    }
}