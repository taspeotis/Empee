using System;
using System.Windows.Forms;
using Empee.Domain.Contracts;
using Empee.Infrastructure;

namespace Showcase
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var compositionContainer = Bootstrapper.CreateCompositionContainer())
            {
                var defaultBehaviors = compositionContainer.GetExportedValue<IDefaultBehaviors>();

                defaultBehaviors.AcceptDefaultBehaviors("Empee Showcase");
            }
        }
    }
}