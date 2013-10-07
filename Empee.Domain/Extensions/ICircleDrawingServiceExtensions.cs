using Empee.Domain.Contracts;
using SharpDX;

namespace Empee.Domain.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class ICircleDrawingServiceExtensions
    {
        /// <remarks>SetCenter would conflict with <c>IEllipseDrawingServiceExtensions.SetEllipseCenter"</c></remarks>
        public static T SetCircleCenter<T>(this T circleDrawingService, Vector2 center)
            where T : ICircleDrawingService
        {
            circleDrawingService.Center = center;

            return circleDrawingService;
        }

        /// <remarks>SetCenter would conflict with <c>IEllipseDrawingServiceExtensions.SetEllipseCenter"</c></remarks>
        public static T SetCircleCenter<T>(this T circleDrawingService, float x, float y)
            where T : ICircleDrawingService
        {
            circleDrawingService.Center = new Vector2(x, y);

            return circleDrawingService;
        }

        public static T SetRadius<T>(this T circleDrawingService, float radius)
            where T : ICircleDrawingService
        {
            circleDrawingService.Radius = radius;

            return circleDrawingService;
        }
    }
}