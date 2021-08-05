using KikClient.Database;
using KikClient.Utilities;
using KikClient.Networking;
using KikClient.Networking.Accessories;
using KikClient.Networking.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Xml.Linq;

/**
 * Created by Moon on 1/?/2021
 * Main class for C# bot client
 */

namespace KikClient
{
    public class Client : XmppClient
    {
        //Default bot admin
        public const string admin = "trainzboy_zzw@talk.kik.com";

        #region Identifying Information
        //Must always exist
        public string Username { get; private set; }
        public string DeviceId { get; private set; }
        public string AndroidId { get; private set; }
        
        //May be populated later from queries
        public string Jid { get; private set; }
        public string Password { get; private set; }
        public string Passkey { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Pic { get; private set; }

        //Kik version constants
        /*public const string versionName = "10.7.0.6811";
        public const string secretHash = "6l50fPQgb15e7uhGeRYF16egcqw=";
        public const string targetDomain = "talk1070an.kik.com";
        public const string versionName = "11.1.1.12218";
        public const string secretHash = "U2zo0mgN91SbESVhcMDdjkmQA04=";
        public const string targetDomain = "talk1110an.kik.com";
        public const string versionName = "11.26.0.16889";
        public const string secretHash = "av3vKJ1ADJyOeTxlN2+BgR7dNeA=";
        public const string targetDomain = "talk11260an.kik.com";*/
        public const string versionName = "14.1.2.11629";
        public const string secretHash = "ZTqNKTVMCbZb6AEfK/DCQHWXjvc=";
        public const string targetDomain = "talk1410an.kik.com";
        public const int targetPort = 5223;
        #endregion Identifying Information

        #region Events

        #endregion Events

        #region Options
        /*private BotDetector BotDetector { get; set; }

        private bool _enableBotDetector = false;
        public bool EnableBotDetector
        {
            get => _enableBotDetector;
            set
            {
                _enableBotDetector = value;
                if (value)
                {
                    OtherUserJoinedGroup += OnOtherUserJoinedGroup_BotDetector;
                }
                else
                {
                    OtherUserJoinedGroup -= OnOtherUserJoinedGroup_BotDetector;
                }
            }
        }*/

        //When enabled, the database is updated with each group member's last reading/talking activity
        private bool _enableActivityTracker = false;
        public bool EnableActivityTracker
        {
            get => _enableActivityTracker;
            set
            {
                _enableActivityTracker = value;
                if (value)
                {
                    MessageReceived += OnMessageReceived_ActivityTracker;
                    ReceiptReceived += OnReceiptReceived_ActivityTracker;
                }
                else
                {
                    MessageReceived -= OnMessageReceived_ActivityTracker;
                    ReceiptReceived -= OnReceiptReceived_ActivityTracker;
                }
            }
        }

        //When enabled, message history is requested and parsed on login
        public bool EnableMessageHistoryRetrival { get; set; } = false;

        //When enabled, messages are stored in our client database and available for retrival later
        public bool EnableMessageStorage { get; set; } = false;

        public bool ForceNextLoginFresh { get; set; } = false;
        #endregion Options

        //Configs and databases
        private Config Config { get; set; }
        private DatabaseService Database { get; set; }
        private Pinger Pinger { get; set; }

        #region Constructors
        public Client(string username, string password) : base(targetDomain, targetPort)
        {
            Username = username;
            Password = password;

            //Init config
            Config = new Config(username);
            Database = new DatabaseService($"{username}-database.db");
            if (string.IsNullOrEmpty(Config.GetString("username"))) Config.SaveString("username", username);
            if (string.IsNullOrEmpty(Config.GetString("deviceId"))) Config.SaveString("deviceId", KikUtilities.GenerateDeviceID());
            if (string.IsNullOrEmpty(Config.GetString("androidId"))) Config.SaveString("androidId", KikUtilities.GenerateAndroidID());

            DeviceId = Config.GetString("deviceId");
            AndroidId = Config.GetString("androidId");

            ServerConnected += Login;
            Authenticated += XmppClient_Authenticated;
            DeAuthenticated += XmppClient_DeAuthenticated;
            RosterUpdateCommandReceived += RequestUpdatedRoster;
            //GroupUpdateInMessageReceived += Database.ParseGroup;
        }

        public Client(string username) : base(targetDomain, targetPort)
        {
            Username = username;

            //Config is assumed to already exist for resuming a session
            Config = new Config(username);
            Database = new DatabaseService($"{username}-database.db");

            DeviceId = Config.GetString("deviceId");
            AndroidId = Config.GetString("androidId");
            Passkey = Config.GetString("passkey");
            FirstName = Config.GetString("first");
            LastName = Config.GetString("last");
            Email = Config.GetString("email");

            if (string.IsNullOrEmpty(Passkey) || string.IsNullOrEmpty(DeviceId) || string.IsNullOrEmpty(AndroidId))
            {
                throw new InvalidCredentialException("Cannot find an existing passkey/deviceId/androidId for the provided username. Be sure you've logged in previously.");
            }

            ServerConnected += Login;
            Authenticated += XmppClient_Authenticated;
            DeAuthenticated += XmppClient_DeAuthenticated;
            RosterUpdateCommandReceived += RequestUpdatedRoster;
            //GroupUpdateInMessageReceived += Database.ParseGroup;
        }
        #endregion Constructors

        #region Connection Events
        private async Task XmppClient_Authenticated(long timestamp, bool success)
        {
            if (success)
            {
                if (timestamp > 0)
                {
                    Logger.Success("Authenticated successfully!");

                    Pinger = new Pinger(this);
                    Pinger.Start();

                    await RequestUpdatedRoster();
                    if (EnableMessageHistoryRetrival)
                    {
                        await RequestMessageHistory();
                    }
                }
            }
            else
            {
                //Wipe credentials since they obviously didn't work
                Config.SaveString("jid", string.Empty);
                Config.SaveString("passkey", string.Empty);
                Logger.Error("Failed to authenticate! Is your password right? Did you log in from another device?");
            }
        }

        private Task XmppClient_DeAuthenticated()
        {
            Pinger.Stop();
            Logger.Error("Disconnected from Kik servers. Did you log in from another device?");
            return Task.CompletedTask;
        }
        #endregion Connection Events

        #region Message Event Handlers
        //Tacks people talking in groups
        private async Task OnMessageReceived_ActivityTracker(string groupJid, string userJid, string message, long timestamp, XElement element)
        {
            if (!string.IsNullOrEmpty(message)) await Database.UpdateGroupActivity(groupJid, userJid, DatabaseService.ActivityType.Talking);
        }

        //Tracks people reading messages in groups
        private async Task OnReceiptReceived_ActivityTracker(string groupJid, string userJid, List<string> messageIds, bool r, bool d, XElement element)
        {
            if (r)
            {
                await Database.UpdateGroupActivity(groupJid, userJid, DatabaseService.ActivityType.Reading);
            }
        }

        /*private async Task OnOtherUserJoinedGroup_BotDetector(string groupJid, string userJid, XElement element)
        {
            if (BotDetector == null) BotDetector = new BotDetector(this,
                async (groupJid, userJid) =>
                {
                    //Username lookup if possible
                    var displayJid = userJid;
                    if (Utilities.IsAlias(userJid)) displayJid = Database.AliasLookup(userJid);
                    displayJid = displayJid ?? userJid;

                    await Message(admin, $"Bot ({Utilities.GetUsernameFromJid(displayJid)}) has joined ({groupJid})");
                },
                async (groupJid, userJid) =>
                {
                    //Username lookup if possible
                    var displayJid = userJid;
                    if (Utilities.IsAlias(userJid)) displayJid = Database.AliasLookup(userJid);
                    displayJid = displayJid ?? userJid;

                    await Message(admin, $"Normal person ({Utilities.GetUsernameFromJid(displayJid)}) has joined ({groupJid})");
                });

            await Task.Delay(5000);
            await BotDetector.ParseDetector(groupJid, userJid);
        }*/
        #endregion Message Event Handlers

        #region Functions
        public async Task Message(string jid, string message, Dictionary<string, string> extraTags = null, string botmention = null)
        {
            await Send(StanzaGenerator.Message(jid, Jid, message, extraTags: extraTags, botmention: botmention).Item1);
        }

        /// <summary>
        /// Resolves an alias into a normal jid by "starting chatting" with the user.
        /// Does not "stop chatting" with the user.
        /// </summary>
        /// <param name="alias">Alias to resolve</param>
        /// <param name="groupJid">Group in which the alias was found</param>
        /// <param name="onResolved">Event fired when the alias is successfully resolved</param>
        /// <param name="onFailed">Event fired if the resolution fails</param>
        /// <returns></returns>
        public async Task ResolveAlias(string alias, string groupJid, Func<string, Task> onResolved = null, Func<Task> onFailed = null)
        {
            if (!alias.IsAlias())
            {
                if (onResolved != null) await onResolved.Invoke(alias);
            }
            else
            {
                //Check the database for previously resolved jids
                var resolvedJid = Database.AliasLookup(alias);

                //If the alias was not in the database, try to look it up
                if (string.IsNullOrEmpty(resolvedJid))
                {
                    var stanza = StanzaGenerator.StartChattingWithAlias(alias, groupJid);
                    await SendAndAwaitResponse(stanza.Item1, async (element) =>
                        {
                            var type = element.Attribute("type").Value;
                            var query = element.Element("{kik:iq:friend}query");

                            if (type == "result")
                            {
                                //Query results
                                var item = query.Element("{kik:iq:friend}item");
                                var jid = item.Attribute("jid").Value;
                                var displayName = item.Element("{kik:iq:friend}display-name").Value;

                                var picElement = item.Element("{kik:iq:friend}pic");
                                var profilePic = picElement?.Value ?? "none";
                                var profilePicLastUpdated = picElement?.Attribute("ts").Value ?? "0";

                                await Database.AddAlias(alias, jid, groupJid, displayName, profilePic, long.Parse(profilePicLastUpdated));
                                if (onResolved != null) await onResolved.Invoke(jid);
                            }
                            else if (type == "error")
                            {
                                Logger.Error("Error resolving alias:\n");
                                Logger.Error(element.ToString());
                                if (onFailed != null) await onFailed.Invoke();
                            }
                            return true;
                        },
                        stanza.Item2,
                        onFailed
                    );
                }
                else if (onResolved != null) await onResolved.Invoke(resolvedJid);
            }
        }

        /// <summary>
        /// Requests updated roster info for the current account
        /// Calls itself recursively until there is no new roster information
        /// </summary>
        /// <returns></returns>
        public async Task RequestUpdatedRoster()
        {
            if (string.IsNullOrEmpty(Config.GetString("lastRosterTimestamp"))) Config.SaveString("lastRosterTimestamp", "0");
            var lastRosterTimestamp = long.Parse(Config.GetString("lastRosterTimestamp"));

            var rosterRequestStanza = StanzaGenerator.RequestRoster(lastRosterTimestamp);
            await SendAndAwaitResponse(rosterRequestStanza.Item1, async (element) =>
                {
                    var query = element.Element("{jabber:iq:roster}query");
                    var ts = query.Attribute("ts").Value;
                    var more = query.Attribute("more")?.Value == "1";

                    if (!string.IsNullOrEmpty(ts)) Config.SaveString("lastRosterTimestamp", ts);

                    foreach (var child in query.Elements())
                    {
                        if (child.Name == "{jabber:iq:roster}item") await Database.ParseItem(child, false);
                        else if (child.Name == "{jabber:iq:roster}g") await Database.ParseGroup(child, false);
                    }

                    await Database.DatabaseContext.SaveChangesAsync();

                    if (more) await RequestUpdatedRoster();
                    return true;
                },
                rosterRequestStanza.Item2);
        }

        /// <summary>
        /// Requests the message history for the current account
        /// </summary>
        /// <param name="onHistoryChunkReceived">Event fired when the history is received. Parameters: (number of messages in history)</param>
        /// <returns></returns>
        public async Task RequestMessageHistory(Func<int, Task> onHistoryChunkReceived = null, Func<Task> onTimeout = null) => await AckMessageHistory(requestMore: true, onHistoryChunkReceived: onHistoryChunkReceived, onTimeout: onTimeout);

        /// <summary>
        /// Acknowledges all messages given in the provided arguments, and requests more history if desired
        /// Calls itself recursively until there is no new history
        /// </summary>
        /// <param name="messageInfo">List of all messages to ack. Format: (groupJid, userJid, messageId)</param>
        /// <param name="requestMore">Whether or not to see more history after the messages have been acked</param>
        /// <param name="onHistoryChunkReceived">Event fired when the history is received. Parameters: (number of messages in history)</param>
        /// <returns></returns>
        private async Task AckMessageHistory(List<(string, string, string)> messageInfo = null, bool requestMore = false, Func<int, Task> onHistoryChunkReceived = null, Func<Task> onTimeout = null)
        {
            var historyRequestStanza = StanzaGenerator.SendMessageHistoryAcks(messageInfo, requestMore);
            await SendAndAwaitResponse(historyRequestStanza.Item1, async (element) =>
            {
                var query = element.Element("{kik:iq:QoS}query");
                var history = query.Element("{kik:iq:QoS}history");

                //Fire off the event with 0 results if there's no history
                if (history == null && onHistoryChunkReceived != null)
                {
                    await onHistoryChunkReceived.Invoke(0);
                }

                //If history is null, the server has no more history for us to see. Do nothing.
                if (history != null)
                {
                    var more = history.Attribute("more")?.Value == "1";
                    var msgs = history.Elements("{kik:iq:QoS}msg");

                    //<groupJid> <userJid> <msgId>
                    var ackList = new List<(string, string, string)>();

                    foreach (var msg in msgs)
                    {
                        await ParseMessage(msg);

                        //Build list of info to ack
                        var groupJid = msg.Element("{kik:iq:QoS}g")?.Attribute("jid").Value;

                        //Workaround for known brick attempt
                        //TODO: REMOVE. Doesn't seem to work. *shrug*
                        groupJid = groupJid ?? msg.Element("{kik:iq:QoS}flag").Element("{kik:iq:QoS}g")?.Attribute("jid").Value;

                        ackList.Add((groupJid, msg.Attribute("from").Value, msg.Attribute("id").Value));
                    }

                    //Fire off the event with the proper parameters now that we can
                    if (onHistoryChunkReceived != null)
                    {
                        await onHistoryChunkReceived.Invoke(msgs.Count());
                    }

                    //Recursively call ourselves again to ack this set of messages and wait for the next
                    await AckMessageHistory(ackList, more, onHistoryChunkReceived);
                }
                return true;
            },
            historyRequestStanza.Item2,
            onTimeout);
        }

        /// <summary>
        /// Removes the provided list of users from the provided group.
        /// Be wary of rate limit errors.
        /// </summary>
        /// <param name="groupJid">The group to remove the users from</param>
        /// <param name="userJids">The users to remove</param>
        /// <returns></returns>
        public async Task RemoveUsersFromGroup(string groupJid, params string[] userJids)
        {
            //Recursively call this funcion until there are no more users left in the list
            var removeStanza = StanzaGenerator.RemoveFromGroup(groupJid, userJids.First());
            await SendAndAwaitResponse(removeStanza.Item1, async (element) =>
            {
                var remainingUsers = userJids.Except(new string[] { userJids.First() }).ToArray();
                if (remainingUsers.Length > 0) await RemoveUsersFromGroup(groupJid, remainingUsers);
                return true;
            }, removeStanza.Item2);
        }

        private async Task Login()
        {
            Jid = Config.GetString("jid");
            Passkey = Config.GetString("passkey");

            async Task resumeLogin(string jid)
            {
                await Send(
                    KikUtilities.BuildStringFromKStanzaDictionary(
                        StanzaGenerator.LoginStanza(
                            jid.Substring(0, jid.IndexOf("@")),
                            DeviceId,
                            Passkey,
                            secretHash,
                            versionName
                        )
                    )
                );
            }

            async Task onAnonLogin(long _, bool succeeded)
            {
                Authenticated -= onAnonLogin;

                var profileQueryStanza = StanzaGenerator.RegisterQuery(Username, Password, versionName, DeviceId, AndroidId);
                await SendAndAwaitResponse(profileQueryStanza.Item1, async (element) =>
                {
                    var type = element.Attribute("type").Value;
                    var query = element.Element("{jabber:iq:register}query");

                    if (type == "result")
                    {
                        //Login results
                        var jidPrefix = query.Element("{jabber:iq:register}node").Value;

                        //Since we know this is a correct passkey, we can save it
                        //and the resulting jid
                        Jid = jidPrefix + "@talk.kik.com";
                        Config.SaveString("jid", Jid);

                        Passkey = KikUtilities.HashPassword(Username, Password);
                        Config.SaveString("passkey", Passkey);

                        Email = query.Element("{jabber:iq:register}email").Value;
                        Config.SaveString("email", Email);

                        FirstName = query.Element("{jabber:iq:register}first").Value;
                        Config.SaveString("first", FirstName);

                        LastName = query.Element("{jabber:iq:register}last").Value;
                        Config.SaveString("last", LastName);

                        //We will now disconnect. Schedule a reconnection, but only once
                        async Task reconnect()
                        {
                            //Logger.Debug(Environment.StackTrace);

                            ServerDisconnected -= reconnect;
                            await Start();
                        };
                        ServerDisconnected += reconnect;
                    }
                    else if (type == "error")
                    {
                        Logger.Error("Error logging in:\n");
                        Logger.Error(element.ToString());
                    }

                    await Stop();
                    return true;
                },
                profileQueryStanza.Item2,
                async () =>
                {
                    Logger.Error("Profile info query timed out! Aborting login.");
                    await Stop();
                });
            }

            async Task anonLogin()
            {
                Authenticated += onAnonLogin;

                await Send(
                    KikUtilities.BuildStringFromKStanzaDictionary(
                        StanzaGenerator.AnonymousLoginStanza(
                            DeviceId,
                            secretHash,
                            versionName
                        )
                    )
                );
            }

            if (!string.IsNullOrEmpty(Jid) && !string.IsNullOrEmpty(Passkey) && !ForceNextLoginFresh)
            {
                await resumeLogin(Jid);
            }
            else if (!string.IsNullOrEmpty(Password))
            {
                ForceNextLoginFresh = false;
                await anonLogin();
            }
        }

        public void RegisterCommandService(CommandService.CommandService commandService)
        {
            MessageReceived += commandService.ParseMessage;
        }

        public void UnregisterCommandService(CommandService.CommandService commandService)
        {
            MessageReceived -= commandService.ParseMessage;
        }
        #endregion Functions

        #region Contact Handling
        public User GetUser(string userJid) => Database.GetUser(userJid);

        public User GetUserByUsername(string username) => Database.GetUserByUsername(username);

        public Group GetGroup(string groupJid) => Database.GetGroup(groupJid);

        public Group GetGroupByTag(string groupTag) => Database.GetGroupByTag(groupTag);

        public List<GroupMember> GetGroupMembers(string groupJid) => Database.GetGroupMembers(groupJid);

        /// <summary>
        /// Checks the admin status of a user in a group
        /// </summary>
        /// <param name="groupJid">The group to check the user's status in</param>
        /// <param name="userJid">The user to check the status of</param>
        /// <returns>Returns true if the user is an admin in the provided group. False if the groupJid is not a group, or if the user is not an admin or member of said group</returns>
        public bool IsAdmin(string groupJid, string userJid)
        {
            if (groupJid.GetUsernameFromJid() != "GROUP") return false;
            return GetGroupMembers(groupJid).Any(x => x.Role == Role.Admin || x.Role == Role.Owner && x.UserJid == userJid) || userJid ==  admin;
        }
        #endregion Contact Handling
    }
}
