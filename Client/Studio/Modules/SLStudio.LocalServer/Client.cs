using ServiceStack.Text;
using System.IO;
using System.IO.Pipes;

namespace SLStudio.LocalServer
{
    public static class Client
    {
        public static void SendMessage(ClientMessage message)
        {
            using var client = new NamedPipeClientStream(SharedConstants.GlobalKey);

            var connected = ConnectToServer(client);
            if (!connected)
                return;

            var json = JsonSerializer.SerializeToString(message);

            using var writer = new StreamWriter(client);
            writer.Write(json);
            writer.Flush();
        }

        private static bool ConnectToServer(NamedPipeClientStream client)
        {
            try
            {
                client.Connect(100);
                return client.IsConnected;
            }
            catch
            {
                return false;
            }
        }
    }
}