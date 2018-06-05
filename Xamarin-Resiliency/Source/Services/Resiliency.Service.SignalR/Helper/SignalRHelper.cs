using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;

namespace Resiliency.Service.SignalR.Helper
{
    public class SignalRHelper
    {
        private IHubProxy _chat;
        private HubConnection _hubConnection;
        private string _hubName => "ChatHub";
        private string _eventName => "broadcastMessage";
        private string _url => "http://bananasvc.azurewebsites.net";

        public async Task OpenChannel()
        {
            _hubConnection = new HubConnection(_url);
            _chat = _hubConnection.CreateHubProxy(_hubName);
            //_chat.On<string, string>(_eventName, (s, s1) => { Console.Write(""); });

            await _hubConnection.Start();
        }

        public async Task SendSignalRMessage(string name, string message)
        {
            //await OpenChannel();
            await _chat.Invoke("Send", name, message);
        }
    }
}
