using SnakeAndLadders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Services
{
    public interface INetworkManager : IDisposable
    {
        public event Action<byte[]> OnDataReceived;
        public event Action OnOtherPeerDisconnected;
        public Task StartReceiving();
        public Task SetupServer(Action<ConnectedClientInfo> onSomeoneConnect);
        public Task Connect(string ip, ushort port);
        public Task Send(byte[] data);
    }
}
