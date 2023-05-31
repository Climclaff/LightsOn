using LightOn.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LightOnEmulator
{
    public class TransformerMeasurement
    {
        public int Id { get; set; }

        public float CurrentLoad { get; set; }

        public DateTime Date { get; set; }

        public int? TransformerId { get; set; }

    }
    public class Transformer
    {
        public int Id { get; set; }

        public int MaxLoad { get; set; }
    }
        class Program
    {
        /*
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
                            TransformerMeasurement measurement = new TransformerMeasurement();
                            var buffer = new byte[256];
                            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                            if (result.EndOfMessage)
                            {
                                Dictionary<int, float> receivedValue = DeserializePair(buffer, result.Count);


                                measurement.CurrentLoad = receivedValue.Values.FirstOrDefault();
                                measurement.TransformerId = receivedValue.Keys.FirstOrDefault();
                                measurement.Date = DateTime.Now;

                                var seralizedMeasurement = SerializeMeasurement(measurement);
                                await webSocket.SendAsync(new ArraySegment<byte>(seralizedMeasurement), WebSocketMessageType.Text, true, CancellationToken.None);

                            }
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
                static Dictionary<int, float> DeserializePair(byte[] buffer, int count)
        {
                    string json = Encoding.UTF8.GetString(buffer, 0, count);
                    return JsonSerializer.Deserialize<Dictionary<int, float>>(json);
                }

                static byte[] SerializeMeasurement(TransformerMeasurement measurement)
                {
                    string json = JsonSerializer.Serialize(measurement);
                    byte[] buffer = Encoding.UTF8.GetBytes(json);
                    return buffer;
                }
        */

        static async Task Main(string[] args)
        {
            using (ClientWebSocket webSocket = new ClientWebSocket())
            {

                Thread.Sleep(6000);
                List<Transformer> transformers= new List<Transformer>();
                transformers.Add(new Transformer { Id = 1, MaxLoad = 20});
                transformers.Add(new Transformer { Id = 2, MaxLoad = 20 });
                transformers.Add(new Transformer { Id = 3, MaxLoad = 25 });


                // Run multiple instances of HandleWebSocketConnection concurrently
                await Task.WhenAll(transformers.Select(HandleWebSocketConnection));

            }

            static async Task HandleWebSocketConnection(Transformer transformer)
            {
                using (ClientWebSocket webSocket = new ClientWebSocket())
                {
                    Uri serverUri = new Uri($"wss://localhost:7014/ws?id={transformer.Id}");
                    try
                    {
                        await webSocket.ConnectAsync(serverUri, CancellationToken.None);
                        Console.WriteLine($"Transformer {transformer.Id} connected.");
                        while (webSocket.State == WebSocketState.Open)
                        {
                            TransformerMeasurement measurement = new TransformerMeasurement();
                            var buffer = new byte[256];
                            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                            if (result.EndOfMessage)
                            {
                                Dictionary<int, float> receivedValue = DeserializePair(buffer, result.Count);


                                measurement.CurrentLoad = receivedValue.Values.FirstOrDefault();
                                measurement.TransformerId = receivedValue.Keys.FirstOrDefault();
                                measurement.Date = DateTime.Now;

                                var seralizedMeasurement = SerializeMeasurement(measurement);
                                await webSocket.SendAsync(new ArraySegment<byte>(seralizedMeasurement), WebSocketMessageType.Text, true, CancellationToken.None);

                            }
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
            static Dictionary<int, float> DeserializePair(byte[] buffer, int count)
            {
                string json = Encoding.UTF8.GetString(buffer, 0, count);
                return JsonSerializer.Deserialize<Dictionary<int, float>>(json);
            }

            static byte[] SerializeMeasurement(TransformerMeasurement measurement)
            {
                string json = JsonSerializer.Serialize(measurement);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                return buffer;
            }
        }
    }
}