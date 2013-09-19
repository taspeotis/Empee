﻿using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using Empee.Domain.Contracts;

namespace Empee.Domain.Providers
{
    [Export(typeof (IDefaultBehaviors))]
    internal sealed class DefaultBehaviors : IDefaultBehaviors
    {
        private readonly IContext _context;
        private readonly IExecutionLoopService _executionLoopService;
        private readonly IRenderService _renderService;

        [ImportingConstructor]
        public DefaultBehaviors(
            IContext context,
            IExecutionLoopService executionLoopService,
            IRenderService renderService)
        {
            _context = context;
            _executionLoopService = executionLoopService;
            _renderService = renderService;
        }

        public void AcceptDefaultBehaviors(string controlText)
        {
            AcceptDefaultRenderControl(controlText);
            AcceptDefaultResize();
            AcceptDefaultToggleFullScreen();

            _executionLoopService.Run();
        }

        public void AcceptDefaultRenderControl(string controlText)
        {
            _context.RenderControl = new Form {Text = controlText};
        }

        public void AcceptDefaultResize()
        {
            _context.RenderControl.Resize += RenderControlOnResize;
        }

        public void AcceptDefaultToggleFullScreen()
        {
            _context.RenderControl.KeyDown += RenderControlOnKeyDown;
        }

        private void RenderControlOnResize(object sender, EventArgs eventArgs)
        {
            _renderService.Resize();
        }

        private void RenderControlOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Alt && keyEventArgs.KeyCode == Keys.Enter)
                _renderService.ToggleFullScreen();
        }
    }
}