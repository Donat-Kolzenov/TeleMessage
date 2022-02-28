using System;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;

namespace Chat.DesktopClient.Managers
{
    class ConnectionManager
    {
        private readonly string _api;

        private const string HOST = "localhost:5000";

        public ClientWebSocket Client { get; private set; }

        public ConnectionManager(string api)
        {
            Client = new ClientWebSocket();
            _api = api;
        }

        public async Task StartConnection()
        {
            await Client.ConnectAsync(
                new Uri($"ws://{HOST}/{_api}"),
                CancellationToken.None);
        }

        public async Task EndConnection()
        {
            await Client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
            /*await Client.SendAsync(
                null,
                WebSocketMessageType.Close,
                true,
                CancellationToken.None);*/
        }
    }
}
