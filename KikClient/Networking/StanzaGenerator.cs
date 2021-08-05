using Google.Protobuf;
using KikClient.Networking.Images;
using KikClient.Utilities;
using Mobile.Profile.V1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

/**
 * Translated by Moon on 1/?/2020
 * This class is designed to generate the various stanzas we'll need to communicate with kik servers.
 * Returns `Dictionary<string, string>`s which can be fed through the stanza builder in Utilities
 * or tuples that are of format (xmpp, uuid)
 */

namespace KikClient.Networking
{
    public static class StanzaGenerator
    {
        /// <summary>
        /// Sends a message to a user or group
        /// </summary>
        /// <param name="to">The destination jid for the message</param>
        /// <param name="from">The jid the message is from. Cannot be spoofed easily</param>
        /// <param name="message">The body of the message</param>
        /// <param name="extraTags">If you have any custom tags you want to add to the message, add them here</param>
        /// <param name="botmention">If you want to mention a bot, its username goes here</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) Message(string to, string from = null, string message = null, string timestamp = null, Dictionary<string, string> extraTags = null, string botmention = null)
        {
            var isGroup = to.GetUsernameFromJid() == "GROUP";

            //Escape html
            message = message?.EscapeXml();

            timestamp = timestamp ?? "$ts";

            var xmpp = "<message " +
                        "type=\"" + (isGroup ? "groupchat" : "chat") + "\" " +
                        (isGroup ? "xmlns=\"kik:groups\" " : "") +
                        "to=\"" + to + "\" " +
                        (string.IsNullOrEmpty(from) ? "" : "from=\"" + from + "\" ") +
                        "id=\"$id\" " +
                        $"cts=\"{timestamp}\">" +
                        (message != null ? $"<body>{message}</body>" : "") +
                        (message != null ? $"<preview>{message}</preview>" : "");
            if (!string.IsNullOrEmpty(botmention))
            {
                xmpp += "<mention><bot>" + botmention + "</bot></mention>";
            }
            if (extraTags != null)
            {
                foreach (var pair in extraTags)
                {
                    xmpp += "<" + pair.Key + ">" +
                            pair.Value +
                            "</" + pair.Key + ">";
                }
            }
            xmpp += "<kik push=\"true\" qos=\"true\" " +
                    $"timestamp=\"{timestamp}\" />" +
                    "<request xmlns=\"kik:message:receipt\" r=\"true\" d=\"true\"/>" +
                    "</message>";
            return PrepareXMPP(xmpp);
        }

        /// <summary>
        /// Given a text string, generates a text image and sends it as a sticker
        /// </summary>
        /// <param name="text">The text to generate a sticker of</param>
        /// <param name="to">The message destination</param>
        /// <param name="from">The client jid, if applicable</param>
        /// <param name="fontSize">Optionally, designate the font size</param>
        /// <param name="foregroundColor">Optionally, designate the text color</param>
        /// <param name="backgroundColor">Optionally, designate the background color</param>
        /// <returns></returns>
        public static (string, string) Sticker(string text, string to, string from = null, int fontSize = 12, Color foregroundColor = default(Color), Color backgroundColor = default(Color))
        {
            return Sticker(
                ImageGenerator.DrawText(
                    text,
                    new Font(FontFamily.GenericSansSerif, fontSize),
                    foregroundColor == default(Color) ? Color.Blue : foregroundColor,
                    foregroundColor == default(Color) ? Color.Transparent : backgroundColor
                ),
                to,
                from
            );
        }

        /// <summary>
        /// Given basic message information and an array of bytes, sends a sticker using the byte array as the image
        /// </summary>
        /// <param name="png">The image to send</param>
        /// <param name="to">The message destination</param>
        /// <param name="from">The client jid, if applicable</param>
        /// <returns></returns>
        public static (string, string) Sticker(byte[] png, string to, string from = null)
        {
            //Set up message portion, varying based on destination type
            var isGroup = to.GetUsernameFromJid() == "GROUP";

            var xmpp = "<message " +
                        "type=\"" + (isGroup ? "groupchat" : "chat") + "\" " +
                        (isGroup ? "xmlns=\"kik:groups\" " : "") +
                        "to=\"" + to + "\" " +
                        (string.IsNullOrEmpty(from) ? "" : "from=\"" + from + "\" ") +
                        "id=\"$id\" " +
                        "cts=\"$ts\">";

            //Add kik-y bits
            xmpp += "<kik push=\"true\" qos=\"true\" timestamp=\"$ts\" /><request xmlns=\"kik:message:receipt\" r=\"true\" d=\"true\" />";

            //Add content tag opening and strings section -- Random uuid is specific to content tag, not related to message id
            xmpp += "<content app-id=\"com.kik.ext.stickers\" id=\"" + KikUtilities.GenerateKikUUID() + "\" v=\"2\">" +
                    "<strings>" +
                    "<app-name>Moon's Bot Stickers</app-name>" +
                    "<disallow-save>false</disallow-save>" +
                    "<allow-forward>true</allow-forward>" +
                    "<video-should-loop>false</video-should-loop>" +
                    "<video-should-autoplay>false</video-should-autoplay>" +
                    "<layout>photo</layout>" +
                    "<video-should-be-muted>false</video-should-be-muted>" +
                    "<card-icon>https://stickers.kik.com/icon-square.png?v=5</card-icon>" +
                    "</strings>";

            //Add extras section
            xmpp += "<extras>" +
                    //"<item><key>packId</key><val>Nothing</val></item>" +
                    //"<item><key>sticker_url</key><val>http://kik.me/nothing</val></item>" +
                    //"<item><key>icon</key><val>http://kik.me/nothing</val></item>" +
                    //"<item><key>pngPreview</key><val>http://smiley-cdn.kik.com/smileys/1db846b7/96x96.png</val></item>" +
                    //"<item><key>sticker_pack_id</key><val>Nothing</val></item>" +
                    //"<item><key>id</key><val>6491837138731108</val></item>" +
                    //"<item><key>compatPreview</key><val>http://smiley-cdn.kik.com/smileys/1db846b7/96x96.png</val></item>" +
                    "</extras>";

            //Add images, icon is standard stickers icon
            xmpp += "<images>" +
                    "<icon>aVZCT1J3MEtHZ29BQUFBTlNVaEVVZ0FBQURJQUFBQXlDQUlBQUFDUlhSL21BQUFBQTNOQ1NWUUlDQWpiNFUvZ0FBQURmRWxFUVZSWWhlM1lTVXdUVVJ6SDhlL1FLalV0VzFVaW1ucHlKVjdVeElSTEZUVnFvZ2dINDBrdjNpUkdFNzFZQ2RFRUExNU1ORzZKVytKeWRrL1VnMkpSWEE5ZUZJeWdSZ1F0Z3FCdFRDcEJ4c004cGdnemI1YTJ5b0hmNlhYZTYrUER6SC9ldktseWRhL0srRXZPL3dZWVo0TGxKQk1zSnhtbkxHODZYdzdPWnRrV2ZIbWpqeWNUUEw5TVg4ZS9aZm55V0J0QlVXUUR3dHNCVkpXN2gwakdzOCtxckI4Tkdrank3aUZmV2xCVlNrcVpFMmF5VDNRcEN1c2lxQ3JYYThESnNxM1lYK1hEMVFSRHFZL1JVL1JMTDFOUmlPWFZxWS85blVSUFpKcFZVWWRuK013Mkh1UEhaN3QvSUg4R0szZUo5dTlCYnRiYStwYXRPN0d5WHBpK2QzRXQ0c0FFeEdOY2k5RDNDY0RqcGJJK1E2eUtPbEZNN2MwOE9PNEFOREpOSjJsckFsQVVLdXJTWm9XcnhYbHFiK2JWTFpjbUxhOXY4ellLNFBGU3ZzdGlzQVZMcS9GNExGMlRscFk3OUhjQkZNeElnNlhYd2YyakdUQnBpUTZYUVZXRGJKZ3B5NWNuU3FyeFdNWk1XdlIvMGovVmRJd3BhMjFFTkJ6ZGQzWVNqNG5HNmoybVkweFoycW1LbnNvc1NVUzdBcExIbHpFck9GczA1T3U0NitoWFlQcGM0d0hHckdWYnM2SVpteVdiakk4YnMzd0JnSUZrMWpndytBdGdTcjV4cjJ3SDBkNlVCUTZFRmpOekViOSs0czAxSFNOanhWb3pSdkVIS2R0R1lNU0tNRFFrR3k5anFabDRzUzBvb1h5bndYSEpiWWljVlZKS29qc3QwL3I5VFBJWkhGZFZ3V296cVJQWncyZE8yRDBveDBOVmc0WHAyd2RlMzNiQ1NpYUExTjdYYWJ5NWJEeG8zS1diK2pwNGVOcDBCbVBXODhzdVFVQ09odzBITEV5OTcybVNQaitNV2ZxN1ZHSElzRjhXeS9NVWU4T2pNeGFUbU5hV2RodXVxRGJyTjg3Ni9WYW1WcDVlc0o3SGxIWDNrR2prVzIzWjlCU1VXTlI0ZHh0UEw5cWF5cFNWaklzVHR0SnFnNnZIY0gwYVdVOVB6dHVkU3JaQTNLZ1JqUlU3ckNmeUIyV21IOTNXOVdTWHBhcDg3d0lvbk1YQ2RSWVRsVzB6TmZWK29QR0lBNU1GQzNod25OK0RBUE9YczJDTmJHVGc3eDJ3YnVwNXh5UHo5Y2tsQzdoWks0cHNRYm10cXpuUzlLV0Y1ck9PVGJaWXdQVjlEQTBDRk02aXFvSEE5TkVEUW9zTlRGL2JlWGJKamNrdUM3aFJTNkpIdEZmdnBxcUJ2T0pVNzh4Rm8wMzluVHcrNTlLRW8xOXN0SXg5dnh0SU12QVRmeEVvd3BUbzVkNWg5eVkzTENBd2pWVzdUVGRNOFc3dU83enZNc1BTVXp5UHBadko5YWVPZkh6Qnl5dnBtdEpsWlMvajlKZm1DWmFUVExDY1pKeXkvZ0QxM2dZUHZBK0t1Z0FBQUFCSlJVNUVya0pnZ2c9PQ==</icon>" +
                    $"<png-preview>{Convert.ToBase64String(png)}</png-preview>" +
                    "</images>";

            //Close tags
            xmpp += "<uris/>" +
                    "</content>" +
                    "</message>";

            return PrepareXMPP(xmpp);
        }

        /// <summary>
        /// Uploads the provided stream as media, and sends the media messgae
        /// </summary>
        /// <param name="text">The text to make an image of</param>
        /// <param name="to">The jid to send the media to</param>
        /// <param name="jid">The jid of the client</param>
        /// <param name="passkey">The passkey of the client</param>
        /// <param name="kikVersion">The kik version of the client</param>
        /// <param name="fontSize">Optionally, designate the font size</param>
        /// <param name="foregroundColor">Optionally, designate the text color</param>
        /// <param name="backgroundColor">Optionally, designate the background color</param>
        /// <returns></returns>
        public static async Task<(string, string)> Media(string text, string to, string jid, string passkey, string kikVersion, int fontSize = 12, Color foregroundColor = default(Color), Color backgroundColor = default(Color))
        {
            return await Media(
                new MemoryStream(
                    ImageGenerator.DrawText(
                        text,
                        new Font(FontFamily.GenericSansSerif, fontSize),
                        foregroundColor == default(Color) ? Color.Blue : foregroundColor,
                        foregroundColor == default(Color) ? Color.Transparent : backgroundColor
                    )
                ),
                "text.png",
                to,
                jid,
                passkey,
                kikVersion
            );
        }

        /// <summary>
        /// Uploads the provided stream as media, and sends the media messgae
        /// </summary>
        /// <param name="media">The media to upload. Can be a FileStream or MemoryStream, for example</param>
        /// <param name="fileName">The name or path to the file, inlcuding the extension (".png", ".mp4")</param>
        /// <param name="to">The jid to send the media to</param>
        /// <param name="jid">The jid of the client</param>
        /// <param name="passkey">The passkey of the client</param>
        /// <param name="kikVersion">The kik version of the client</param>
        /// <returns></returns>
        public static async Task<(string, string)> Media(Stream media, string fileName, string to, string jid, string passkey, string kikVersion)
        {
            var extension = fileName[^4..];
            var isVideo = extension == ".mp4";
            var mediaLength = media.Length;

            //Set up thumbnails
            //TODO: Proper thumbnail for videos, large images
            var thumbnail = "";
            if (!isVideo)
            {
                var imageBytes = new byte[mediaLength];
                await media.ReadAsync(imageBytes, 0, (int)mediaLength);
                thumbnail = Convert.ToBase64String(imageBytes);
            }
            else
            {
                var thumbnailBitmap = new Bitmap(400, 230);
                using (var thumbnailStream = new MemoryStream())
                {
                    thumbnailBitmap.Save(thumbnailStream, ImageFormat.Png);
                    thumbnail = Convert.ToBase64String(thumbnailStream.ToArray());
                }
            }


            //Upload image
            var mediaId = await MediaUploader.UploadMedia(media, fileName, jid, passkey, kikVersion, () => media.Dispose());

            //Set up message portion, varying based on destination type
            var isGroup = to.GetUsernameFromJid() == "GROUP";

            var xmpp = "<message " +
                        "type=\"" + (isGroup ? "groupchat" : "chat") + "\" " +
                        (isGroup ? "xmlns=\"kik:groups\" " : "") +
                        "to=\"" + to + "\" " +
                        (string.IsNullOrEmpty(jid) ? "" : "from=\"" + jid + "\" ") +
                        "id=\"$id\" " +
                        "cts=\"$ts\">";

            //Add kik-y bits
            xmpp += "<kik push=\"true\" qos=\"true\" timestamp=\"$ts\" /><request xmlns=\"kik:message:receipt\" r=\"true\" d=\"true\" />";

            //Add content id from image upload
            //xmpp += "<content id=\"" + id + "\" app-id=\"com.kik.ext.gallery\" v=\"2\">";
            xmpp += $"<content id=\"{mediaId}\" app-id=\"com.kik.ext.{(isVideo ? "video-" : "")}camera\" v=\"2\">";

            //Add strings section
            xmpp += "<strings>" +
                    //"<app-name>Gallery</app-name>" +
                    "<app-name>Camera</app-name>" +
                    $"<file-size>{mediaLength}</file-size>" +
                    $"<file-name>{mediaId}{extension}</file-name>" +
                    "<allow-forward>true</allow-forward>" +
                    $"{(isVideo ? "<layout>video</layout>" : "")}" +
                    "</strings>";

            if (isVideo)
            {
                xmpp += "<extras><item><key>needstranscoding</key><val>false</val></item></extras>";
            }
            else xmpp += "<extras />";

            //Add previews
            xmpp += "<images>" +
                    $"<preview>{thumbnail}</preview>" +
                    "<icon>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</icon>" +
                    "</images>" +
                    "<uris />";

            //Wrap up message tags
            xmpp += "</content>" +
                    "</message>";

            return PrepareXMPP(xmpp);
        }

        /// <summary>
        /// Starts chatting with a user
        /// </summary>
        /// <param name="jid">The jid to start chatting with</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) StartChatting(string jid)
        {
            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:iq:friend\"><add jid=\"{jid}\" /><context type=\"pull-username-search\" reply=\"false\" /></query></iq>");
        }

        /// <summary>
        /// Starts chatting with an alias
        /// </summary>
        /// <param name="alias">The alias to start chatting with</param>
        /// <param name="groupJid">The group in which the alias exists</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) StartChattingWithAlias(string alias, string groupJid)
        {
            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:iq:friend\"><add jid=\"{alias}\" /><context type=\"group-menu-add\" jid=\"{groupJid}\" reply=\"false\" /></query></iq>");
        }

        /// <summary>
        /// Stops chatting with a user
        /// </summary>
        /// <param name="jid">The jid of the user to stop chatting with</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) StopChatting(string jid)
        {
            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:iq:friend\"><remove jid=\"{jid}\" /></query></iq>");
        }

        public static (string, string) LeaveGroup(string groupJid)
        {
            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:groups:admin\"><g jid=\"{groupJid}\"><l /></g></query></iq>");
        }

        /// <summary>
        /// Removes a user from a group, if the client is an admin
        /// </summary>
        /// <param name="groupJid">Group to remove the user from</param>
        /// <param name="userJid">User to remove</param>
        /// <returns></returns>
        public static (string, string) RemoveFromGroup(string groupJid, string userJid)
        {
            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:groups:admin\"><g jid=\"{groupJid}\"><m r=\"1\">{userJid}</m></g></query></iq>");
        }

        /// <summary>
        /// Changes the pasword, and optionally the email as well, for the currently logged in account
        /// </summary>
        /// <param name="newPassword">The new desired password</param>
        /// <param name="username">The username of the account</param>
        /// <param name="email">The current email of the account, or if <paramref name="changeEmail"/> is true, the new email to set</param>
        /// <param name="changeEmail">If true, the email will be changed as well</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) ChangePassword(string newPassword, string username, string email, bool changeEmail = false)
        {
            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:iq:user-profile\">{(changeEmail ? $"<email>{email}</email>" : "")}<passkey-e>{KikUtilities.HashPassword(email, newPassword)}</passkey-e><passkey-u>{KikUtilities.HashPassword(username, newPassword)}</passkey-u></query></iq>");
        }

        /// <summary>
        /// Changes the email for the currently signed in account
        /// WARNING: I do not know what will happen if the password is incorrect.
        /// </summary>
        /// <param name="password">The current password</param>
        /// <param name="newEmail">The new email</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) ChangeEmail(string password, string newEmail)
        {
            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:iq:user-profile\"><email>{newEmail}</email><passkey-e>{KikUtilities.HashPassword(newEmail, password)}</passkey-e></query></iq>");
        }

        /// <summary>
        /// Uses the given SetUserProfileRequest to set information about the Client's profile
        /// </summary>
        /// <param name="request">The SetUserProfileRequest to send to Kik</param>
        /// <returns></returns>
        public static (string, string) SetUserProfile(SetUserProfileRequest request)
        {
            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:iq:xiphias:bridge\" service=\"mobile.profile.v1.Profile\" method=\"SetUserProfile\"><body>{Convert.ToBase64String(request.ToByteArray())}</body></query></iq>");
        }

        /// <summary>
        /// Adds a smiley to the currently signed in account
        /// </summary>
        /// <param name="category">Smiley category. Usually just the text of the emote</param>
        /// <param name="id">Smiley ID</param>
        /// <param name="title">Smiley title. Can be null.</param>
        /// <param name="text">Smiley text. Ex: ":D"</param>
        /// <param name="installDate">The date in milliseconds that the smiley was unlocked</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) AddSmiley(string category, string id, string title, string text, long installDate)
        {
            return AddSmileys(new List<(string, string, string, string, long)>() { (category, id, title, text, installDate) });
        }

        /// <summary>
        /// Adds a set of smileys to the currently signed in account
        /// </summary>
        /// <param name="smileys">List of smileys, where a smiley is a: (category, id, title, text, installDate)</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) AddSmileys(List<(string, string, string, string, long)> smileys)
        {
            var records = "";
            foreach (var smiley in smileys)
            {
                var encodedSmiley = new SmileyRecord
                {
                    Category = smiley.Item1,
                    Id = smiley.Item2,
                    Title = smiley.Item3,
                    Text = smiley.Item4,
                    InstallDate = smiley.Item5
                };

                records += $"<record sk=\"{smiley.Item2}\">{Convert.ToBase64String(encodedSmiley.ToByteArray()).ConvertStandardBase64ToKikBase64()}</record>";
            }

            return PrepareXMPP($"<iq type=\"set\" id=\"$id\"><query xmlns=\"kik:iq:xdata\"><record-set pk=\"smiley_list\">{records}</record-set></query></iq>");
        }

        /// <summary>
        /// Requests an updated roster. If a timestamp is given, only the roster changes will be returned
        /// </summary>
        /// <param name="timestamp">The time at which the roster was last updated</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) RequestRoster(long timestamp = 0)
        {
            return PrepareXMPP($"<iq type=\"get\" id=\"$id\"><query p=\"8\" {((timestamp > 0) ? $"ts=\"{timestamp}\" " : "")}xmlns=\"jabber:iq:roster\" /></iq>");
        }

        /// <summary>
        /// Shorthand for an empty msg-acks update. Will cause the server to send the message history
        /// </summary>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) RequestHistory() => SendMessageHistoryAcks(requestMore: true);

        /// <summary>
        /// Acknowledges receipt of backlogged messages. Messages that aren't acknowledged will stay in the
        /// backlog for an unknown amount of time, and will be seen upon login of any normal kik client
        /// </summary>
        /// <param name="messageIds">Message ids to acknowledge, in the format of: (groupJid, userJid, messageId)</param>
        /// <param name="requestMore">Request more history to be returned to us</param>
        /// <returns>The stanza string, and the id with which it was sent</returns>
        public static (string, string) SendMessageHistoryAcks(List<(string, string, string)> messageIds = null, bool requestMore = false)
        {
            var acksList = "";
            if (messageIds != null)
            {
                //Iterates over each unique group we got messages from
                //If the groupJid is null, we're acking a dm, and that's fine
                foreach (var groupJid in messageIds.Select(x => x.Item1).Distinct())
                {
                    //For each group, iterate over each unique user who sent messages
                    foreach (var userJid in messageIds.Where(x => x.Item1 == groupJid).Select(x => x.Item2).Distinct())
                    {
                        acksList += $"<sender jid=\"{userJid}\"{(groupJid != null ? $" g=\"{groupJid}\"" : "")}>";
                        foreach (var userMessage in messageIds.Where(x => x.Item1 == groupJid && x.Item2 == userJid))
                        {
                            acksList += $"<ack-id receipt=\"true\">{userMessage.Item3}</ack-id>";
                        }
                        acksList += "</sender>";
                    }
                }
            }

            var msgAcksTag = string.IsNullOrEmpty(acksList) ? "<msg-acks />" : $"<msg-acks>{acksList}</msg-acks>";

            return PrepareXMPP($"<iq type=\"set\" id=\"$id\" cts=\"$ts\"><query xmlns=\"kik:iq:QoS\">{msgAcksTag}<history attach=\"{(requestMore ? "true" : "false")}\" /></query></iq>");
        }

        //Queries for registration information, aka login info
        public static (string, string) RegisterQuery(string username, string password, string version, string deviceId, string androidId, string captchaResponse = null)
        {
            string xmpp = "<iq type=\"set\" " +
                "id=\"$id\">" +
                "<query xmlns=\"jabber:iq:register\">" +
                $"<username>{username}</username>" +
                $"<passkey-u>{KikUtilities.HashPassword(username, password)}</passkey-u>" +
                (!string.IsNullOrEmpty(captchaResponse) ? $"<challenge><response>{captchaResponse}</response></challenge>" : "") +
                $"<device-id>{deviceId[3..]}</device-id>" +
                "<operator>310260</operator>" +
                "<deviceid-type>android</deviceid-type>" +
                "<brand>Android</brand>" +
                $"<version>{version}</version>" +
                "<lang>en_US</lang>" +
                "<android-sdk>23</android-sdk>" +
                $"<prefix>{deviceId[..3]}</prefix>" +
                $"<android-id>{androidId}</android-id>" +
                "<model>Android</model>" +
                "</query></iq>";
            return PrepareXMPP(xmpp);

        }

        public static Dictionary<string, string> AnonymousLoginStanza(string deviceid, string secretHash, string versionName, string connectionType = "WIFI", string lang = "en_US")
        {
            var uuid = KikUtilities.GenerateKikUUID();
            var timestamp = KikUtilities.GenerateKikTimestamp().ToString();

            return new Dictionary<string, string>
            {
                ["signed"] = KikUtilities.SignValues(uuid, timestamp, versionName, deviceid),
                ["lang"] = lang,
                ["sid"] = uuid,
                ["anon"] = "1",
                ["ts"] = timestamp,
                ["v"] = versionName,
                ["cv"] = KikUtilities.HMACSHA1($"{timestamp}:{deviceid}", secretHash),
                ["conn"] = connectionType,
                ["dev"] = deviceid,
                ["n"] = "1"
            };
        }

        public static Dictionary<string, string> LoginStanza(string jidPrefix, string deviceid, string passkey, string secretHash, string versionName, string connectionType = "WIFI", string lang = "en_US")
        {
            var uuid = KikUtilities.GenerateKikUUID();
            var timestamp = KikUtilities.GenerateKikTimestamp().ToString();
            var jidSuffix = "talk.kik.com";
            var jid = jidPrefix + "@" + jidSuffix;

            return new Dictionary<string, string>
            {
                ["signed"] = KikUtilities.SignValues(uuid, timestamp, versionName, jid),
                ["lang"] = lang,
                ["sid"] = uuid,
                ["to"] = jidSuffix,
                ["from"] = jidPrefix + '@' + jidSuffix + '/' + deviceid,
                ["p"] = passkey,
                ["ts"] = timestamp,
                ["v"] = versionName,
                ["cv"] = KikUtilities.HMACSHA1($"{timestamp}:{jid}", secretHash),
                ["conn"] = connectionType ?? "unknown",
                ["n"] = "1"
            };
        }

        public static string Ping() => "<ping/>";

        //Replaces the placeholders in crafted xmpp
        public static (string, string) PrepareXMPP(string xmpp)
        {
            var id = KikUtilities.GenerateKikUUID();
            if (xmpp.Contains("$id"))
            {
                xmpp = xmpp.Replace("$id", id);
            }
            if (xmpp.Contains("$ts"))
            {
                xmpp = xmpp.Replace("$ts", KikUtilities.GenerateKikTimestamp().ToString());
            }
            return (xmpp, id);
        }
    }
}
