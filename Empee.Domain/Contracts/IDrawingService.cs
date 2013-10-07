using System;
using SharpDX;

namespace Empee.Domain.Contracts
{
    public interface IDrawingService
    {
        /* TODO: Brushes */
        Color4 DrawColor { get; set; }

        /* These really should be matrixes? */
        Func<float, float> ScaleMagnitude { get; set; }

        Func<float, float> ScaleXMagnitude { get; set; }

        Func<float, float> ScaleYMagnitude { get; set; }

        Func<float, float> TranslateXCoordinate { get; set; } 

        Func<float, float> TranslateYCoordinate { get; set; }

        void DrawCircle(float xCenter, float yCenter, float radius);

        void FillCircle(float xCenter, float yCenter, float radius);

        void FillDrawCircle(float xCenter, float yCenter, float radius);

        void DrawEllipse(float xCenter, float yCenter, float xRadius, float yRadius);

        void FillEllipse(float xCenter, float yCenter, float xRadius, float yRadius);

        void FillDrawEllipse(float xCenter, float yCenter, float xRadius, float yRadius);

        /// <remarks>strokeWidth = null to draw 1px</remarks>
        void DrawLine(float xFrom, float yFrom, float xTo, float yTo, float? strokeWidth = null);
    }
}