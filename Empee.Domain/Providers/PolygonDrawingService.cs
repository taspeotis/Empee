using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Empee.Domain.Contracts;
using SharpDX;
using SharpDX.Direct2D1;

namespace Empee.Domain.Providers
{
    internal class PolygonDrawingService : FillDrawingService, IPolygonDrawingService
    {
        private readonly RenderTarget _renderTarget;

        public PolygonDrawingService(IDrawingAttributes drawingAttributes, RenderTarget renderTarget)
            : base(drawingAttributes, renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public IEnumerable<Vector2> Points { get; set; }

        public bool Open { get; set; }

        protected override void OutlineInternal(Brush outlineBrush)
        {
            Polygon(FigureBegin.Hollow, g => _renderTarget.DrawGeometry(g, outlineBrush));
        }

        protected override void FillInternal(Brush fillBrush)
        {
            Polygon(FigureBegin.Filled, g => _renderTarget.FillGeometry(g, fillBrush));
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private void Polygon(FigureBegin figureBegin, Action<Geometry> geometryAction)
        {
            using (var direct2DFactory = _renderTarget.Factory)
            using (var pathGeometry = new PathGeometry(direct2DFactory))
            {
                using (var geometrySink = pathGeometry.Open())
                {
                    var translatedPoints = Points.Select(TranslatePoint).ToList();
                    var firstVector = translatedPoints.First();
                    var remainingVectors = translatedPoints.Skip(1).ToArray();

                    geometrySink.SetFillMode(FillMode.Winding);
                    geometrySink.BeginFigure(firstVector, figureBegin);
                    geometrySink.AddLines(remainingVectors);
                    geometrySink.EndFigure(Open ? FigureEnd.Open : FigureEnd.Closed);

                    // CA2202 Do not dispose objects multiple times
                    // Note that calling "Close" in this sense is not the same as "Dispose"
                    // http://msdn.microsoft.com/en-us/library/windows/desktop/dd316932(v=vs.85).aspx
                    // Observe that both "Close" and "Release" (i.e. "Dispose") need to be called.
                    geometrySink.Close();
                }

                geometryAction(pathGeometry);
            }
        }

        private Vector2 TranslatePoint(Vector2 value)
        {
            return new Vector2
            {
                X = TranslateXCoordinate(value.X),
                Y = TranslateYCoordinate(value.Y)
            };
        }
    }
}