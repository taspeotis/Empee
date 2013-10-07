using SharpDX;

namespace Empee.Domain.Contracts
{
    public interface ILineDrawingService : IOutlineDrawingService
    {
        Vector2 From { get; set; }

        Vector2 To { get; set; }
    }
}