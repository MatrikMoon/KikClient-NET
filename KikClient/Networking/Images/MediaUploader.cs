using KikClient.Networking.Sockets;
using KikClient.Utilities;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KikClient.Networking.Images
{
    class MediaUploader
    {
        private const long _maxChunkSize = 614400L;

        internal static async Task<string> UploadMedia(Stream image, string fileName, string jid, string passkey, string kikVersion, Action onComplete = null)
        {
            //Create an id for this image
            var imageId = KikUtilities.GenerateKikUUID();
            var contentMd5 = MD5Stream(image);

            //Connect to kik http servers
            var uri = new Uri($"https://platform.kik.com/content/files/{imageId}");
            var uploadClient = new SSLClient(uri.Host, 443);
            await uploadClient.Start();

            int totalChunks = (int)(image.Length / _maxChunkSize) + 1;
            await SendChunk(uploadClient, image, fileName, uri, jid, passkey, kikVersion, 0, totalChunks, imageId, contentMd5, onComplete);

            return imageId;
        }

        private static async Task SendChunk(SSLClient client, Stream image, string fileName, Uri uri, string jid, string passkey, string kikVersion, int chunk, int totalChunks, string imageId, string contentMd5, Action onComplete = null)
        {
            long chunkSize = chunk >= (totalChunks - 1) ? (image.Length % _maxChunkSize) : _maxChunkSize;
            long chunkStart = chunk * _maxChunkSize;

            //Define "app" name - different things for different results
            var appname = "";
            var extension = fileName[^4..];
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                appname = "com.kik.ext.camera";
            }
            else if (extension == ".mp4") appname = "com.kik.ext.video-camera";

            var chunkHeader = $"PUT {uri.LocalPath} HTTP/1.1\r\n" +
                $"Host: {uri.Host}\r\n" +
                $"Connection: Keep-Alive\r\n" +
                $"Content-Length: {chunkSize}\r\n" +
                $"User-Agent: Kik/{kikVersion} (Android 10) Content\r\n" +
                $"x-kik-jid: {jid}\r\n" +
                $"x-kik-password: {passkey}\r\n" +
                $"x-kik-verification: {KikUtilities.SHA1Hash("YA=57aSA!ztajE5" + imageId + appname)}\r\n" +
                $"x-kik-app-id: {appname}\r\n" +
                $"x-kik-content-chunks: {totalChunks}\r\n" +
                $"x-kik-content-size: {image.Length}\r\n" +
                $"x-kik-content-md5: {contentMd5}\r\n" +
                $"x-kik-chunk-number: {chunk}\r\n" +
                $"x-kik-chunk-md5: {MD5Stream(image, chunkStart, (int)chunkSize)}\r\n" +
                $"x-kik-content-extension: {extension}\r\n" +
                $"\r\n";

            //Logger.Debug(chunkHeader);
            await client.Send(Encoding.UTF8.GetBytes(chunkHeader));

            Func<byte[], Task> onReceive = null;
            onReceive = async (bytes) =>
            {
                var receiveString = Encoding.UTF8.GetString(bytes);

                //Logger.Warning($"Chunk recieve:\n{receiveString}");

                if (receiveString.StartsWith("HTTP/1.1 200 OK"))
                {
                    client.BytesReceived -= onReceive;
                    if (++chunk < totalChunks) await SendChunk(client, image, fileName, uri, jid, passkey, kikVersion, chunk, totalChunks, imageId, contentMd5);
                    else
                    {
                        Logger.Success("File upload success!");
                        await client.Shutdown();
                        onComplete?.Invoke();
                    }
                }
                else if (!receiveString.Contains("Connection: keep-alive"))
                {
                    Logger.Error("File upload failure!");
                    client.BytesReceived -= onReceive;
                    await client.Shutdown();
                    onComplete?.Invoke();
                }
            };
            client.BytesReceived += onReceive;

            byte[] readData = new byte[chunkSize];
            image.Seek(chunkStart, SeekOrigin.Begin);
            image.Read(readData, 0, (int)chunkSize);
            await client.Send(readData);
        }

        public static string MD5Stream(Stream stream, long startOffset = 0, int count = 0)
        {
            using (MD5 md5 = MD5.Create())
            {
                if (startOffset != 0 || count != 0)
                {
                    var bytes = new byte[count];
                    stream.Seek(startOffset, SeekOrigin.Begin);
                    stream.Read(bytes, 0, count);
                    return KikUtilities.BytesToString(md5.ComputeHash(bytes));
                }
                else
                {
                    return KikUtilities.BytesToString(md5.ComputeHash(stream));
                }
            }
        }
    }
}
