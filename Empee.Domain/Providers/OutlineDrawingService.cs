using Empee.Domain.Contracts;
using Empee.Domain.Extensions;
using SharpDX;
using SharpDX.Direct2D1;

namespace Empee.Domain.Providers
{
    internal abstract class OutlineDrawingService : DrawingAttributes, IOutlineDrawingService
    {
        private readonly RenderTarget _renderTarget;

        protected OutlineDrawingService(IDrawingAttributes drawingAttributes, RenderTarget renderTarget)
            : base(drawingAttributes)
        {
            _renderTarget = renderTarget;
        }

        public Color4 OutlineColor { get; set; }

        public float? StrokeWidth { get; set; }

        public void Outline()
        {
            _renderTarget.StrokeWidth = StrokeWidth == null ? 2.0f : ScaleMagnitude((float) StrokeWidth);

            _renderTarget.WithBrush(OutlineColor, OutlineInternal);
        }

        protected abstract void OutlineInternal(Brush outlineBrush);
    }
}