using DevExpress.Mvvm;
using System;
using System.Collections.Generic;

namespace SLStudio.Core
{
    public abstract class ToolContentProvider : IToolContentProvider
    {
        private readonly Dictionary<Type, IToolContent> contentCache;
        private bool isInitialized;

        protected ToolContentProvider()
        {
            contentCache = new Dictionary<Type, IToolContent>();
            isInitialized = false;
        }

        public abstract void Register();

        public void Register<TTool, TContent>(TContent content) where TTool : IToolPanel where TContent : IToolContent
        {
            var toolType = typeof(TTool);
            if (contentCache.ContainsKey(toolType))
                throw new InvalidOperationException($"Type {toolType} is alredy registred");
            contentCache.Add(toolType, content);
        }

        public IToolContent GetContent(Type tool)
        {
            Initialze();

            if (contentCache.TryGetValue(tool, out var content))
                return content;
            return null;
        }

        private void Initialze()
        {
            if (isInitialized)
                return;
            isInitialized = true;

            Register();
        }
    }
}