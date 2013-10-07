using SharpDX;

namespace Empee.Domain.Contracts
{
    public interface IOutlineDrawingService : IDrawingAttributes
    {
        Color4 OutlineColor { get; set; }

        float? StrokeWidth { get; set; }

        void Outline();
    }
}