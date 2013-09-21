using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Empee.Domain.Contracts;
using Empee.Domain.Infrastructure;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace Empee.Domain.Providers
{
    [Export(typeof (IPhysicsService))]
    internal sealed class PhysicsService : IPhysicsService
    {
        private readonly IContext _context;
        private Body _circleBody;

        private Body _ground;
        private World _world;

        [ImportingConstructor]
        public PhysicsService(
            IContext context,
            IExecutionLoopService executionLoopService,
            IRenderService renderService)
        {
            _context = context;

            executionLoopService.Starting += ExecutionLoopServiceOnStarting;
            executionLoopService.Executing += ExecutionLoopServiceOnExecuting;

            renderService.Rendering += RenderServiceOnRendering;
        }

        public float Gravity { get; set; }

        public bool Visible { get; set; }

        private void ExecutionLoopServiceOnStarting(object sender, StartingEventArgs startingEventArgs)
        {
            _world = new World(new Vector2(0, Gravity));

            _circleBody = BodyFactory.CreateCircle(_world, 1.0f, 1.0f, new Vector2(0, -50.0f));
            _circleBody.BodyType = BodyType.Dynamic;

            // Create the ground fixture
            _ground = BodyFactory.CreateEdge(_world, new Vector2(-1000, 0), new Vector2(1000, 0));
            _ground.IsStatic = true;
            _ground.Restitution = 0.3f;
            _ground.Friction = 0.5f;
        }

        private int LastSecond;

        private void ExecutionLoopServiceOnExecuting(object sender, ExecutingEventArgs executingEventArgs)
        {
            var executingDelta = (float) executingEventArgs.ExecutingDelta;

            _world.Step(executingDelta);

            var thisSecond = DateTime.Now.Second;
            var modSecond = thisSecond%3;
            thisSecond -= modSecond;

            if (thisSecond == LastSecond)
                return;

            try
            {
                if (LastSecond == 0)
                    return;

                var x = _circleBody.Position.X;

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (x == 0)
                {
                    if (BitConverter.DoubleToInt64Bits(x)%2 == 0)
                        x = 1;
                    else
                        x = -1;
                }

                // TODO: Bonus random multiplier between 0.5 and 2.5
                var random = new Random();
                var mult = (float) random.NextDouble()*2 + 0.5f;

                _circleBody.ApplyLinearImpulse(new Vector2(x > 0 ? -50f*mult : 50f*mult, -50f));
            }
            finally
            {
                LastSecond = thisSecond;                
            }
        }

        private void RenderServiceOnRendering(object sender, RenderingEventArgs renderingEventArgs)
        {
#if !DEBUG
            if (!Visible)
                return;
#endif

            var drawingService = renderingEventArgs.DrawingService;

            SetCoordinateTransforms(drawingService);

            drawingService.DrawCircle(
                _circleBody.Position.X,
                _circleBody.Position.Y,
                _circleBody.FixtureList[0].Shape.Radius*16.0f);
        }

        private void SetCoordinateTransforms(IDrawingService drawingService)
        {
            var viewport = _context.RenderControl.ClientSize;
            var viewportWidth = (float) viewport.Width;
            var viewportHeight = (float) viewport.Height;

            if (viewportWidth <= 0 || viewportHeight <= 0)
            {
                drawingService.TransformXCoordinate = null;
                drawingService.TransformYCoordinate = null;

                return;
            }

            // This should be in IContext??
            const float pixelsPerMeter = 16.0f;

            drawingService.TransformXCoordinate = x => viewportWidth/2 + x*pixelsPerMeter;
            drawingService.TransformYCoordinate = y => viewportHeight + y*pixelsPerMeter;
        }
    }
}