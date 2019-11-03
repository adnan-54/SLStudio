using Caliburn.Micro;
using System;

namespace SLStudio.Studio.Core.Framework.Results
{
    public class LambdaResult : IResult
    {
        private readonly Action<CoroutineExecutionContext> _lambda;

        public LambdaResult(Action<CoroutineExecutionContext> lambda)
        {
            _lambda = lambda;
        }

        public void Execute(CoroutineExecutionContext context)
        {
            _lambda(context);

            Completed?.Invoke(this, new ResultCompletionEventArgs());
        }

        public event EventHandler<ResultCompletionEventArgs> Completed;
    }
}