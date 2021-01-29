using System.IO;
using System.IO.Pipes;

namespace SLStudio
{
    internal class InternalClient
    {
        public static void SendMessage(string message)
        {
            using var client = new NamedPipeClientStream(SLStudioConstants.GlobalKey);

            if (!TryConnect(client))
                return;

            using StreamWriter writer = new StreamWriter(client);
            writer.Write(message);
            writer.Flush();
        }

        private static bool TryConnect(NamedPipeClientStream client)
        {
            try
            {
                client.Connect(100);
            }
            catch
            {
                return false;
            }

            return client.IsConnected;
        }
    }
}