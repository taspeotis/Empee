namespace Empee.Domain.Contracts
{
    public interface IDrawingService : IDrawingAttributes
    {
        ICircleDrawingService Circle();

        IEllipseDrawingService Ellipse();

        ILineDrawingService Line();

        IPolygonDrawingService Polygon();
    }
}
