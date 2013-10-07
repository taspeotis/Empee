using System.Collections.Generic;
using SharpDX;

namespace Empee.Domain.Contracts
{
    public interface IPolygonDrawingService : IFillDrawingService
    {
        IEnumerable<Vector2> Points { get; set; }

        bool Open { get; set; }
    }
}