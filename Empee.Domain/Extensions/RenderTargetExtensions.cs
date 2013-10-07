using System;
using SharpDX;
using SharpDX.Direct2D1;

namespace Empee.Domain.Extensions
{
    internal static class RenderTargetExtensions
    {
        public static void WithBrush(this RenderTarget renderTarget, Color4 brushColor, Action<Brush> brushAction)
        {
            using (var brush = new SolidColorBrush(renderTarget, brushColor))
                brushAction(brush);
        }
    }
}