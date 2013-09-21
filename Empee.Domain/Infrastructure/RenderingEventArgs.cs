using System;
using Empee.Domain.Contracts;

namespace Empee.Domain.Infrastructure
{
    public sealed class RenderingEventArgs : EventArgs
    {
        public RenderingEventArgs(IDrawingOperations drawingOperations)
        {
            DrawingOperations = drawingOperations;
        }

        public IDrawingOperations DrawingOperations { get; private set; }
    }
}