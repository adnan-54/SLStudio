using System.Windows;
using System.Windows.Controls;

namespace SLStudio.Studio.Core.Framework.Controls
{
    public class ExpanderEx : Expander
    {
        static ExpanderEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderEx),
                new FrameworkPropertyMetadata(typeof(ExpanderEx)));
        } 
    }
}