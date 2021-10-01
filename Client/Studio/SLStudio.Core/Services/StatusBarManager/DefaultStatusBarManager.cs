using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core
{
    internal class DefaultStatusBarManager : BindableBase, IStatusBarManager
    {
        private readonly List<IStatusBarHost> hosts;

        public DefaultStatusBarManager()
        {
            hosts = new List<IStatusBarHost>();
        }

        public IEnumerable<IStatusBarHost> Hosts => hosts;
        public IEnumerable<IStatusBarContent> LeftContents => hosts.SelectMany(h => h.StatusBarProvider.LeftContent).OrderBy(c => c.Id);
        public IEnumerable<IStatusBarContent> CenterContents => hosts.SelectMany(h => h.StatusBarProvider.CenterContent).OrderBy(c => c.Id);
        public IEnumerable<IStatusBarContent> RightContents => hosts.SelectMany(h => h.StatusBarProvider.RightContent).OrderBy(c => c.Id);

        public void Refresh()
        {
            RaisePropertyChanged(() => Hosts);
            RaisePropertyChanged(() => LeftContents);
            RaisePropertyChanged(() => CenterContents);
            RaisePropertyChanged(() => RightContents);
        }

        public void AddHost(IStatusBarHost host)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));

            if (hosts.Contains(host))
                return;

            host.StatusBarProvider.Refresh();
            hosts.Add(host);

            Refresh();
        }

        public void RemoveHost(IStatusBarHost host)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));

            if (!hosts.Contains(host))
                return;

            hosts.Remove(host);

            Refresh();
        }
    }
}