namespace SLStudio;

public partial class SplashScreenView
{
    public SplashScreenView()
    {
        InitializeComponent();

        VersionText.Text = SharedConstants.ApplicationVersion?.ToString(3);
    }
}
