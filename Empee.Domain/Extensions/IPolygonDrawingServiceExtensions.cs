using System.Collections.Generic;
using System.Linq;
using Empee.Domain.Contracts;
using SharpDX;
using XnaVector2 = Microsoft.Xna.Framework.Vector2;

namespace Empee.Domain.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IPolygonDrawingServiceExtensions
    {
        public static T SetPoints<T>(this T polygonDrawingService, IEnumerable<Vector2> points)
            where T : IPolygonDrawingService
        {
            polygonDrawingService.Points = points;

            return polygonDrawingService;
        }

        public static T SetPoints<T>(this T polygonDrawingService, IEnumerable<XnaVector2> points)
            where T : IPolygonDrawingService
        {
            var pointsList = points.Select(p => new Vector2(p.X, p.Y)).ToList();

            return polygonDrawingService.SetPoints(pointsList);
        }

        public static T SetOpen<T>(this T polygonDrawingService, bool open)
            where T : IPolygonDrawingService
        {
            polygonDrawingService.Open = open;

            return polygonDrawingService;
        }
    }
}