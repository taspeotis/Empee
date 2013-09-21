using System;
using Empee.Domain.Contracts;

namespace Empee.Domain.Infrastructure
{
    public sealed class RenderingEventArgs : EventArgs
    {
        public RenderingEventArgs(IDrawingService drawingService)
        {
            DrawingService = drawingService;
        }

        public IDrawingService DrawingService { get; private set; }
    }
}