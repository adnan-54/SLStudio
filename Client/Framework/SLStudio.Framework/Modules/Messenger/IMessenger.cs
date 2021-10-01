using System;

namespace SLStudio
{
    public interface IMessenger : IStudioService
    {
        void Register<TMessage>(object recipient, Action<TMessage> action);

        void Register<TMessage>(object recipient, bool receiveInheritedMessagesToo, Action<TMessage> action);

        void Register<TMessage>(object recipient, object token, Action<TMessage> action);

        void Register<TMessage>(object recipient, object token, bool receiveInheritedMessages, Action<TMessage> action);

        void Send<TMessage>(TMessage message);

        void Send<TMessage, TTarget>(TMessage message);

        void Send<TMessage>(TMessage message, object token);

        void Send<TMessage>(TMessage message, Type messageTargetType, object token);

        void Unregister<TMessage>(object recipient);

        void Unregister<TMessage>(object recipient, object token);

        void Unregister<TMessage>(object recipient, Action<TMessage> action);

        void Unregister(object recipient);

        void Unregister<TMessage>(object recipient, object token, Action<TMessage> action);
    }
}