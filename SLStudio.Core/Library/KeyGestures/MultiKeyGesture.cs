﻿using SLStudio.Core.Modules.StatusBar.Resources;
using SLStudio.Core.Resources.Language;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace SLStudio.Core
{
    public class MultiKeyGesture : KeyGesture
    {
        private readonly IStatusBar statusBar;
        private readonly KeyGesturePart[] gestures;
        private readonly int gesturesCount;
        private int lastInputTimeStamp;
        private int step;

        public MultiKeyGesture(params KeyGesturePart[] gestures) : base(Key.None, ModifierKeys.None, CreateDisplayString(gestures))
        {
            statusBar = IoC.Get<IStatusBar>();
            this.gestures = gestures;
            gesturesCount = gestures.Count();
        }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            if (!(inputEventArgs is KeyEventArgs args) || args.Timestamp == lastInputTimeStamp)
                return false;

            if (step < gesturesCount)
            {
                if (!gestures[step].Matches(args.Key, Keyboard.Modifiers))
                {
                    if (step > 0)
                    {
                        var combination = gestures.Take(step).Concat(new[] { new KeyGesturePart(args.Key, Keyboard.Modifiers) });
                        statusBar.Status = string.Format(Language.CombinationIsNotACommand, CreateDisplayString(combination));
                    }
                    step = 0;
                    return false;
                }

                lastInputTimeStamp = args.Timestamp;
                statusBar.Status = string.Format(Language.WaitingForNextKeyOfChord, gestures[step]);
                step++;
            }

            if (step == gesturesCount)
            {
                step = 0;
                statusBar.Status = StatusBarResources.Ready;
                return true;
            }
            return false;
        }

        private static string CreateDisplayString(IEnumerable<KeyGesturePart> gestures)
        {
            return string.Join(", ", gestures.Select(g => g.ToString()));
        }
    }
}