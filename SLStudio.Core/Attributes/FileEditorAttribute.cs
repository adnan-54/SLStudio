using System;
using System.Collections.Generic;
using System.Text;

namespace SLStudio.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FileEditorAttribute : Attribute
    {
        public FileEditorAttribute(string extension, string nameKey, string descriptionKey, Type resource, string iconSource = null)
        {
        }
    }
}