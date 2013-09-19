using System.Windows.Forms;
using Empee.Domain.Infrastructure;

namespace Empee.Domain.Contracts
{
    public interface IExecutionLoopService
    {
        void Run();

        event StartingEventHandler Starting;

        event ExecutingEventHandler Executing;

        event StoppingEventHandler Stopping;
    }
}