using System;
using Empee.Domain.Contracts;
using SharpDX;
using SharpDX.Direct2D1;

namespace Empee.Domain.Providers
{
    internal sealed class DrawingService : IDrawingService
    {
        private readonly RenderTarget _renderTarget;

        private Func<float, float> _transformXCoordinate = TransformCoordinateIdentity;

        private Func<float, float> _transformYCoordinate = TransformCoordinateIdentity;

        public DrawingService(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public Func<float, float> TransformXCoordinate
        {
            get { return _transformXCoordinate; }
            set { _transformXCoordinate = value ?? TransformCoordinateIdentity; }
        }

        public Func<float, float> TransformYCoordinate
        {
            get { return _transformYCoordinate; }
            set { _transformYCoordinate = value ?? TransformCoordinateIdentity; }
        }

        public void DrawCircle(float x, float y, float radius)
        {
            var ellipse = MakeEllipse(x, y, radius);

            using (var brush = new SolidColorBrush(_renderTarget, Color4.Black))
                _renderTarget.DrawEllipse(ellipse, brush);
        }

        public void FillCircle(float x, float y, float radius)
        {
            var ellipse = MakeEllipse(x, y, radius);

            using (var brush = new SolidColorBrush(_renderTarget, Color4.Black))
                _renderTarget.FillEllipse(ellipse, brush);
        }

        public void FillDrawCircle(float x, float y, float radius)
        {
            FillCircle(x, y, radius);
            DrawCircle(x, y, radius);
        }

        private Ellipse MakeEllipse(float x, float y, float radius)
        {
            var vector = MakeVector2(x, y);

            return new Ellipse(vector, radius, radius);
        }

        private static float TransformCoordinateIdentity(float coordinate)
        {
            return coordinate;
        }

        private Vector2 MakeVector2(float x, float y)
        {
            x = TransformXCoordinate(x);
            y = TransformYCoordinate(y);

            return new Vector2(x, y);
        }
    }
}