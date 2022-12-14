using Test.Screen;
using SadCanvas.Shapes;
using SadConsole.EasingFunctions;
using Circle = SadCanvas.Shapes.Circle;
namespace Test.Pages;

internal class CustomPolygons : Page
{
    public CustomPolygons() : base("Custom Polygons", "Adding and modyfing vertices of existing, predefined polygons.")
    {
        Add(new DrawingBoard());
    }

    internal class DrawingBoard : Canvas
    {
        readonly Gear _gear;
        
        public DrawingBoard() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
        {
            // start with creating a circle
            Point center = (Width / 2, Height / 2);
            var circle = new Circle(center, 180, null, null, 20);

            // duplicate every other edge and shift it along its normal to create gear teeth
            List<Vector2> gearVertices = new();
            var edges = circle.Edges;
            for (int i = 0; i < edges.Length; i++)
            {
                Line edge = edges[i];

                if (i % 2 == 0)
                {
                    Vector2 normal = edge.GetNormalVector();
                    gearVertices.Add(edge.Vertices[0] + normal * GearTooth.InitialLength);
                    gearVertices.Add(edge.Vertices[1] + normal * GearTooth.InitialLength);
                }
                else
                {
                    gearVertices.AddRange(edge.Vertices);
                }
            }
            _gear = new Gear(gearVertices.ToArray(), center);

            // B
            var s = new ScreenSurface(1, 1)
            {
                Parent = this,
                FontSize = (128, 256),
                UsePixelPositioning = true,
                
            };
            s.Position = ((Settings.Rendering.RenderWidth - s.AbsoluteArea.Width) / 2,
                (Settings.Rendering.RenderHeight - s.AbsoluteArea.Height) / 2);
            s.Surface.Print(0, 0, "B");

        }

        public override void Update(TimeSpan delta)
        {
            Clear();
            _gear.Update(this, delta);
            base.Update(delta);
        }
    }

    internal class Gear : Polygon
    {
        readonly GearTooth[] _teeth;
        readonly float _angle = (float)Math.Tau / 500;

        readonly Polygon _innerPoly;
        TimeSpan _time = TimeSpan.Zero;

        const float MaxScale = 1.2f,
            MinScale = 1f,
            FastScaleUp = 1.005f,
            SlowScaleUp = 1.002f,
            FastScaleDown = 0.994f,
            SlowScaleDown = 0.998f;

        float _currentScale = MinScale;
        float _scaleDelta = SlowScaleUp;

        public Gear(Vector2[] vertices, Point center) : base(vertices)
        {
            _teeth = new GearTooth[vertices.Length / 4];
            for (int i = 0; i < _teeth.Length; i++)
                _teeth[i] = new GearTooth();
            _innerPoly = new Circle(center, 130, Color, Color.Black, 10);
        }

        public void Update(Canvas c, TimeSpan deltaTime)
        {
            Rotate(_angle);
            _innerPoly.Rotate(-_angle);

            _currentScale *= _scaleDelta;
            Scale(_scaleDelta);
            _innerPoly.Scale(_scaleDelta);

            _scaleDelta = _currentScale > MaxScale ? Program.GetRandomInt(1) switch { 0 => SlowScaleDown, _ => FastScaleDown } :
                          _currentScale < MinScale ? Program.GetRandomInt(1) switch { 0 => SlowScaleUp, _ => FastScaleUp } :
                          _scaleDelta;
            
            for (int i = 0, t = 1; i < _teeth.Length; i++, t += 4)
            {
                var tooth = _teeth[i];

                Vector2 firstBasePoint = Vertices[t];
                int secBasePointIndex = t + 3;
                Vector2 secondBasePoint = Vertices[secBasePointIndex < Vertices.Length ? secBasePointIndex : 0];

                // get the base of the tooth
                Line toothBaseLine = new(firstBasePoint, secondBasePoint);
                Vector2 toothNormal = toothBaseLine.GetNormalVector();

                // move the corners of the tooth
                float currentToothLength = tooth.GetCurrentLength(deltaTime);
                Vertices[t + 1] = firstBasePoint + toothNormal * currentToothLength;
                Vertices[t + 2] = secondBasePoint + toothNormal * currentToothLength;
            }

            c.Draw(this);
            c.Draw(_innerPoly);
        }
    }

    internal class GearTooth
    {
        static readonly Quad _quad = new() { Mode = EasingMode.Out };
        public const int InitialLength = 10;
        const int MaxPositiveLength = 100;
        const int MaxNegativeLength = -50;

        TimeSpan _elapsedTime = TimeSpan.Zero;
        TimeSpan _targetTime = TimeSpan.Zero;
        
        double _targetLength;
        double _startLength = InitialLength;
        double _currentLength = InitialLength;

        public GearTooth()
        {
            _targetLength = Program.GetRandomInt(MaxNegativeLength, MaxPositiveLength);
            SetNewTargetLength();
        }

        void SetNewTargetLength()
        {
            _targetLength = _targetLength > 0 ?
                Program.GetRandomInt(MaxNegativeLength, 0) - _currentLength :
                Program.GetRandomInt(0, MaxPositiveLength) - _currentLength;
            _targetTime = TimeSpan.FromSeconds(Program.GetRandomInt(1, 2));
            _startLength = _currentLength;
        }

        public float GetCurrentLength(TimeSpan deltaTime)
        {
            _elapsedTime += deltaTime;
            if (_elapsedTime > _targetTime)
            {
                SetNewTargetLength();
                _elapsedTime = TimeSpan.Zero;
            }
            _currentLength = _quad.Ease(_elapsedTime.TotalMilliseconds, _startLength, _targetLength, _targetTime.TotalMilliseconds);
            return (float)_currentLength;
        }
    }
}