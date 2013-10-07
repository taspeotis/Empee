using System;

namespace Empee.Domain.Contracts
{
    public interface IDrawingAttributes
    {
        Func<float, float> ScaleMagnitude { get; set; }

        Func<float, float> ScaleXMagnitude { get; set; }

        Func<float, float> ScaleYMagnitude { get; set; }

        Func<float, float> TranslateXCoordinate { get; set; }

        Func<float, float> TranslateYCoordinate { get; set; }
    }
}