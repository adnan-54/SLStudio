using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio
{
    internal class InternalServer
    {
        private static InternalServer instance;

        private bool isRunning;
        private NamedPipeServerStream server;
        private CancellationTokenSource cancellationTokenSource;
        private Task serverTask;

        private InternalServer()
        {
        }

        public event EventHandler<string> MessageRecived;

        public static InternalServer Create()
        {
            if (instance is not null)
                throw new InvalidOperationException("Server already created");
            instance = new InternalServer();
            return instance;
        }

        public void Start()
        {
            isRunning = true;

            server = new NamedPipeServerStream(StudioConstants.GlobalKey);
            cancellationTokenSource = new CancellationTokenSource();
            serverTask = Task.Run(() => RunAsync(), cancellationTokenSource.Token);
            serverTask.ContinueWith(t => Stop());
        }

        public void Stop()
        {
            if (!isRunning || serverTask is null)
                return;

            isRunning = false;

            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;

            server.Close();
            server = null;

            serverTask.Dispose();
            serverTask = null;
        }

        private async Task RunAsync()
        {
            try
            {
                while (isRunning)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    await server.WaitForConnectionAsync(cancellationTokenSource.Token);

                    var message = await ReadMessageAsync();

                    server.Disconnect();

                    OnMessageRecived(message);
                }
            }
            catch
            {
                await Task.FromException(new TaskCanceledException());
            }
        }

        private Task<string> ReadMessageAsync()
        {
            if (!server.IsConnected)
                return Task.FromResult(string.Empty);

            using var streamReader = new StreamReader(server, leaveOpen: true);
            return streamReader.ReadToEndAsync();
        }

        private void OnMessageRecived(string message)
        {
            MessageRecived?.Invoke(this, message);
        }
    }

    internal class InternalClient
    {
        public static void SendMessage(string message)
        {
            using var client = new NamedPipeClientStream(StudioConstants.GlobalKey);

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