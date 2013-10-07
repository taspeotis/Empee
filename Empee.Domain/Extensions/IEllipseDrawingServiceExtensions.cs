using Empee.Domain.Contracts;
using SharpDX;

namespace Empee.Domain.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IEllipseDrawingServiceExtensions
    {
        /// <remarks>SetCenter would conflict with <c>ICircleDrawingServiceExtensions.SetCircleCenter"</c></remarks>
        public static T SetEllipseCenter<T>(this T ellipseDrawingService, Vector2 center)
            where T : IEllipseDrawingService
        {
            ellipseDrawingService.Center = center;

            return ellipseDrawingService;
        }

        /// <remarks>SetCenter would conflict with <c>ICircleDrawingServiceExtensions.SetCircleCenter"</c></remarks>
        public static T SetEllipseCenter<T>(this T ellipseDrawingService, float x, float y)
            where T : IEllipseDrawingService
        {
            ellipseDrawingService.Center = new Vector2(x, y);

            return ellipseDrawingService;
        }

        public static T SetRadiusX<T>(this T ellipseDrawingService, float radius)
            where T : IEllipseDrawingService
        {
            ellipseDrawingService.RadiusX = radius;

            return ellipseDrawingService;
        }

        public static T SetRadiusY<T>(this T ellipseDrawingService, float radius)
            where T : IEllipseDrawingService
        {
            ellipseDrawingService.RadiusY = radius;

            return ellipseDrawingService;
        }
    }
}