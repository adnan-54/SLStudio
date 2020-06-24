using DevExpress.Mvvm.UI.Interactivity;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace SLStudio.Core.Behaviors
{
    public class FadeInFadeOutWindowBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty FadeInTimeProperty = DependencyProperty.Register("FadeInTime", typeof(double), typeof(FadeInFadeOutWindowBehavior), new PropertyMetadata(1.0));

        public static readonly DependencyProperty FadeOutTimeProperty = DependencyProperty.Register("FadeOutTime", typeof(double), typeof(FadeInFadeOutWindowBehavior), new PropertyMetadata(1.0));

        public double FadeInTime
        {
            get => (double)GetValue(FadeInTimeProperty);
            set => SetValue(FadeInTimeProperty, value);
        }

        public double FadeOutTime
        {
            get => (double)GetValue(FadeOutTimeProperty);
            set => SetValue(FadeOutTimeProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Opacity = 0;
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Closing += OnClosing;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation(1, TimeSpan.FromSeconds(FadeInTime)));
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            AssociatedObject.Closing -= OnClosing;

            e.Cancel = true;

            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(FadeOutTime));
            anim.Completed += (sender, e) => AssociatedObject.Close();
            AssociatedObject.BeginAnimation(UIElement.OpacityProperty, anim);
        }
    }
}