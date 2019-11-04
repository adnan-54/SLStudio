﻿using System;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Framework.Commands
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandHandlerAttribute : ExportAttribute
    {
        public CommandHandlerAttribute() : base(typeof(ICommandHandler))
        {
            
        }
    }
}