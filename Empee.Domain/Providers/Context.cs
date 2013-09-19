using System.ComponentModel.Composition;
using System.Windows.Forms;
using Empee.Domain.Contracts;

namespace Empee.Domain.Providers
{
    [Export(typeof(IContext))]
    internal sealed class Context : IContext
    {
        public Control RenderControl { get; set; }
    }
}
