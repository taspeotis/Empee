using System;
using Empee.Domain.Contracts;

namespace Empee.Domain.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IDrawingAttributesExtensions
    {
        public static T SetScaleMagnitude<T>(this T drawingAttributes, Func<float, float> scaleMagnitude)
            where T : IDrawingAttributes
        {
            drawingAttributes.ScaleMagnitude = scaleMagnitude;

            return drawingAttributes;
        }

        public static T SetScaleXMagnitude<T>(this T drawingAttributes, Func<float, float> scaleXMagnitude)
            where T : IDrawingAttributes
        {
            drawingAttributes.ScaleXMagnitude = scaleXMagnitude;

            return drawingAttributes;
        }

        public static T SetScaleYMagnitude<T>(this T drawingAttributes, Func<float, float> scaleYMagnitude)
            where T : IDrawingAttributes
        {
            drawingAttributes.ScaleYMagnitude = scaleYMagnitude;

            return drawingAttributes;
        }

        public static T SetTranslateXCoordinate<T>(this T drawingAttributes, Func<float, float> translateXCoordinate)
            where T : IDrawingAttributes
        {
            drawingAttributes.TranslateXCoordinate = translateXCoordinate;

            return drawingAttributes;
        }

        public static T SetTranslateYCoordinate<T>(this T drawingAttributes, Func<float, float> translateYCoordinate)
            where T : IDrawingAttributes
        {
            drawingAttributes.TranslateYCoordinate = translateYCoordinate;

            return drawingAttributes;
        }
    }
}
