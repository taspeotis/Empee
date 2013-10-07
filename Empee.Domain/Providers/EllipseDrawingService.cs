using Empee.Domain.Contracts;
using SharpDX;
using SharpDX.Direct2D1;

namespace Empee.Domain.Providers
{
    internal sealed class EllipseDrawingService : FillDrawingService, IEllipseDrawingService
    {
        private readonly RenderTarget _renderTarget;

        public EllipseDrawingService(IDrawingAttributes drawingAttributes, RenderTarget renderTarget)
            : base(drawingAttributes, renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public Vector2 Center { get; set; }

        public float RadiusX { get; set; }

        public float RadiusY { get; set; }

        protected override void OutlineInternal(Brush drawBrush)
        {
            var ellipse = MakeEllipse();

            _renderTarget.DrawEllipse(ellipse, drawBrush);
        }

        private Ellipse MakeEllipse()
        {
            var translatedCenter = new Vector2
            {
                X = TranslateXCoordinate(Center.X),
                Y = TranslateYCoordinate(Center.Y)
            };

            var scaledRadiusX = ScaleXMagnitude(RadiusX);
            var scaledRadiusY = ScaleYMagnitude(RadiusY);

            return new Ellipse(translatedCenter, scaledRadiusX, scaledRadiusY);
        }

        protected override void FillInternal(Brush fillBrush)
        {
            var ellipse = MakeEllipse();

            _renderTarget.FillEllipse(ellipse, fillBrush);
        }
    }
}