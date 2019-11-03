using Caliburn.Micro;
using System;

namespace SLStudio.Studio.Core.Framework.Results
{
    public interface IOpenResult<TChild> : IResult
	{
		Action<TChild> OnConfigure { get; set; }
		Action<TChild> OnShutDown { get; set; }
	}
}