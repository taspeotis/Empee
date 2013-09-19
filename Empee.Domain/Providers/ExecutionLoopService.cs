using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Empee.Domain.Contracts;
using Empee.Domain.Infrastructure;
using SharpDX.Windows;

namespace Empee.Domain.Providers
{
    [Export(typeof (IExecutionLoopService))]
    internal sealed class ExecutionLoopService : IExecutionLoopService
    {
        private static readonly double Frequency = Stopwatch.Frequency;

        private readonly IContext _context;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private long _lastElapsedTicks;

        [ImportingConstructor]
        public ExecutionLoopService(IContext context)
        {
            _context = context;

            _stopwatch.Start();
        }

        public void Run()
        {
            var renderControl = _context.RenderControl;

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

        public event StartingEventHandler Starting;

        public event ExecutingEventHandler Executing;

        public event StoppingEventHandler Stopping;

        private void RenderCallback()
        {
            var elapsedTicks = _stopwatch.ElapsedTicks;
            var executingDelta = (elapsedTicks - _lastElapsedTicks)/Frequency;

            OnExecuting(new ExecutingEventArgs(executingDelta));

            // Take not of when our execution started, for next time
            _lastElapsedTicks = elapsedTicks;
        }

        private void OnStarting(StartingEventArgs startingEventArgs)
        {
            var eventHandler = Starting;

            if (eventHandler != null)
                eventHandler(this, startingEventArgs);
        }

        private void OnExecuting(ExecutingEventArgs executingEventArgs)
        {
            var eventHandler = Executing;

            if (eventHandler != null)
                eventHandler(this, executingEventArgs);
        }

        private void OnStopping()
        {
            var eventHandler = Stopping;

            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }
    }
}