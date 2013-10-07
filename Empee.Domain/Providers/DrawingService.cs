using System;
using Empee.Domain.Contracts;
using SharpDX;
using SharpDX.Direct2D1;

namespace Empee.Domain.Providers
{
    internal sealed class DrawingService : IDrawingService
    {
        private readonly RenderTarget _renderTarget;

        private Func<float, float> _scaleMagnitude = TransformIdentity;
        private Func<float, float> _scaleXMagnitude = TransformIdentity;
        private Func<float, float> _scaleYMagnitude = TransformIdentity;
        private Func<float, float> _translateXCoordinate = TransformIdentity;
        private Func<float, float> _translateYCoordinate = TransformIdentity;

        public DrawingService(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public Color4 DrawColor { get; set; }

        public Func<float, float> ScaleMagnitude
        {
            get { return _scaleMagnitude; }
            set { _scaleMagnitude = value ?? TransformIdentity; }
        }

        public Func<float, float> ScaleXMagnitude
        {
            get { return _scaleXMagnitude; }
            set { _scaleXMagnitude = value ?? TransformIdentity; }
        }

        public Func<float, float> ScaleYMagnitude
        {
            get { return _scaleYMagnitude; }
            set { _scaleYMagnitude = value ?? TransformIdentity; }
        }

        public Func<float, float> TranslateXCoordinate
        {
            get { return _translateXCoordinate; }
            set { _translateXCoordinate = value ?? TransformIdentity; }
        }

        public Func<float, float> TranslateYCoordinate
        {
            get { return _translateYCoordinate; }
            set { _translateYCoordinate = value ?? TransformIdentity; }
        }

        public void DrawCircle(float xCenter, float yCenter, float radius)
        {
            DrawEllipse(xCenter, yCenter, radius, radius);
        }

        public void FillCircle(float xCenter, float yCenter, float radius)
        {
            FillEllipse(xCenter, yCenter, radius, radius);
        }

        public void FillDrawCircle(float xCenter, float yCenter, float radius)
        {
            FillCircle(xCenter, yCenter, radius);
            DrawCircle(xCenter, yCenter, radius);
        }

        public void DrawEllipse(float xCenter, float yCenter, float xRadius, float yRadius)
        {
            var ellipse = MakeEllipse(xCenter, yCenter, xRadius, yRadius);

            using (var brush = new SolidColorBrush(_renderTarget, DrawColor))
                _renderTarget.DrawEllipse(ellipse, brush);
        }

        public void FillEllipse(float xCenter, float yCenter, float xRadius, float yRadius)
        {
            var ellipse = MakeEllipse(xCenter, yCenter, xRadius, yRadius);

            using (var brush = new SolidColorBrush(_renderTarget, DrawColor))
                _renderTarget.FillEllipse(ellipse, brush);
        }

        public void FillDrawEllipse(float xCenter, float yCenter, float xRadius, float yRadius)
        {
            FillEllipse(xCenter, yCenter, xRadius, yRadius);
            DrawEllipse(xCenter, yCenter, xRadius, yRadius);
        }

        public void DrawLine(float xFrom, float yFrom, float xTo, float yTo, float? strokeWidth = null)
        {
            var fromVector = MakeVector2(xFrom, yFrom);
            var toVector = MakeVector2(xTo, yTo);

            using (var brush = new SolidColorBrush(_renderTarget, DrawColor))
            {
                // TODO: Get Direct2D 1.1 NuGet packages and use StrokeStyle1 to actually draw 1px 
                strokeWidth = strokeWidth == null ? 2.0f : ScaleMagnitude((float) strokeWidth);

                _renderTarget.DrawLine(fromVector, toVector, brush, (float) strokeWidth);
            }
        }

        private static float TransformIdentity(float value)
        {
            return value;
        }

        private Ellipse MakeEllipse(float xCenter, float yCenter, float xRadius, float yRadius)
        {
            var centerVector = MakeVector2(xCenter, yCenter);

            xRadius = ScaleXMagnitude(xRadius);
            yRadius = ScaleYMagnitude(yRadius);

            return new Ellipse(centerVector, xRadius, yRadius);
        }

        private Vector2 MakeVector2(float x, float y)
        {
            x = TranslateXCoordinate(x);
            y = TranslateYCoordinate(y);

            return new Vector2(x, y);
        }
    }
}