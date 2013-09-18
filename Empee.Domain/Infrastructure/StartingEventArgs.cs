using System;
using System.Windows.Forms;

namespace Empee.Domain.Infrastructure
{
    public class StartingEventArgs : EventArgs
    {
        public StartingEventArgs(Control renderControl)
        {
            RenderControl = renderControl;
        }

        public Control RenderControl { get; private set; }
    }
}