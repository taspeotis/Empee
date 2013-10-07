using Empee.Domain.Contracts;
using Empee.Domain.Extensions;
using SharpDX;
using SharpDX.Direct2D1;

namespace Empee.Domain.Providers
{
    internal abstract class FillDrawingService : OutlineDrawingService
    {
        private readonly RenderTarget _renderTarget;

        internal FillDrawingService(IDrawingAttributes drawingAttributes, RenderTarget renderTarget)
            : base(drawingAttributes, renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public Color4 FillColor { get; set; }

        public void Fill()
        {
            _renderTarget.WithBrush(FillColor, FillInternal);
        }

        public void FillOutline()
        {
            Fill();
            Outline();
        }

        protected abstract void FillInternal(Brush fillBrush);
    }
}