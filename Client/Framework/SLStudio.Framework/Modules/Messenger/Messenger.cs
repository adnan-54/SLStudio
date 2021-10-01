using System;

namespace SLStudio
{
    internal class Messenger : StudioService, IMessenger
    {
        private readonly DevExpress.Mvvm.IMessenger messenger;

        public Messenger()
        {
            messenger = DevExpress.Mvvm.Messenger.Default;
        }

        public void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            DevExpress.Mvvm.MessengerExtensions.Register(messenger, recipient, action);
        }

        public void Register<TMessage>(object recipient, bool receiveInheritedMessagesToo, Action<TMessage> action)
        {
            DevExpress.Mvvm.MessengerExtensions.Register(messenger, recipient, receiveInheritedMessagesToo, action);
        }

        public void Register<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            DevExpress.Mvvm.MessengerExtensions.Register(messenger, recipient, token, action);
        }

        public void Register<TMessage>(object recipient, object token, bool receiveInheritedMessages, Action<TMessage> action)
        {
            messenger.Register(recipient, token, receiveInheritedMessages, action);
        }

        public void Send<TMessage>(TMessage message)
        {
            DevExpress.Mvvm.MessengerExtensions.Send(messenger, message);
        }

        public void Send<TMessage, TTarget>(TMessage message)
        {
            DevExpress.Mvvm.MessengerExtensions.Send<TMessage, TTarget>(messenger, message);
        }

        public void Send<TMessage>(TMessage message, object token)
        {
            DevExpress.Mvvm.MessengerExtensions.Send(messenger, message, token);
        }

        public void Send<TMessage>(TMessage message, Type messageTargetType, object token)
        {
            messenger.Send(message, messageTargetType, token);
        }

        public void Unregister<TMessage>(object recipient)
        {
            DevExpress.Mvvm.MessengerExtensions.Unregister<TMessage>(messenger, recipient);
        }

        public void Unregister<TMessage>(object recipient, object token)
        {
            DevExpress.Mvvm.MessengerExtensions.Unregister<TMessage>(messenger, recipient, token);
        }

        public void Unregister<TMessage>(object recipient, Action<TMessage> action)
        {
            DevExpress.Mvvm.MessengerExtensions.Unregister(messenger, recipient, action);
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