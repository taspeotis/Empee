using Empee.Domain.Contracts;
using SharpDX.Direct2D1;

namespace Empee.Domain.Providers
{
    internal sealed class DrawingService : DrawingAttributes, IDrawingService
    {
        private readonly RenderTarget _renderTarget;

        internal DrawingService(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public ICircleDrawingService Circle()
        {
            return new CircleDrawingService(Ellipse());
        }

        public IEllipseDrawingService Ellipse()
        {
            return new EllipseDrawingService(this, _renderTarget);
        }

        public ILineDrawingService Line()
        {
            return new LineDrawingService(this, _renderTarget);
        }

        public IPolygonDrawingService Polygon()
        {
            return new PolygonDrawingService(this, _renderTarget);
        }
    }
}