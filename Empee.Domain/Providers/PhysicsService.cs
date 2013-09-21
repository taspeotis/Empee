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
        private World _world;

        private Body _circleBody;

        private Body _ground;

        [ImportingConstructor]
        public PhysicsService(IContext context, IExecutionLoopService executionLoopService)
        {
            executionLoopService.Starting += ExecutionLoopServiceOnStarting;
            executionLoopService.Executing += ExecutionLoopServiceOnExecuting;
        }

        private void ExecutionLoopServiceOnStarting(object sender, StartingEventArgs startingEventArgs)
        {
            _world = new World(new Vector2(0, Gravity));


            _circleBody = BodyFactory.CreateCircle(_world, 1.0f, 1.0f, new Vector2(0, -10.0f));
            _circleBody.BodyType = BodyType.Dynamic;

            // Create the ground fixture
            _ground = BodyFactory.CreateEdge(_world, new Vector2(-10, 0), new Vector2(10, 0));
            _ground.IsStatic = true;
            _ground.Restitution = 0.3f;
            _ground.Friction = 0.5f;
        }

        private void ExecutionLoopServiceOnExecuting(object sender, ExecutingEventArgs executingEventArgs)
        {
            var executingDelta = (float) executingEventArgs.ExecutingDelta;

            _world.Step(executingDelta);

            Debug.WriteLine(_circleBody.Position.Y);
        }

        public float Gravity { get; set; }
    }
}