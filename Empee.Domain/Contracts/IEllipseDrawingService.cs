using SharpDX;

namespace Empee.Domain.Contracts
{
    public interface IEllipseDrawingService : IFillDrawingService
    {
        Vector2 Center { get; set; }

        float RadiusX { get; set; }

        float RadiusY { get; set; }
    }
}