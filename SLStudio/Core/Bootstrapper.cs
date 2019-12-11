﻿using Caliburn.Micro;
using MahApps.Metro;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace SLStudio
{
    public sealed class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer container = new SimpleContainer();

        public Bootstrapper()
        {
            PreInitialize();
            Initialize();
        } 

        protected override void Configure()
        {
            container.Instance(container);
        }

        private void PreInitialize()
        {
            string themeAccent = Settings.Default.ThemeAccent;
            string themeBase = Settings.Default.ThemeBase;
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(themeAccent), ThemeManager.GetAppTheme(themeBase));

            CultureInfo culture = new CultureInfo(Settings.Default.LanguageCode);
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}