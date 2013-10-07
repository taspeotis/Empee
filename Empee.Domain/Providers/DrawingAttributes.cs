using System;
using Empee.Domain.Contracts;

namespace Empee.Domain.Providers
{
    internal abstract class DrawingAttributes : IDrawingAttributes
    {
        // TODO: Consider a OutlineDrawingAttributes, FillDrawingAttributes interfaces and classes

        private Func<float, float> _scaleMagnitude;
        private Func<float, float> _scaleXMagnitude;
        private Func<float, float> _scaleYMagnitude;
        private Func<float, float> _translateXCoordinate;
        private Func<float, float> _translateYCoordinate;

        protected DrawingAttributes()
        {
            ScaleMagnitude = TransformIdentity;
            ScaleXMagnitude = TransformIdentity;
            ScaleYMagnitude = TransformIdentity;
            TranslateXCoordinate = TransformIdentity;
            TranslateYCoordinate = TransformIdentity;
        }

        protected DrawingAttributes(IDrawingAttributes drawingAttributes)
        {
            ScaleMagnitude = drawingAttributes.ScaleMagnitude;
            ScaleXMagnitude = drawingAttributes.ScaleXMagnitude;
            ScaleYMagnitude = drawingAttributes.ScaleYMagnitude;
            TranslateXCoordinate = drawingAttributes.TranslateXCoordinate;
            TranslateYCoordinate = drawingAttributes.TranslateYCoordinate;
        }

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

        private static float TransformIdentity(float value)
        {
            return value;
        }
    }
}