using SnakeAndLadders.Helpers;
using SnakeAndLadders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Services
{
    public class NetworkServer : IDisposable
    {
        private readonly Socket _socket;
        private Socket _clientSocket = null;

        public event Action<byte[]> OnDataReceived;
        public event Action OnOtherPeerDisconnected;

        public NetworkServer()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task SetupServer(Action<ConnectedClientInfo> onSomeoneConnect)
        {
            _socket.Bind(new IPEndPoint(new IPAddress([0, 0, 0, 0]), Constants.SERVER_PORT));
            _socket.Listen(2);
            _clientSocket = await _socket.AcceptAsync();
            onSomeoneConnect(new ConnectedClientInfo { IPAddress = _clientSocket.RemoteEndPoint.ToString() });
            await Task.Run(StartReceiving);
        }

        private bool IsSocketConnected()
        {
            try
            {
                return !(_clientSocket != null && _clientSocket.Poll(1000, SelectMode.SelectRead) && _clientSocket.Available == 0);
            }
            catch (SocketException)
            {
                return false; // Socket error means it's disconnected
            }
        }

        private async Task StartReceiving()
        {
            while(IsSocketConnected())
            {
                var buffer = new byte[4096];
                await _clientSocket.ReceiveAsync(buffer);
                if(OnDataReceived != null)
                {
                    OnDataReceived(buffer);
                }
            }
            if(OnOtherPeerDisconnected != null)
            {
                OnOtherPeerDisconnected();
            }
        }

        public async Task Send(byte[] data)
        {
            if(_clientSocket != null)
            {
                await _clientSocket.SendAsync(data);
            }
        }

        public void Dispose()
        {
            _socket.Dispose();
            if(_clientSocket != null)
            {
                _clientSocket.Dispose();
            }
        }
    }
}
