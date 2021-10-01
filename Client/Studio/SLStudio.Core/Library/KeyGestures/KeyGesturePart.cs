using System.Windows.Input;

namespace SLStudio.Core
{
    public class KeyGesturePart
    {
        public KeyGesturePart(Key key, ModifierKeys modifier = ModifierKeys.None)
        {
            Key = key;
            Modifier = modifier;
        }

        public Key Key { get; private set; }
        public ModifierKeys Modifier { get; private set; }

        public bool Matches(Key key, ModifierKeys modifier)
        {
            return key == Key && modifier == Modifier;
        }

        public override string ToString()
        {
            return OemKeyGesture.CreateDisplayString(Key, Modifier);
        }
    }
}