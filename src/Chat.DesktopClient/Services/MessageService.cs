using System;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using Core;
using Chat.DesktopClient.Managers;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Chat.DesktopClient.Services
{
    class MessageService : IDisposable
    {
        private const string API = "message";

        private bool disposed;

        private readonly ConnectionManager _connectionManager;

        public MessageService()
        {
            _connectionManager = new ConnectionManager(API);
            _ = _connectionManager.StartConnection();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                if (disposing)
                    _ = _connectionManager.EndConnection();
            }
        }

        ~MessageService() => Dispose(false);

        public void Send(Message message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            var bytes = Encoding.UTF8.GetBytes(jsonMessage);
            _connectionManager.Client.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }

        public async Task<Message> Receive()
        {
            var buffer = new byte[1024 * 4];

            var result = await _connectionManager.Client.ReceiveAsync(
                new ArraySegment<byte>(buffer),
                CancellationToken.None);

            string message = Encoding.UTF8.GetString(
                buffer,
                0,
                result.Count);

            return JsonConvert.DeserializeObject<Message>(message);
        }
    }
}
