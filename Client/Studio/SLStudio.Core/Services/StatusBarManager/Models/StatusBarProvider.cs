using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core
{
    public abstract class StatusBarProvider : IStatusBarProvider
    {
        private readonly List<IStatusBarContent> leftContent;
        private readonly List<IStatusBarContent> centerContent;
        private readonly List<IStatusBarContent> rightContent;

        protected StatusBarProvider()
        {
            leftContent = new List<IStatusBarContent>();
            centerContent = new List<IStatusBarContent>();
            rightContent = new List<IStatusBarContent>();
        }

        public IEnumerable<IStatusBarContent> LeftContent => leftContent;

        public IEnumerable<IStatusBarContent> CenterContent => centerContent;

        public IEnumerable<IStatusBarContent> RightContent => rightContent;

        public void Refresh()
        {
            leftContent.Clear();
            centerContent.Clear();
            rightContent.Clear();

            Setup();
        }

        protected abstract void Setup();

        protected void AddLeftContent(IStatusBarContent content)
        {
            AddContent(content, ContentTarget.Left);
        }

        protected void AddCenterContent(IStatusBarContent content)
        {
            AddContent(content, ContentTarget.Center);
        }

        protected void AddRightContent(IStatusBarContent content)
        {
            AddContent(content, ContentTarget.Right);
        }

        private void AddContent(IStatusBarContent content, ContentTarget target)
        {
            switch (target)
            {
                case ContentTarget.Left:
                    AddContentCore(content, leftContent);
                    break;

                case ContentTarget.Center:
                    AddContentCore(content, centerContent);
                    break;

                case ContentTarget.Right:
                    AddContentCore(content, rightContent);
                    break;
            }
        }

        private static void AddContentCore(IStatusBarContent content, List<IStatusBarContent> contentHost)
        {
            var contentCollection = contentHost.ToList();
            contentCollection.Add(content);

            contentHost.Clear();
            contentHost.AddRange(contentCollection.OrderBy(c => c.Id));
        }

        private enum ContentTarget
        {
            Left,
            Center,
            Right
        }
    }
}