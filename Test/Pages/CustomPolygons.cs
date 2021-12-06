using Test.Screen;
using SadCanvas.Shapes;

namespace Test.Pages;

internal class CustomPolygons : Page
{
    public CustomPolygons() : base("Custom Polygons", "Adding vertices to existing, predefined polygons.")
    {
        Add(new DrawingBoard());
    }

    internal class DrawingBoard : Canvas
    {
        public DrawingBoard() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
        {
            // start with creating a circle
            Point center = (Width / 2, Height / 2);
            var circle = new Circle(center, 180, GetRandomColor(), 20)
            {
                FillColor = GetRandomColor()
            };

            // duplicate every other edge and shift it along its normal to create teeth
            List<Vector2> gearVertices = new();
            List<Vector2> teethNormals = new();
            var edges = circle.Edges;
            for (int i = 0; i < edges.Length; i++)
            {
                Line edge = edges[i];

                if (i % 2 == 0)
                {
                    Vector2 normal = edge.GetNormalVector();
                    gearVertices.Add(edge.Vertices[0] + normal * GearTooth.InitialLength);
                    gearVertices.Add(edge.Vertices[1] + normal * GearTooth.InitialLength);
                    teethNormals.Add(normal);
                }
                else
                {
                    gearVertices.AddRange(edge.Vertices);
                }
            }

            var poly = new Gear(gearVertices.ToArray(), teethNormals);
            DrawPolygon(poly, true);
        }
    }

    internal class Gear : Polygon
    {
        readonly GearTooth[] _teeth;

        public Gear(Vector2[] vertices, List<Vector2> teethNormals) : base(vertices, Canvas.GetRandomColor())
        {
            _teeth = new GearTooth[teethNormals.Count];
            FillColor = Canvas.GetRandomColor();

            for (int i = 0; i < teethNormals.Count; i++)
            {
                _teeth[i] = new GearTooth(teethNormals[i]);
            }
        }
    }

    internal class GearTooth
    {
        public const int InitialLength = 50;

        readonly Vector2 _normal;
        float _currentLength;
        float _targetLength;

        public GearTooth(Vector2 normal)
        {
            _normal = normal;
        }

        public Vector2 GetVector()
        {
            throw new NotImplementedException();
        }
    }
}