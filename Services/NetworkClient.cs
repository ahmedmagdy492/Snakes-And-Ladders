using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Services
{
    public class NetworkClient : IDisposable
    {
        private readonly Socket _socket;
        public event Action<byte[]> OnDataReceived;
        public event Action OnOtherPeerDisconnected;

        public NetworkClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task Connect(string ip, ushort port)
        {
            await _socket.ConnectAsync(ip, port);
        }

        private bool IsSocketConnected()
        {
            try
            {
                return !(_socket.Poll(1000, SelectMode.SelectRead) && _socket.Available == 0);
            }
            catch (SocketException)
            {
                return false; // Socket error means it's disconnected
            }
        }

        public async Task StartReceiving()
        {
            while (IsSocketConnected())
            {
                var buffer = new byte[4096];
                await _socket.ReceiveAsync(buffer);
                if (OnDataReceived != null)
                {
                    OnDataReceived(buffer);
                }
            }
            if (OnOtherPeerDisconnected != null)
            {
                OnOtherPeerDisconnected();
            }
        }

        public void Dispose()
        {
            _socket.Dispose();
        }
    }
}
