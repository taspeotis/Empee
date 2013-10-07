using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using Empee.Domain.Contracts;
using Empee.Domain.Infrastructure;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using Color = SharpDX.Color;
using Device = SharpDX.Direct3D11.Device;
using Direct2DFactory = SharpDX.Direct2D1.Factory;
using DXGIFactory = SharpDX.DXGI.Factory;
using DirectWriteFactory = SharpDX.DirectWrite.Factory;
using FeatureLevel = SharpDX.Direct2D1.FeatureLevel;
using RectangleF = SharpDX.RectangleF;

namespace Empee.Domain.Providers
{
    [Export(typeof (IRenderService))]
    internal sealed class RenderService : IRenderService, IDisposable
    {
        private const SwapChainFlags DefaultSwapChainFlags = SwapChainFlags.AllowModeSwitch;

        private Device _device;

        private Control _renderControl;

        private RenderTarget _renderTarget;

        private SwapChain _swapChain;

        [ImportingConstructor]
        public RenderService(IExecutionLoopService executionLoopService)
        {
            executionLoopService.Starting += ExecutionLoopServiceOnStarting;
            executionLoopService.Executing += ExecutionLoopServiceOnExecuting;
            executionLoopService.Stopping += ExecutionLoopServiceOnStopping;
        }

        public void Dispose()
        {
            CleanUp();
        }

        public void ToggleFullScreen()
        {
            if (_swapChain.IsFullScreen)
            {
                _swapChain.IsFullScreen = false;

                // TODO: Come back from previous dimensions
                // Come back from previous dimensions
                _renderControl.Size = new Size(400, 400);
            }
            else
            {
                var screen = Screen.FromControl(_renderControl);

                _renderControl.ClientSize = screen.Bounds.Size;
                // TODO: If we're going full screen, first resize the window the the full size
                // of the monitor it's on. Also, store the Rectangle for going un-full-screen.    

                _swapChain.IsFullScreen = true;
            }
        }

        public event RenderingEventHandler Rendering;

        public void Resize()
        {
            DisposeRenderTarget();

            _swapChain.ResizeBuffers(0, 0, 0, Format.Unknown, DefaultSwapChainFlags);

            using (var backBuffer = Surface.FromSwapChain(_swapChain, 0))
                CreateRenderTarget(backBuffer);
        }

        private void ExecutionLoopServiceOnStarting(object sender, StartingEventArgs startingEventArgs)
        {
            _renderControl = startingEventArgs.RenderControl;

            try
            {
                CreateDeviceAndSwapChainOrNull();
            }
            catch (Exception)
            {
                _renderControl = null;
            }
        }

        private void CreateDeviceAndSwapChainOrNull()
        {
            try
            {
                var swapChainDescription = new SwapChainDescription
                {
                    BufferCount = 1,
                    Flags = DefaultSwapChainFlags,
                    IsWindowed = true,
                    ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                    OutputHandle = _renderControl.Handle,
                    SampleDescription = new SampleDescription(1, 0),
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput
                };

                // Direct2D requires BGRA
                Device.CreateWithSwapChain(
                    DriverType.Hardware, DeviceCreationFlags.BgraSupport,
                    swapChainDescription, out _device, out _swapChain);

                // DRY => Surface.FromSwapChain
                using (var backBuffer = Surface.FromSwapChain(_swapChain, 0))
                    CreateRenderTargetOrNull(backBuffer);
            }
            catch (Exception)
            {
                DisposeSwapChain();
                DisposeDevice();
            }
        }

        private void CreateRenderTargetOrNull(Surface backBuffer)
        {
            try
            {
                CreateRenderTarget(backBuffer);

                MakeWindowAssociation();
            }
            catch (Exception)
            {
                DisposeRenderTarget();
            }
        }

        private void CreateRenderTarget(Surface backBuffer)
        {
            using (var direct2DFactory = new Direct2DFactory())
            {
                var desktopDpi = direct2DFactory.DesktopDpi;

                var renderTargetProperties = new RenderTargetProperties
                {
                    DpiX = desktopDpi.Width,
                    DpiY = desktopDpi.Height,
                    MinLevel = FeatureLevel.Level_DEFAULT,
                    PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Ignore),
                    Type = RenderTargetType.Default,
                    Usage = RenderTargetUsage.None
                };

                _renderTarget = new RenderTarget(direct2DFactory, backBuffer, renderTargetProperties);
            }
        }

        private void MakeWindowAssociation()
        {
            // Allegedly DXGI doesn't play nice with WinForms: http://slimdx.org/tutorials/DeviceCreation.php
            using (var dxgiFactory = _swapChain.GetParent<DXGIFactory>())
                dxgiFactory.MakeWindowAssociation(_renderControl.Handle, WindowAssociationFlags.IgnoreAltEnter);
        }

        private void OnRendering(RenderingEventArgs renderingEventArgs)
        {
            var eventHandler = Rendering;

            if (eventHandler != null)
                eventHandler(this, renderingEventArgs);
        }

        private void ExecutionLoopServiceOnExecuting(object sender, ExecutingEventArgs executingEventArgs)
        {
            var executionDelta = executingEventArgs.ExecutingDelta;
            var framesPerSecond = executionDelta > 0 ? 1/executionDelta : 0;

            _renderTarget.BeginDraw();
            _renderTarget.Transform = Matrix3x2.Identity;
            _renderTarget.Clear(Color.White);

            OnRendering(new RenderingEventArgs(new DrawingService(_renderTarget)));

            if (framesPerSecond > 0)
            {
                using (var directWriteFactory = new DirectWriteFactory())
                using (var textFormat = new TextFormat(directWriteFactory, "Courier New", 12))
                using (var blackBrush = new SolidColorBrush(_renderTarget, Color4.Black))
                {
                    _renderTarget.DrawText(
                        String.Format("{0} FPS", (int) framesPerSecond),
                        textFormat, new RectangleF(0, 0, 200, 200), blackBrush);
                }
            }

            _renderTarget.EndDraw();

            _swapChain.Present(1, PresentFlags.None);
        }

        private void ExecutionLoopServiceOnStopping(object sender, EventArgs eventArgs)
        {
            CleanUp();
        }

        private void CleanUp()
        {
            BreakWindowAssociation();
            DisposeRenderTarget();
            DisposeSwapChain();
            DisposeDevice();
        }

        private void BreakWindowAssociation()
        {
            if (_swapChain != null)
            {
                using (var dxgiFactory = _swapChain.GetParent<DXGIFactory>())
                    dxgiFactory.MakeWindowAssociation(IntPtr.Zero, WindowAssociationFlags.None);
            }
        }

        private void DisposeRenderTarget()
        {
            if (_renderTarget != null)
            {
                _renderTarget.Dispose();
                _renderTarget = null;
            }
        }

        private void DisposeSwapChain()
        {
            if (_swapChain != null)
            {
                _swapChain.Dispose();
                _swapChain = null;
            }
        }

        private void DisposeDevice()
        {
            if (_device != null)
            {
                _device.Dispose();
                _device = null;
            }
        }
    }
}