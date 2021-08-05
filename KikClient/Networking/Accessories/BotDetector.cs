using System;
using System.Threading.Tasks;

/**
 * Created by Moon on 12/19/2017
 * Translated to C# on 1/22/2021
 * If messages are thrown through here, every person
 * who joins a chat will be vetted to be sure
 * they're not a sex bot
 */

namespace KikClient.Networking.Accessories
{
    class BotDetector
    {
        //TODO: Reliable jid resolution, followed by this, should work
        /*
        //<groupJid> <userJid>
        private Func<string, string, Task> OnBotDetected;

        //<groupJid> <userJid>
        private Func<string, string, Task> OnBotNotDetected;

        private Client Client { get; set; }

        public BotDetector(Client client, Func<string, string, Task> onBotDetected, Func<string, string, Task> onBotNotDetected = null)
        {
            Client = client;
            OnBotDetected = onBotDetected;
            OnBotNotDetected = onBotNotDetected;
        }

        //Should be called on group join
        public async Task ParseDetector(string groupJid, string userJid)
        {
            //This stanza both requests a response, and pings the user at the same time
            var stanza = StanzaGenerator.PrepareXMPP(
                "<message type=\"chat\" to=\"" + userJid + "\" from=\"" + Client.Jid + "\" id=\"$id\" cts=\"$ts\">" +
                "<ping/>" +
                "<kik push=\"true\" qos=\"true\" timestamp=\"$ts\" />" +
                "<request xmlns=\"kik:message:receipt\" r=\"true\" d=\"true\"/>" +
                "</message>");

            await Client.SendAndAwaitResponse(stanza.Item1, async (element) =>
            {
                if (OnBotNotDetected != null) await OnBotNotDetected(groupJid, userJid);
            },
            stanza.Item2,
            async () =>
            {
                await OnBotDetected(groupJid, userJid);
            });
        }*/
    }
}
