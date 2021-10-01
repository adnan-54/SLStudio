using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace SLStudio.Core
{
    public class OemKeyGesture : KeyGesture
    {
        private static readonly Regex OemExpression = new Regex("Oem.[A-Za-z0-9]*");
        private static readonly ModifierKeysConverter modifierKeysConverter = new ModifierKeysConverter();

        public OemKeyGesture(Key key) : base(key, ModifierKeys.None, CreateDisplayString(key))
        {
        }

        public OemKeyGesture(Key key, ModifierKeys modifiers) : base(key, modifiers, CreateDisplayString(key, modifiers))
        {
        }

        public static string CreateDisplayString(Key key, ModifierKeys modifiers)
        {
            var modFlags = GetAllValuesOfFlagged(modifiers).Where(m => m != ModifierKeys.None);
            var modDisplayString = string.Join("+", modFlags.Select(m => CreateDisplayString(m)));
            if (string.IsNullOrEmpty(modDisplayString))
                return CreateDisplayString(key);
            return $"{modDisplayString}+{CreateDisplayString(key)}";
        }

        public static string CreateDisplayString(Key key)
        {
            var inputGestureText = key.ToString();
            return OemExpression.Replace(inputGestureText, new MatchEvaluator(ReplaceOem));
        }

        private static string CreateDisplayString(ModifierKeys modifiers)
        {
            return (string)modifierKeysConverter.ConvertTo(modifiers, typeof(string));
        }

        private static IEnumerable<ModifierKeys> GetAllValuesOfFlagged(ModifierKeys modifiers)
        {
            foreach (ModifierKeys value in Enum.GetValues(typeof(ModifierKeys)))
            {
                if ((value & modifiers) == value)
                    yield return value;
            }
        }

        private static string ReplaceOem(Match match)
        {
            if (Enum.TryParse(match.Value, out Key key))
                return GetCharsFromKeys(key);

            return string.Empty;
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
            StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        private static string GetCharsFromKeys(Key key)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            ToUnicode((uint)virtualKey, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }
    }
}