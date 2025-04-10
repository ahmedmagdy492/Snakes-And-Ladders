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
    public enum NetworkMode
    {
        Client,
        Server
    }

    public class NetworkManager : INetworkManager
    {
        private readonly Socket _serverSocket;
        private readonly NetworkMode _networkMode;
        private Socket _clientSocket = null;

        public event Action<byte[]> OnDataReceived;
        public event Action OnOtherPeerDisconnected;

        public NetworkManager(NetworkMode networkMode)
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _networkMode = networkMode;
        }

        public async Task SetupServer(Action<ConnectedClientInfo> onSomeoneConnect)
        {
            _serverSocket.Bind(new IPEndPoint(new IPAddress([0, 0, 0, 0]), Constants.SERVER_PORT));
            _serverSocket.Listen(2);
            _clientSocket = await _serverSocket.AcceptAsync();
            onSomeoneConnect(new ConnectedClientInfo { IPAddress = _clientSocket.RemoteEndPoint.ToString() });
            await Task.Run(StartReceiving);
        }

        public async Task Connect(string ip, ushort port)
        {
            await _serverSocket.ConnectAsync(ip, port);
        }

        private bool IsClientConnected()
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

        private bool IsServerSocketConnected()
        {
            try
            {
                return !(_serverSocket != null && _serverSocket.Poll(1000, SelectMode.SelectRead) && _serverSocket.Available == 0);
            }
            catch (SocketException)
            {
                return false; // Socket error means it's disconnected
            }
        }

        public async Task StartReceiving()
        {
            if (_networkMode == NetworkMode.Server)
            {
                while (IsClientConnected())
                {
                    var buffer = new byte[4096];
                    await _clientSocket.ReceiveAsync(buffer);
                    if (OnDataReceived != null)
                    {
                        OnDataReceived(buffer);
                    }
                }
            }
            else if (_networkMode == NetworkMode.Client)
            {
                while (IsServerSocketConnected())
                {
                    var buffer = new byte[4096];
                    await _serverSocket.ReceiveAsync(buffer);
                    if (OnDataReceived != null)
                    {
                        OnDataReceived(buffer);
                    }
                }
            }
            if (OnOtherPeerDisconnected != null)
            {
                OnOtherPeerDisconnected();
            }
        }

        public async Task Send(byte[] data)
        {
            if(_networkMode == NetworkMode.Server)
            {
                if (_clientSocket != null)
                {
                    await _clientSocket.SendAsync(data);
                }
            }
            else
            {
                if (_serverSocket != null)
                {
                    await _serverSocket.SendAsync(data);
                }
            }
        }

        public void Dispose()
        {
            _serverSocket.Dispose();
            if(_clientSocket != null)
            {
                _clientSocket.Dispose();
            }
        }
    }
}
