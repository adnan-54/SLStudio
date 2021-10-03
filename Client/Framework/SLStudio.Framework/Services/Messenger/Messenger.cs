using System;

namespace SLStudio
{
    internal class Messenger : Service, IMessenger
    {
        private readonly DevExpress.Mvvm.IMessenger messenger;

        public Messenger()
        {
            messenger = DevExpress.Mvvm.Messenger.Default;
        }

        public void Register<TMessage>(object recipient, object token, bool receiveInheritedMessages, Action<TMessage> action)
        {
            messenger.Register(recipient, token, receiveInheritedMessages, action);
        }

        public void Send<TMessage>(TMessage message, Type messageTargetType, object token)
        {
            messenger.Send(message, messageTargetType, token);
        }

        public void Unregister(object recipient)
        {
            messenger.Unregister(recipient);
        }

        public void Unregister<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            messenger.Unregister(recipient, token, action);
        }
    }
}