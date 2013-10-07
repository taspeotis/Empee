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

        /*


        public void DrawLine(float xFrom, float yFrom, float xTo, float yTo, float? strokeWidth = null)
        {
            var fromVector = MakeVector2(xFrom, yFrom);
            var toVector = MakeVector2(xTo, yTo);

            using (var brush = new SolidColorBrush(_renderTarget, DrawColor))
            {
                // TODO: Get Direct2D 1.1 use StrokeStyle1 to actually draw 1px
                strokeWidth = strokeWidth == null ? 2.0f : ScaleMagnitude((float) strokeWidth);

                _renderTarget.DrawLine(fromVector, toVector, brush, (float) strokeWidth);
            }
        }

        public void DrawPolygon(IEnumerable<PointF> points)
        {
            Polygon(points, FigureBegin.Hollow, FigureEnd.Closed, (rt, g, b) => rt.DrawGeometry(g, b));
        }

        public void FillPolygon(IEnumerable<PointF> points)
        {
            Polygon(points, FigureBegin.Filled, FigureEnd.Closed, (rt, g, b) => rt.FillGeometry(g, b));
        }

        public void FillDrawPolygon(IEnumerable<PointF> points)
        {
            var pointsList = points as IList<PointF> ?? points.ToList();

            FillPolygon(pointsList);
            DrawPolygon(pointsList);
        }

        private void Polygon(
            IEnumerable<PointF> points,
            FigureBegin figureBegin, FigureEnd figureEnd,
            Action<RenderTarget, Geometry, Brush> geometryAction)
        {
            var vectors = points.Select(p => MakeVector2(p.X, p.Y)).ToList();

            using (var direct2DFactory = _renderTarget.Factory)
            using (var pathGeometry = new PathGeometry(direct2DFactory))
            {
                using (var geometrySink = pathGeometry.Open())
                {
                    var firstVector = vectors.First();
                    var remainingVectors = vectors.Skip(1).ToArray();

                    geometrySink.SetFillMode(FillMode.Winding);
                    geometrySink.BeginFigure(firstVector, figureBegin);
                    geometrySink.AddLines(remainingVectors);
                    geometrySink.EndFigure(figureEnd);
                    geometrySink.Close();
                }

                using (var brush = new SolidColorBrush(_renderTarget, DrawColor))
                    geometryAction(_renderTarget, pathGeometry, brush);
            }
        }
        
        private Vector2 MakeVector2(float x, float y)
        {
            x = TranslateXCoordinate(x);
            y = TranslateYCoordinate(y);

            return new Vector2(x, y);
        }*/

    }
}