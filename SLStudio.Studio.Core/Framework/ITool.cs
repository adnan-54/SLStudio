using SLStudio.Studio.Core.Framework.Services;

namespace SLStudio.Studio.Core.Framework
{
    public interface ITool : ILayoutItem
	{
		PaneLocation PreferredLocation { get; }
        double PreferredWidth { get; }
        double PreferredHeight { get; }

		bool IsVisible { get; set; }
	}
}
