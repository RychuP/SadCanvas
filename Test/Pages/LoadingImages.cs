namespace Test.Pages;

internal class LoadingImages : Page
{
    public LoadingImages() : base("Loading Images", "Shows use of canvas in loading images from files.")
    {
        var canvas = new Parrot();
        Add(canvas);
    }
}

internal class Parrot : PixelSurface
{
    public Parrot() : base("Res/Images/parrot.jpg")
    {
        Children.Add(new Mario());
        Children.Add(new VerticalLines() { Position = (70, 430) });
        Children.Add(new VerticalLines() { Position = (70, -24) });
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

            Surface.DrawBox(new Rectangle(0, 0, Surface.Width, Surface.Height),
                ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick,
                new ColoredGlyph(Color.Green, Color.Yellow))
            );

            Position = (4, 3);
        }
    }
}
internal class VerticalLines : Canvas
{
    int currentColumn = 0;

    public VerticalLines() : base(500, 20, Color.LightBlue)
    {
        UsePixelPositioning = true;
    }

    public override void Update(TimeSpan delta)
    {
        var color = Program.RandomColor.ToMonoColor();

        currentColumn++;
        if (currentColumn >= Width) currentColumn = 0;

        for (int y = 0; y < Height; y++)
        {
            Point position = new(currentColumn, y);
            SetPixel(position, color);
        }

        base.Update(delta);
    }
}