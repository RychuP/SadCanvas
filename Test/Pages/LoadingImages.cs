using Test.Screen;
using SadCanvas.Shapes;
namespace Test.Pages;

internal class LoadingImages : Page
{
    public LoadingImages() : base("Loading Images", "Canvas instance can be created directly from various image formats.")
    {
        var canvas = new Parrot();
        Add(canvas);
    }

    class Parrot : Canvas
    {
        public Parrot() : base("Res/Images/parrot.jpg")
        {
            Children.Add(new Mario());
            Children.Add(new VerticalLines() { Position = (70, 430) });
            Children.Add(new VerticalLines(true) { Position = (70, -24) });
        }
    }

    class Mario : ScreenSurface
    {
        public Mario() : base(16, 9)
        {
            Canvas mario = new("Res/Images/mario.png")
            {
                UsePixelPositioning = true,
                Position = (16, 23)
            };
            Children.Add(mario);

            Surface.DrawBox(new SadRogue.Primitives.Rectangle(0, 0, Surface.Width, Surface.Height),
                ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick,
                new ColoredGlyph(Color.Green, Color.Yellow))
            );

            Position = (4, 3);
        }
    }

    class VerticalLines : Canvas
    {
        static readonly Color s_color = Color.LightBlue;
        readonly bool _reverse;
        Color _currentColor;
        int _currentColumn;
        int _delta;

        public VerticalLines(bool reverse = false) : base(500, 20, s_color)
        {
            UsePixelPositioning = true;
            _delta = reverse ? -1 : 1;
            _currentColumn = reverse ? Width - 1 : 0;
            _reverse = reverse;
        }

        public override void Update(TimeSpan delta)
        {
            if (_delta == 1)
            {
                if (_currentColumn == Width - 1)
                {
                    _delta = -1;
                    _currentColor = (_reverse) ? Program.GetRandomColor() : s_color;
                }
                else
                {
                    _currentColor = (_reverse) ? s_color : Program.GetRandomColor();
                }
            }
            else
            {
                if (_currentColumn == 0)
                {
                    _delta = 1;
                    _currentColor = (_reverse) ? s_color : Program.GetRandomColor();
                }
                else
                {
                    _currentColor = (_reverse) ? Program.GetRandomColor() : s_color;
                }
            }

            Point start = (_currentColumn, 0);
            Point end = (_currentColumn, Height - 1);
            DrawLine(start, end, _currentColor);
            _currentColumn += _delta;
            base.Update(delta);
        }
    }
}
