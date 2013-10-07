using Empee.Domain.Contracts;
using SharpDX;

namespace Empee.Domain.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class ILineDrawingServiceExtensions
    {
        public static T SetFrom<T>(this T lineDrawingService, Vector2 from)
            where T : ILineDrawingService
        {
            lineDrawingService.From = from;

            return lineDrawingService;
        }

        public static T SetFrom<T>(this T lineDrawingService, float x, float y)
            where T : ILineDrawingService
        {
            lineDrawingService.From = new Vector2(x, y);

            return lineDrawingService;
        }

        public static T SetTo<T>(this T lineDrawingService, Vector2 to)
            where T : ILineDrawingService
        {
            lineDrawingService.To = to;

            return lineDrawingService;
        }

        public static T SetTo<T>(this T lineDrawingService, float x, float y)
            where T : ILineDrawingService
        {
            lineDrawingService.To = new Vector2(x, y);

            return lineDrawingService;
        }
    }
}