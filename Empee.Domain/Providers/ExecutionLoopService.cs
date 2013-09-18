using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows.Forms;
using Empee.Domain.Contracts;
using Empee.Domain.Infrastructure;
using SharpDX.Windows;

namespace Empee.Domain.Providers
{
    [Export(typeof(IExecutionLoopService))]
    internal sealed class ExecutionLoopService : IExecutionLoopService
    {
        private static readonly double Frequency = Stopwatch.Frequency;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private long _lastElapsedTicks;

        [ImportingConstructor]
        public ExecutionLoopService()
        {
            _stopwatch.Start();
        }

        public void Run(Control renderControl)
        {
            OnStarting(new StartingEventArgs(renderControl));

            try
            {
                // Take note of what time we started
                _lastElapsedTicks = _stopwatch.ElapsedTicks;

                // TODO: Using RenderLoop might be bad design; why
                // must we have a window to run an execution loop?
                // Where else could we specify the render control?
                RenderLoop.Run(renderControl, RenderCallback);
            }
            finally
            {
                OnStopping();   
            }
        }

        private void RenderCallback()
        {
            var elapsedTicks = _stopwatch.ElapsedTicks;
            var executingDelta = (elapsedTicks - _lastElapsedTicks)/Frequency;

            OnExecuting(new ExecutingEventArgs(executingDelta));

            // Take not of when our execution started, for next time
            _lastElapsedTicks = elapsedTicks;
        }

        public event StartingEventHandler Starting;

        private void OnStarting(StartingEventArgs startingEventArgs)
        {
            var eventHandler = Starting;

            if (eventHandler != null)
                eventHandler(this, startingEventArgs);
        }

        public event ExecutingEventHandler Executing;

        private void OnExecuting(ExecutingEventArgs executingEventArgs)
        {
            var eventHandler = Executing;

            if (eventHandler != null)
                eventHandler(this, executingEventArgs);
        }

        public event StoppingEventHandler Stopping;

        private void OnStopping()
        {
            var eventHandler = Stopping;

            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }
    }
}