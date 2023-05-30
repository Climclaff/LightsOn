using LightOn.Data;
using LightOn.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace LightOn.Websockets
{
    public class WebSocketHandler
    {


        private static readonly ConcurrentDictionary<int, WebSocket> _connectedClients = new ConcurrentDictionary<int, WebSocket>();

        public static async Task HandleWebSocketConnection(WebSocket webSocket, int transformerId, ApplicationDbContext context)
        {
            // Store the WebSocket instance for the client
            _connectedClients.TryAdd(transformerId, webSocket);

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    byte[] buffer = new byte[1024];
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var measurement = JsonSerializer.Deserialize<TransformerMeasurement>(json);
                        await context.TransformerMeasurements.AddAsync(measurement);
                        await context.SaveChangesAsync();

                    }
                        
                }
            }
            catch (Exception)
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


        public static async Task SendMessageToClient(int transformerId, string message)
        {
            if (_connectedClients.TryGetValue(transformerId, out WebSocket webSocket))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        public static async Task SendTransformerLoadToClient(int id, byte[] encodedMessage)
        {
            if (_connectedClients.TryGetValue(id, out WebSocket webSocket))
            {
                try
                {
                    await webSocket.SendAsync(new ArraySegment<byte>(encodedMessage), WebSocketMessageType.Binary, true, CancellationToken.None);
                }
                catch (Exception)
                {
                    throw new Exception($"error during sending simulation data for transformer with id {id}");
                }
            }
           
        }
    }
}
