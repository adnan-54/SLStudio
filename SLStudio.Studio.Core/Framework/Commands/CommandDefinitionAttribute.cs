using System;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Framework.Commands
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandDefinitionAttribute : ExportAttribute
    {
        public CommandDefinitionAttribute() : base(typeof(CommandDefinitionBase))
        {
            
        }
    }
}