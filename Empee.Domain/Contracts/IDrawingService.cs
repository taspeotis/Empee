using System;

namespace Empee.Domain.Contracts
{
    public interface IDrawingService
    {
        /* TODO: Brushes */

        Func<float, float> TransformXCoordinate { get; set; } 

        Func<float, float> TransformYCoordinate { get; set; }

        void DrawCircle(float x, float y, float radius);

        void FillCircle(float x, float y, float radius);

        void FillDrawCircle(float x, float y, float radius);
    }
}