using System;

namespace SLStudio.Controls
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public ValueChangedEventArgs(T newValue, T oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
        }

        public T NewValue { get; }

        public T OldValue { get; }
    }
}