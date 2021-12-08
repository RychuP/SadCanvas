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
        readonly Gear _gear;

        public DrawingBoard() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
        {
            // start with creating a circle
            Point center = (Width / 2, Height / 2);
            var circle = new Circle(center, 180, true, 20);

            // duplicate every other edge and shift it along its normal to create gear teeth
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

            _gear = new Gear(gearVertices.ToArray(), teethNormals);
            DrawPolygon(_gear, true);
        }

        //public override void Update(TimeSpan delta)
        //{
        //    _gear.Update(delta);
        //    base.Update(delta);
        //}
    }

    internal class Gear : Polygon
    {
        readonly GearTooth[] _teeth;

        public Gear(Vector2[] vertices, List<Vector2> teethNormals) : base(vertices, true)
        {
            _teeth = new GearTooth[teethNormals.Count];
            for (int i = 0; i < teethNormals.Count; i++)
                _teeth[i] = new GearTooth(teethNormals[i]);
        }

        public void Update(TimeSpan delta)
        {
            // i - count of teeth, t - pointer to the current tooth first point in polygon vertices array
            for (int i = 0, t = 2; i < _teeth.Length; i++, t += 4)
            {
                var tooth = _teeth[i];
                //tooth.Update(delta);
                //Vertices[t] = 
            }
        }
    }

    internal class GearTooth
    {
        public const int InitialLength = 10;

        readonly Vector2 _normal;
        float _targetLength;

        public GearTooth(Vector2 normal)
        {
            _normal = normal;
        }

        public Vector2 GetVector()
        {
            throw new NotImplementedException();
        }

        public void ChangeLength(float targetLength)
        {

        }
    }
}