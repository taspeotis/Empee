using Empee.Domain.Contracts;
using SharpDX;
using SharpDX.Direct2D1;

namespace Empee.Domain.Providers
{
    internal sealed class LineDrawingService : OutlineDrawingService, ILineDrawingService
    {
        private readonly RenderTarget _renderTarget;

        public LineDrawingService(IDrawingAttributes drawingAttributes, RenderTarget renderTarget)
            : base(drawingAttributes, renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public Vector2 From { get; set; }

        public Vector2 To { get; set; }

        protected override void OutlineInternal(Brush outlineBrush)
        {
            var translatedFrom = MakeTranslatedVector(From);
            var translatedTo = MakeTranslatedVector(To);

            _renderTarget.DrawLine(translatedFrom, translatedTo, outlineBrush);
        }

        private Vector2 MakeTranslatedVector(Vector2 vector)
        {
            return new Vector2
            {
                X = TranslateXCoordinate(vector.X),
                Y = TranslateYCoordinate(vector.Y)
            };
        }
    }
}