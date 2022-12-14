using Test.Screen;
using SadCanvas.Shapes;
namespace Test.Pages;

internal class RotatingShapes : Page
{
    public RotatingShapes() : base("Rotating Shapes", "Simple animation achieved by applying rotation to primitive shapes.")
    {
        Add(new Animation());
    }

    internal class Animation : Canvas
    {
        readonly Polygon _p, _p2, _p3, _p4;
        readonly float _angle = (float)Math.Tau / 200;

        public Animation() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
        {
            Point center = (Width / 2, Height / 2);
            _p4 = new SadCanvas.Shapes.Rectangle(center - (80, 200), 80, 150, Program.GetRandomColor(), Color.BlanchedAlmond);
            _p = new Square(center - (200, 100), 200, Program.GetRandomColor(), Color.LightBlue);
            _p2 = new Circle(center + (100, 0), 130, Program.GetRandomColor(), Color.LightCoral, 10);
            center += (3, 100);
            _p3 = new Triangle(center - (120, 100),
                                center + (60, 70),
                                center - (60, -70), Program.GetRandomColor(), Color.LightSeaGreen);
        }

        public override void Update(TimeSpan delta)
        {
            _p.Rotate(_angle);
            _p2.Rotate(-_angle);
            _p3.Rotate(-_angle);
            _p4.Rotate(-_angle);
            base.Update(delta);
        }

        public override void Render(TimeSpan delta)
        {
            Clear();
            DrawPolygon(_p4, true);
            DrawPolygon(_p, true);
            DrawPolygon(_p2, true);
            DrawPolygon(_p3, true);
            base.Render(delta);
        }
    }
}