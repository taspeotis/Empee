using Empee.Domain.Contracts;
using SharpDX;

namespace Empee.Domain.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IOutlineDrawingServiceExtensions
    {
        public static T SetOutlineColor<T>(this T outlineDrawingService, Color4 outlineColor)
            where T : IOutlineDrawingService
        {
            outlineDrawingService.OutlineColor = outlineColor;

            return outlineDrawingService;
        }
    }
}
