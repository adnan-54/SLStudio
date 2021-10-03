using ServiceStack.Text;
using SLStudio.Logging;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.LocalServer
{
    public class Server
    {
        private static readonly ILogger logger = LogManager.GetLogger<Server>();
        private static Server instance;

        private NamedPipeServerStream serverPipe;
        private CancellationTokenSource cancelationTokenSource;
        private Task serverTask;

        private Server()
        {
        }

        public event EventHandler ServerStarted;

        public event EventHandler<TaskStatus> ServerStoped;

        public event EventHandler<ClientMessage> MessageRecived;

        public bool IsRunning => serverTask != null
                                 && serverTask.Status != TaskStatus.Canceled
                                 && serverTask.Status != TaskStatus.Faulted
                                 && serverTask.Status != TaskStatus.RanToCompletion;

        public CancellationToken CancellationToken => cancelationTokenSource.Token;

        public bool IsCancelRequested => cancelationTokenSource != null && cancelationTokenSource.IsCancellationRequested;

        public static Server CreateServer()
        {
            if (instance is null)
                instance = new Server();

            return instance;
        }

        public void Start()
        {
            if (IsRunning)
                return;

            serverPipe = new NamedPipeServerStream(SharedConstants.GlobalKey);
            cancelationTokenSource = new CancellationTokenSource();

            serverTask = Task.Run(DoWork, CancellationToken);
            serverTask.ContinueWith(_ => Stop());

            ServerStarted?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            serverPipe.Close();
            serverPipe.Dispose();
            serverPipe = null;

            cancelationTokenSource.Cancel();
            cancelationTokenSource.Dispose();
            cancelationTokenSource = null;

            var taskStatus = serverTask.Status;
            serverTask.Dispose();
            serverTask = null;

            ServerStoped?.Invoke(this, taskStatus);
        }

        private async Task DoWork()
        {
            try
            {
                while (IsRunning)
                {
                    if (IsCancelRequested)
                        return;

                    await serverPipe.WaitForConnectionAsync(CancellationToken);

                    var json = await ReadMessage();
                    var message = DeserializeMessage(json);
                    serverPipe.Disconnect();

                    MessageRecived?.Invoke(this, message);
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
            }
        }

        private Task<string> ReadMessage()
        {
            if (!IsRunning || IsCancelRequested)
                return Task.FromResult(string.Empty);

            using var streamReader = new StreamReader(serverPipe, leaveOpen: true);
            return streamReader.ReadToEndAsync();
        }

        private static ClientMessage DeserializeMessage(string json)
        {
            try
            {
                return JsonSerializer.DeserializeFromString<ClientMessage>(json);
            }
            catch
            {
                return new();
            }
        }
    }
}