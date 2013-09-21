using System.Windows.Forms;

namespace Empee.Domain.Contracts
{
    // TODO: We could always get these values OUT of this interface and put them in their respective
    // services (e.g. RenderControl => IRenderService). Keep IContext for properly shared variables.
    public interface IContext
    {
        // TODO: Implement INotifyPropertyChanged for this (or make it a collection?)
        // So that the graphics context can be created/destroyed as necessary.

        Control RenderControl { get; set; }
    }
}