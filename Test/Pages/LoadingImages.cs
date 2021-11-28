using Test.Screen;
using SadCanvas.Shapes;

namespace Test.Pages;

internal class LoadingImages : Page
{
    public LoadingImages() : base("Image files", "Canvas instance can be created directly from various image formats.")
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
            Children.Add(new VerticalLines() { Position = (70, -24) });
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
        int currentColumn = 0;

        public VerticalLines() : base(500, 20, Color.LightBlue.ToMonoColor())
        {
            UsePixelPositioning = true;
        }

        public override void Update(TimeSpan delta)
        {
            MonoColor color = GetRandomColor();
            Point start = (currentColumn++, 0);
            Point end = (currentColumn, Height - 1);
            Draw(new Line(start, end, color));
            base.Update(delta);
        }
    }
}
