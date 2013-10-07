using System;
using Empee.Domain.Contracts;
using SharpDX;

namespace Empee.Domain.Providers
{
    internal sealed class CircleDrawingService : ICircleDrawingService
    {
        private readonly IEllipseDrawingService _ellipseDrawingService;

        public CircleDrawingService(IEllipseDrawingService ellipseDrawingService)
        {
            _ellipseDrawingService = ellipseDrawingService;
        }

        public void FillOutline()
        {
            _ellipseDrawingService.FillOutline();
        }

        public void Outline()
        {
            _ellipseDrawingService.Outline();
        }

        public float? StrokeWidth
        {
            get { return _ellipseDrawingService.StrokeWidth; }
            set { _ellipseDrawingService.StrokeWidth = value; }
        }

        public Color4 OutlineColor
        {
            get { return _ellipseDrawingService.OutlineColor; }
            set { _ellipseDrawingService.OutlineColor = value; }
        }

        public void Fill()
        {
            _ellipseDrawingService.Fill();
        }

        public Color4 FillColor
        {
            get { return _ellipseDrawingService.FillColor; }
            set { _ellipseDrawingService.FillColor = value; }
        }

        public Func<float, float> TranslateYCoordinate
        {
            get { return _ellipseDrawingService.TranslateYCoordinate; }
            set { _ellipseDrawingService.TranslateYCoordinate = value; }
        }

        public Func<float, float> TranslateXCoordinate
        {
            get { return _ellipseDrawingService.TranslateXCoordinate; }
            set { _ellipseDrawingService.TranslateXCoordinate = value; }
        }

        public Func<float, float> ScaleYMagnitude
        {
            get { return _ellipseDrawingService.ScaleYMagnitude; }
            set { _ellipseDrawingService.ScaleYMagnitude = value; }
        }

        public Func<float, float> ScaleXMagnitude
        {
            get { return _ellipseDrawingService.ScaleXMagnitude; }
            set { _ellipseDrawingService.ScaleXMagnitude = value; }
        }

        public Func<float, float> ScaleMagnitude
        {
            get { return _ellipseDrawingService.ScaleMagnitude; }
            set { _ellipseDrawingService.ScaleMagnitude = value; }
        }


        public Vector2 Center
        {
            get { return _ellipseDrawingService.Center; }
            set { _ellipseDrawingService.Center = value; }
        }

        public float Radius
        {
            get { return _ellipseDrawingService.RadiusX; }

            set
            {
                _ellipseDrawingService.RadiusX = value;
                _ellipseDrawingService.RadiusY = value;
            }
        }
    }
}