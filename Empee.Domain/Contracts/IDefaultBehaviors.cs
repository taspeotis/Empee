namespace Empee.Domain.Contracts
{
    public interface IDefaultBehaviors
    {
        void AcceptDefaultBehaviors(string controlText);

        // TODO: This thing will wire up the gfx to the event loop
        // ALT+ENTER from the input service to the gfx service

        void AcceptDefaultRenderControl(string controlText);

        void AcceptDefaultResize();

        void AcceptDefaultToggleFullScreen();

        void AcceptDefaultGravity();
    }
}