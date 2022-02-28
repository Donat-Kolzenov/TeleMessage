using Prism.Commands;
using Prism.Mvvm;
using Core;
using Chat.DesktopClient.Services;

namespace Chat.DesktopClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private MessageService _messageService;

        private string _message;

        private string _username = "New user";

        private bool _isConnected;

        private readonly string _target = "";

        public string _output;

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Title { get; } = "Socket chat";

        public string Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }

        public DelegateCommand SendMessageCommand { get; set; }

        public DelegateCommand ConnectionCommand { get; set; }

        public DelegateCommand DisconnectionCommand { get; set; }

        public MainWindowViewModel()
        {
            SendMessageCommand = new DelegateCommand(SendMessage);
            ConnectionCommand = new DelegateCommand(Connected);
            DisconnectionCommand = new DelegateCommand(Disconnected);
        }

        private void SendMessage()
        {
            var message = new Message()
            {
                Text = $"{Username}: {_message}",
                Target = _target,
                Origin = Username
            };

            _messageService.Send(message);
        }

        private async void ReceiveMessage()
        {
            while (_messageService is not null)
            {
                Message result = await _messageService.Receive();
                string receiveMessage = result.Text;
                Output += $"\n{receiveMessage}";
            }
        }

        private void Connected()
        {
            IsConnected = true;
            _messageService = new MessageService();
            _message = $"{Username} was connected";
            SendMessage();
            ReceiveMessage();
        }

        private void Disconnected()
        {
            IsConnected = false;
            _message = $"{Username} was disconnected";
            SendMessage();
            _messageService.Dispose();
        }
    }
}
