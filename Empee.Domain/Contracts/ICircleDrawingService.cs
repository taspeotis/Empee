using SharpDX;

namespace Empee.Domain.Contracts
{
    public interface ICircleDrawingService : IFillDrawingService
    {
        Vector2 Center { get; set; }

        float Radius { get; set; }
    }
}
