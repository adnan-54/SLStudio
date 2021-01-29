using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace SLStudio
{
    internal class InternalServer
    {
        private static InternalServer instance;

        private bool isRunning;
        private Thread serverThread;
        private NamedPipeServerStream server;
        private CancellationTokenSource cancellationToken;

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
            serverThread = new Thread(ServerThread)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };

            server = new NamedPipeServerStream(SLStudioConstants.GlobalKey);
            cancellationToken = new CancellationTokenSource();
            serverThread.Start();
            isRunning = true;
        }

        public void Stop()
        {
            if (!isRunning || serverThread is null)
                return;

            cancellationToken.Cancel();
            serverThread = null;
            server.Dispose();
            server = null;
            isRunning = false;
        }

        private void ServerThread(object param)
        {
            while (true)
            {
                cancellationToken.Token.ThrowIfCancellationRequested();

                server.WaitForConnection();

                using var streamReader = new StreamReader(server, leaveOpen: true);
                var message = streamReader.ReadToEnd();
                OnMessageRecived(message);

                server.Disconnect();
            }
        }

        private void OnMessageRecived(string message)
        {
            MessageRecived?.Invoke(this, message);
        }
    }
}