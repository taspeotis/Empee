using Empee.Domain.Infrastructure;

namespace Empee.Domain.Contracts
{
    public interface IRenderService
    {
        void Resize();

        void ToggleFullScreen();

        event RenderingEventHandler Rendering;
    }
}