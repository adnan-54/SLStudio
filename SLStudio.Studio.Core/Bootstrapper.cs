using Caliburn.Micro;
using SLStudio.Studio.Core.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace SLStudio.Studio.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        private List<Assembly> priorityAssemblies;

        public Bootstrapper()
        {
            PreInitialize();
            Initialize();
        }

        protected CompositionContainer Container { get; set; }

        internal IList<Assembly> PriorityAssemblies { get { return priorityAssemblies; } }

        protected virtual void PreInitialize()
        {
            LoadLanguage();
        }

        private void LoadLanguage()
        {
            CultureInfo culture = new CultureInfo(Settings.Studio.Default.LanguageCode, true);
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        protected override void Configure()
        {
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(@"./");
            AssemblySource.Instance.AddRange(directoryCatalog.Parts
                .Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
                .Where(assembly => !AssemblySource.Instance.Contains(assembly)));

            priorityAssemblies = SelectAssemblies().ToList();
            AggregateCatalog priorityCatalog = new AggregateCatalog(priorityAssemblies.Select(x => new AssemblyCatalog(x)));
            CatalogExportProvider priorityProvider = new CatalogExportProvider(priorityCatalog);

            AggregateCatalog mainCatalog = new AggregateCatalog(AssemblySource.Instance
                    .Where(assembly => !priorityAssemblies.Contains(assembly))
                    .Select(x => new AssemblyCatalog(x)));

            CatalogExportProvider mainProvider = new CatalogExportProvider(mainCatalog);

            Container = new CompositionContainer(priorityProvider, mainProvider);
            priorityProvider.SourceProvider = Container;
            mainProvider.SourceProvider = Container;

            var batch = new CompositionBatch();

            BindServices(batch);
            batch.AddExportedValue(mainCatalog);

            Container.Compose(batch);
        }

        protected virtual void BindServices(CompositionBatch batch)
        {
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(Container);
            batch.AddExportedValue(this);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            IEnumerable<Lazy<object>> exports = Container.GetExports<object>(contract);

            if (exports.Any())
                return exports.First().Value;

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return Container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            Container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewFor<IMainWindow>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetEntryAssembly() };
        }
    }
}
