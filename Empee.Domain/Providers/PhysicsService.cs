using System;
using System.ComponentModel.Composition;
using Empee.Domain.Contracts;
using Empee.Domain.Extensions;
using Empee.Domain.Infrastructure;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using SharpDX;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Empee.Domain.Providers
{
    [Export(typeof (IPhysicsService))]
    internal sealed class PhysicsService : IPhysicsService
    {
        private readonly IContext _context;

        private Body _circleBody;
        private Body _polygonBody;
        private Body _ground;
        private World _world;

        [ImportingConstructor]
        public PhysicsService(IContext context, IExecutionLoopService executionLoopService, IRenderService renderService)
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

            Vector2[] vectors = 
            {
                 new Vector2(2.5f,0) 
                ,new Vector2(1.5f,3.5f) 
                ,new Vector2(-1.5f,2.5f) 
                ,new Vector2(-2.5f,0)
                ,new Vector2(-1.5f,-1.0f)
                ,new Vector2(1.5f,-1.0f)
            };

            var vertices = new Vertices(vectors);

            _polygonBody = BodyFactory.CreatePolygon(_world, vertices, 1.0f, new Vector2(0, -30.0f));
            _polygonBody.BodyType = BodyType.Dynamic;

            // Create the ground fixture
            _ground = BodyFactory.CreateEdge(_world, new Vector2(-1000, 0), new Vector2(1000, 0));
            _ground.IsStatic = true;
            _ground.Restitution = 0.3f;
            _ground.Friction = 0.5f;
        }

        private int _lastSecond;


        private void ExecutionLoopServiceOnExecuting(object sender, ExecutingEventArgs executingEventArgs)
        {
            var executingDelta = (float) executingEventArgs.ExecutingDelta;

            _world.Step(executingDelta);

            var thisSecond = DateTime.Now.Second;
            var modSecond = thisSecond%3;
            thisSecond -= modSecond;

            if (thisSecond == _lastSecond)
                return;

            try
            {
                if (_lastSecond == 0)
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

                _circleBody.ApplyLinearImpulse(new Vector2(x > 0 ? -50f*mult : 50f*mult, 150f + 20*mult));
            }
            finally
            {
                _lastSecond = thisSecond;                
            }
        }

        private void RenderServiceOnRendering(object sender, RenderingEventArgs renderingEventArgs)
        {
#if !DEBUG
            if (!Visible)
                return;
#endif

            var drawingService = renderingEventArgs.DrawingService;

            SetTransforms(drawingService);

            foreach (var body in _world.BodyList)
                RenderBody(drawingService, body);
        }

        private void SetTransforms(IDrawingService drawingService)
        {
            var viewport = _context.RenderControl.ClientSize;
            var viewportWidth = (float) viewport.Width;
            var viewportHeight = (float) viewport.Height;

            // This should be in IContext??
            const float pixelsPerMeter = 16.0f;

            drawingService.TranslateXCoordinate = x => viewportWidth/2 + x*pixelsPerMeter;
            drawingService.TranslateYCoordinate = y => viewportHeight + y*pixelsPerMeter;

            drawingService.ScaleXMagnitude = x => x*pixelsPerMeter;
            drawingService.ScaleYMagnitude = y => y*pixelsPerMeter;
        }

        private static void RenderBody(IDrawingService drawingService, Body body)
        {
            var bodyTransform = GetBodyTransform(body);
            var bodyColor = GetBodyColor(body);

            foreach (var fixture in body.FixtureList)
            {
                var outlineDrawingService = GetFixtureOutlineDrawingService(drawingService, fixture, bodyTransform);

                if (outlineDrawingService != null)
                {
                    outlineDrawingService.OutlineColor = bodyColor;
                    outlineDrawingService.Outline();
                }
            }
        }

        private static Transform GetBodyTransform(Body body)
        {
            Transform bodyTransform;
            body.GetTransform(out bodyTransform);
            return bodyTransform;
        }

        private static readonly Color4 DefaultShapeColor = new Color4(0.9f, 0.7f, 0.7f, 1.0f);
        private static readonly Color4 InactiveShapeColor = new Color4(0.5f, 0.5f, 0.3f, 1.0f);
        private static readonly Color4 KinematicShapeColor = new Color4(0.5f, 0.5f, 0.9f, 1.0f);
        private static readonly Color4 SleepingShapeColor = new Color4(0.6f, 0.6f, 0.6f, 1.0f);
        private static readonly Color4 StaticShapeColor = new Color4(0.5f, 0.9f, 0.5f, 1.0f);

        private static Color4 GetBodyColor(Body body)
        {
            if (!body.Enabled)
                return InactiveShapeColor;

            var bodyType = body.BodyType;

            if (bodyType == BodyType.Static)
                return StaticShapeColor;

            if (bodyType == BodyType.Kinematic)
                return KinematicShapeColor;

            if (!body.Awake)
                return SleepingShapeColor;

            return DefaultShapeColor;
        }

        private static IOutlineDrawingService GetFixtureOutlineDrawingService(
            IDrawingService drawingService, Fixture fixture, Transform bodyTransform)
        {
            switch (fixture.ShapeType)
            {
                case ShapeType.Circle:
                {
                    var circleShape = (CircleShape) fixture.Shape;
                    var center = TransformVector(bodyTransform, circleShape.Position);
                    var radius = circleShape.Radius;

                    return drawingService.Circle()
                        .SetCircleCenter(center.X, center.Y)
                        .SetRadius(radius);
                }
                    
                case ShapeType.Edge:
                {
                    var edgeShape = (EdgeShape) fixture.Shape;
                    var vertex1 = TransformVector(bodyTransform, edgeShape.Vertex1);
                    var vertex2 = TransformVector(bodyTransform, edgeShape.Vertex2);

                    return drawingService.Line()
                        .SetFrom(vertex1.X, vertex1.Y)
                        .SetTo(vertex2.X, vertex2.Y);
                }
/*
                case ShapeType.Polygon:
                {
                    var polygonShape = (PolygonShape) fixture.Shape;
                    var vertices = polygonShape.Vertices;

                    var points = vertices
                        .Select(v => TransformVector(bodyTransform, v))
                        .Select(v => new PointF(v.X, v.Y));

                    drawingService.DrawPolygon(points);

                    break;
                }

                case ShapeType.Chain:
                {
                    var chainShape = (ChainShape) fixture.Shape;
                    var vertices = chainShape.Vertices;

                    for (var x = 0; x < vertices.Count - 1; ++x)
                    {
                        var vertex1 = MathUtils.Mul(ref bodyTransform, vertices[x]);
                        var vertex2 = MathUtils.Mul(ref bodyTransform, vertices[x + 1]);

                        drawingService.DrawLine(vertex1.X, vertex1.Y, vertex2.X, vertex2.Y);
                    }

                    break;
                }*/
            }

            return null;
        }

        private static Vector2 TransformVector(Transform transform, Vector2 vector)
        {
            return MathUtils.Mul(ref transform, vector);
        }
    }
}