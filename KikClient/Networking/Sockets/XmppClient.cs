using KikClient.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

/**
 * Created by Moon on 1/13/2021
 * Wrapper for SSLClient which deals with kik xmpp events
 */

namespace KikClient.Networking.Sockets
{
    public class XmppClient
    {
        private SSLClient Socket { get; set; }
        private string Endpoint { get; set; }
        private int Port { get; set; }
        private string IncomingStanzaBuffer { get; set; }

        #region Events
        public event Func<string, Task> Acknowledgement;
        public event Func<long, bool, Task> Authenticated;
        public event Func<Task> DeAuthenticated;
        public event Func<string, XElement, Task> StanzaWithIdReceived;

        //<gjid> <ujid> <message> <timestamp> <element>
        public event Func<string, string, string, long, XElement, Task> MessageReceived;

        //<gjid> <ujid> <messageIds> <r> <d>
        public event Func<string, string, List<string>, bool, bool, XElement, Task> ReceiptReceived;

        public event Func<Task> RosterUpdateCommandReceived;

        //<element>
        //public event Func<XElement, Task> GroupUpdateInMessageReceived;

        //<gjid> <element>
        public event Func<string, XElement, Task> AddedToGroup;

        //<gjid> <element>
        public event Func<string, XElement, Task> RemovedFromGroup;

        //<gjid> <element>
        public event Func<string, XElement, Task> PromotedToAdmin;

        //<gjid> <element>
        public event Func<string, XElement, Task> DemotedFromAdmin;

        //<gjid> <ujid> <element>
        public event Func<string, string, XElement, Task> AddedOtherUserToGroup;

        //<gjid> <ujid> <element>
        public event Func<string, string, XElement, Task> RemovedOtherUserFromGroup;

        //<gjid> <ujid> <element>
        public event Func<string, string, XElement, Task> OtherUserJoinedGroup;

        //<gjid> <ujid> <element>
        public event Func<string, string, XElement, Task> OtherUserLeftGroup;

        //<gjid> <ujid> <element>
        public event Func<string, string, XElement, Task> OtherUserBannedFromGroup;

        //<gjid> <ujid> <element>
        public event Func<string, string, XElement, Task> OtherUserUnbannedFromGroup;

        //<gjid> <ujid> <element>
        public event Func<string, string, XElement, Task> OtherUserPromotedToAdmin;

        //<gjid> <ujid> <element>
        public event Func<string, string, XElement, Task> OtherUserDemotedFromAdmin;

        public event Func<Task> ServerConnected;
        public event Func<Task> ServerFailedToConnect;
        public event Func<Task> ServerDisconnected;
        #endregion Events

        public XmppClient(string endpoint, int port)
        {
            Endpoint = endpoint;
            Port = port;
        }

        #region Functions
        public async Task Start()
        {
            Socket = new SSLClient(Endpoint, Port);
            Socket.BytesReceived += Client_BytesReceived;
            Socket.ServerConnected += Client_ServerConnected;
            Socket.ServerFailedToConnect += Client_ServerFailedToConnect;
            Socket.ServerDisconnected += Client_ServerDisconnected;

            await Socket.Start();
        }

        public async Task Stop() => await Send("</k>");
        #endregion Functions

        #region Socekt Events
        private async Task Client_ServerDisconnected()
        {
            Socket.BytesReceived -= Client_BytesReceived;
            Socket.ServerConnected -= Client_ServerConnected;
            Socket.ServerFailedToConnect -= Client_ServerFailedToConnect;
            Socket.ServerDisconnected -= Client_ServerDisconnected;

            if (ServerDisconnected != null) await ServerDisconnected.Invoke();
        }

        private async Task Client_ServerFailedToConnect()
        {
            if (ServerFailedToConnect != null) await ServerFailedToConnect.Invoke();
        }

        private async Task Client_ServerConnected()
        {
            if (ServerConnected != null) await ServerConnected.Invoke();
        }

        //Temp error file counter
        private static int counter = 0;
        private async Task Client_BytesReceived(byte[] bytes)
        {
            Logger.ColoredLog($"< {Encoding.UTF8.GetString(bytes)}", ConsoleColor.Cyan);

            var stanza = Encoding.UTF8.GetString(bytes);
            try
            {
                IncomingStanzaBuffer += stanza;

                await Parse(IncomingStanzaBuffer);
                IncomingStanzaBuffer = string.Empty;
            }
            catch (XmlException e)
            {
                //We may actaully want to crash for other types of malformed xml
                if (!e.Message.Contains("Unexpected end of file") && !e.Message.Contains("There is an unclosed literal"))
                {
                    await File.WriteAllTextAsync($"{++counter}-xmpp-error.txt", $"{IncomingStanzaBuffer}\n\n{e.Message}");

                    Logger.Error(e.Message);
                    IncomingStanzaBuffer = string.Empty;
                }
            }
        }
        #endregion Socket Events

        #region Senders
        public async Task Send(string stanza)
        {
            Logger.ColoredLog($"> {stanza}", ConsoleColor.Magenta);
            await Socket.Send(Encoding.UTF8.GetBytes(stanza));
        }


        /// <summary>
        /// Waits for a response to the request with the designated ID, or if none is supplied, 
        /// any request at all. After which, it will unsubscribe from the event
        /// </summary>
        /// <param name="stanza">The stanza to send</param>
        /// <param name="onRecieved">A Function executed when a matching stanza is received. If no <paramref name="id"/> is provided, this will trigger on any stanza with an id.
        /// This Function should return a boolean indicating whether or not the request was satisfied. For example, if it returns True, the event subscription is cancelled and the timer
        /// destroyed, and no more messages will be parsed through the Function. If it returns false, it is assumed that the stanza was unsatisfactory, and the Function will continue to receive
        /// potential matches.</param>
        /// <param name="id">The id of the stanza to wait for. Optional. If none is provided, all stanzas with ids will be sent to <paramref name="onRecieved"/>.</param>
        /// <param name="onTimeout">A Function that executes in the event of a timeout. Optional.</param>
        /// <param name="timeout">Duration in milliseconds before the wait times out.</param>
        /// <returns></returns>
        public async Task SendAndAwaitResponse(string stanza, Func<XElement, Task<bool>> onRecieved, string id = null, Func<Task> onTimeout = null, int timeout = 5000)
        {
            Func<string, XElement, Task> receivedStanza = null;

            //TODO: I don't think Register awaits async callbacks 
            var cancellationTokenSource = new CancellationTokenSource();
            var registration = cancellationTokenSource.Token.Register(async () =>
            {
                StanzaWithIdReceived -= receivedStanza;
                if (onTimeout != null) await onTimeout.Invoke();

                cancellationTokenSource.Dispose();
            });

            receivedStanza = async (receivedId, element) =>
            {
                if (id == null || receivedId == id)
                {
                    if (await onRecieved(element))
                    {
                        StanzaWithIdReceived -= receivedStanza;

                        registration.Dispose();
                        cancellationTokenSource.Dispose();
                    };
                }
            };

            cancellationTokenSource.CancelAfter(timeout);
            StanzaWithIdReceived += receivedStanza;

            await Send(stanza);
        }
        #endregion Senders

        #region Parsers
        /**
         * Preparsing fixes up stanzas that might otherwise be seen as malformed,
         * for example the <k ok="1"> tags that indicate successful authentication
         */
        protected async Task<XElement> PreParse(string input)
        {
            if (input.StartsWith("<k ok=\""))
            {
                var kElement = new XElement("k");
                kElement.SetAttributeValue("ok", input.Substring(input.IndexOf("ok=\"") + 4, 1));
                if (input.Contains("ts")) kElement.SetAttributeValue("ts", input.Substring(input.IndexOf("ts=\"") + 4, "1610592732577".Length));
                return kElement;
            }
            else if (input.Equals("</k>"))
            {
                var kElement = new XElement("k");
                kElement.SetAttributeValue("end", "1"); //This is not part of the kik api, but rather a hack to let the parser further up the chain know that the connection has been severed
                return kElement;
            }
            else if (input == " ") //Kik occasionally sends one of these. Probably some sort of sneaky authenticity check. Send it right back.
            {
                await Send(" ");
                return new XElement("space");
            }

            //Avoid a known brick which replaces double quotes wiht singles
            //TODO: Remove this, or find a better way
            input = input.Replace("push=\"true''/><is-typing val=''false\"", "push=\"true\"/><is-typing val=\"false\"");

            return XElement.Parse(input);
        }

        /**
         * The parser takes in a string, and fires off the corresponding actions.
         * This suggests that parsing must happen through event subscription, but
         * it seems that a lot of it will end up happening as callbacks
         */
        protected async Task Parse(string input)
        {
            var element = await PreParse(input);

            //Parse acks, otherwise pass it up to the event handlers
            if (element.Name == "ack")
            {
                if (Acknowledgement != null) await Acknowledgement.Invoke(element.Attribute("id").Value);
            }
            else if (element.Attribute("id") != null)
            {
                if (StanzaWithIdReceived != null) await StanzaWithIdReceived.Invoke(element.Attribute("id").Value, element);
            }

            //Login, logout, k stanza parsing
            else if (element.Name == "k")
            {
                if (element.Attribute("ok") != null)
                {
                    if (long.TryParse(element.Attribute("ts")?.Value, out var ts))
                    {
                        if (Authenticated != null) await Authenticated.Invoke(ts, element.Attribute("ok").Value == "1");
                    }
                    else if (Authenticated != null) await Authenticated.Invoke(0, element.Attribute("ok").Value == "1");
                }
                else if (element.Attribute("end") != null)
                {
                    if (DeAuthenticated != null) await DeAuthenticated.Invoke();
                }
            }

            //Parse Messages and pass them up to the event handlers
            if (element.Name.LocalName == "message")
            {
                await ParseMessage(element);
            }
        }

        protected async Task ParseMessage(XElement input)
        {
            var type = input.Attribute("type").Value;
            var from = input.Attribute("from").Value;

            //DMs
            if (type == "chat")
            {
                var ts = input.ElementAnyNS("kik").Attribute("timestamp")?.Value; var body = input.ElementAnyNS("body")?.Value;

                //Not all messages have a body, or a timestamp.
                if (MessageReceived != null) await MessageReceived.Invoke(from, from, body?.UnescapeXml(), long.Parse(ts), input);
            }

            //Group removals and additions
            else if (type == "groupchat")
            {
                var sysmsg = input.ElementAnyNS("sysmsg")?.Value;
                var status = input.ElementAnyNS("status");
                var updatedGroup = input.ElementAnyNS("g");

                //Group updates come with an instruction to request a new roster
                if (input.ElementAnyNS("roster") != null)
                {
                    if (RosterUpdateCommandReceived != null) await RosterUpdateCommandReceived.Invoke();
                }

                //Update the group if we were sent group info in the response
                //TEMPORARILY DISABLED as I forgot most groupchat related messages carry a g...
                //if (updatedGroup != null && GroupUpdateInMessageReceived != null) await GroupUpdateInMessageReceived.Invoke(updatedGroup);

                //We have to check for is-typing as an element here because the group
                //namespace doesn't seem to use it as a type
                if (input.ElementAnyNS("is-typing") == null)
                {
                    var ts = input.ElementAnyNS("kik").Attribute("timestamp")?.Value;
                    var groupJid = input.ElementAnyNS("g").Attribute("jid").Value;
                    var body = input.ElementAnyNS("body")?.Value;

                    if (MessageReceived != null) await MessageReceived.Invoke(groupJid, from, body?.UnescapeXml(), long.Parse(ts), input);
                }

                //These can be null if someone toggles their ability to receive dms
                if (!string.IsNullOrEmpty(sysmsg))
                {
                    if (sysmsg.Contains("You have been removed from the group"))
                    {
                        if (RemovedFromGroup != null) await RemovedFromGroup.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, input);
                    }
                    else if (sysmsg.Contains("has added you to the chat"))
                    {
                        if (AddedToGroup != null) await AddedToGroup.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, input);
                    }
                    else if (sysmsg.Contains("You have been promoted to admin by"))
                    {
                        if (PromotedToAdmin != null) await PromotedToAdmin.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, input);
                    }
                    else if (sysmsg.Contains("Your admin status has been removed by"))
                    {
                        if (DemotedFromAdmin != null) await DemotedFromAdmin.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, input);
                    }
                }
                else if (status != null)
                {
                    if (status.Value.Contains("has removed") && status.Value.EndsWith("from this group"))
                    {
                        if (RemovedOtherUserFromGroup != null) await RemovedOtherUserFromGroup.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, status.Attribute("jid").Value, input);
                    }
                    else if (status.Value.Contains("has joined the chat, invited by"))
                    {
                        if (AddedOtherUserToGroup != null) await AddedOtherUserToGroup.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, status.Attribute("jid").Value, input);
                    }
                    else if (status.Value.Contains("has joined the chat"))
                    {
                        if (OtherUserJoinedGroup != null) await OtherUserJoinedGroup.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, status.Attribute("jid").Value, input);
                    }
                    else if (status.Value.Contains("has left the chat"))
                    {
                        if (OtherUserLeftGroup != null) await OtherUserLeftGroup.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, status.Attribute("jid").Value, input);
                    }
                    else if (status.Value.Contains("has banned"))
                    {
                        if (OtherUserBannedFromGroup != null) await OtherUserBannedFromGroup.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, status.Attribute("jid").Value, input);
                    }
                    else if (status.Value.Contains("has unbanned"))
                    {
                        if (OtherUserUnbannedFromGroup != null) await OtherUserUnbannedFromGroup.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, status.Attribute("jid").Value, input);
                    }
                    else if (status.Value.Contains("has promoted") && (status.Value.EndsWith("to admin") || status.Value.EndsWith("to owner")))
                    {
                        if (OtherUserPromotedToAdmin != null) await OtherUserPromotedToAdmin.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, status.Attribute("jid").Value, input);
                    }
                    else if (status.Value.Contains("has removed admin status from"))
                    {
                        if (OtherUserDemotedFromAdmin != null) await OtherUserDemotedFromAdmin.Invoke(input.ElementAnyNS("g").Attribute("jid").Value, status.Attribute("jid").Value, input);
                    }
                }
            }

            //Receipts
            else if (type == "receipt")
            {
                var receipt = input.ElementAnyNS("receipt");
                var group = input.ElementAnyNS("g");
                var messageIds = receipt.ElementsAnyNS("msgid").Select(x => x.Attribute("id").Value).ToList();

                var groupJid = group?.Attribute("jid").Value;
                var receiptType = receipt.Attribute("type").Value;

                if (ReceiptReceived != null) await ReceiptReceived.Invoke(groupJid ?? from, from, messageIds, receiptType == "read", receiptType == "delivered", input);
            }
        }
        #endregion Parsers
    }
}
