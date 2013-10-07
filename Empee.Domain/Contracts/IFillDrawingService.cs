using SharpDX;

namespace Empee.Domain.Contracts
{
    public interface IFillDrawingService : IOutlineDrawingService
    {
        Color4 FillColor { get; set; }

        void Fill();
        
        void FillOutline();
    }
}