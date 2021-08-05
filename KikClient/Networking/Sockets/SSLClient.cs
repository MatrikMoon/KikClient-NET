using KikClient.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace KikClient.Networking.Sockets
{
    public class ClientPlayer
    {
        public Socket socket = null;
        public SslStream sslStream = null;
        public const int BufferSize = 8192;
        public byte[] buffer = new byte[BufferSize];
        public List<byte> accumulatedBytes = new List<byte>();
    }

    public class SSLClient
    {
        public event Func<byte[], Task> BytesReceived;
        public event Func<Task> ServerConnected;
        public event Func<Task> ServerFailedToConnect;
        public event Func<Task> ServerDisconnected;

        private int port;
        private string endpoint;
        private ClientPlayer player;

        public bool Connected
        {
            get
            {
                return player?.socket?.Connected ?? false;
            }
        }

        public SSLClient(string endpoint, int port)
        {
            this.endpoint = endpoint;
            this.port = port;

            player = new ClientPlayer();
        }

        public async Task Start()
        {
            if (!IPAddress.TryParse(endpoint, out var ipAddress))
            {
                //If we want to default to ipv4, we should uncomment the following line. I'm leaving it
                //as it is now so we can test ipv6/ipv4 mix stability
                //IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                IPHostEntry ipHostInfo = Dns.GetHostEntry(endpoint);
                ipAddress = ipHostInfo.AddressList[0];
            }

            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            player.socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //Try to connect to the server
                await player.socket.ConnectAsync(remoteEP);
                var client = player.socket;

                //Try to authenticate with SSL
                player.sslStream = new SslStream(new NetworkStream(client));
                await player.sslStream.AuthenticateAsClientAsync(endpoint);

                //Signal Connection complete
                if (ServerConnected != null) await ServerConnected.Invoke();
            }
            catch (Exception e)
            {
                Logger.Debug(e.ToString());

                if (ServerFailedToConnect != null) await ServerFailedToConnect.Invoke();
            }

            ReceiveLoop();
        }

        private async void ReceiveLoop()
        {
            try
            {
                // Begin receiving the data from the remote device.  
                while (Connected)
                {
                    var bytesRead = await player.sslStream.ReadAsync(player.buffer, 0, ClientPlayer.BufferSize);
                    if (bytesRead > 0)
                    {
                        var currentBytes = new byte[bytesRead];
                        Buffer.BlockCopy(player.buffer, 0, currentBytes, 0, bytesRead);

                        if (BytesReceived != null) await BytesReceived.Invoke(currentBytes);
                    }
                    else if (bytesRead == 0) throw new Exception("Stream ended");
                }
            }
            catch (ObjectDisposedException)
            {
                await ServerDisconnected_Internal();
            }
            catch (Exception e)
            {
                Logger.Debug(e.ToString());
                await ServerDisconnected_Internal();
            }
        }

        public async Task Send(byte[] data)
        {
            try
            {
                await player.sslStream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Logger.Debug(e.ToString());
                await ServerDisconnected_Internal();
            }
        }

        private async Task ServerDisconnected_Internal()
        {
            await Shutdown();
            if (ServerDisconnected != null) await ServerDisconnected.Invoke();
        }

        public async Task Shutdown()
        {
            await player.sslStream.DisposeAsync();
        }
    }
}