using System.Windows.Forms;

namespace Empee.Domain.Contracts
{
    public interface IContext
    {
        // TODO: Implement INotifyPropertyChanged for this (or make it a collection?)
        // So that the graphics context can be created/destroyed as necessary.

        Control RenderControl { get; set; } 
    }
}