using System;
using System.Threading;
using System.Threading.Tasks;

/**
 * Created by Moon on 2/14/2021
 * Pings the kik servers on a fixed interval. This isn't exactly what kik does, but it keeps the connection alive
 */

namespace KikClient.Networking.Accessories
{
    class Pinger
    {
        private Client Client { get; set; }
        private CancellationTokenSource CancellationToken { get; set; }

        public Pinger(Client client)
        {
            Client = client;
            CancellationToken = new CancellationTokenSource();
        }

        internal async void Start()
        {
            while (!CancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Client.Send(StanzaGenerator.Ping());
                }
                catch
                {
                    CancellationToken.Cancel();
                }

                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(3), CancellationToken.Token);
                }
                catch (TaskCanceledException) { }
            }
        }

        internal void Stop()
        {
            CancellationToken.Cancel();
        }
    }
}
