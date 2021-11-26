namespace Test.Pages;

internal class ChangingTextures : Page
{
    public ChangingTextures() : base("Texture Utilities", 
        "Canvas includes static methods to load and create textures.")
    {
        // starting with a small, empty texture and changing it to a larger one
        var c = new Canvas(100, 50, Color.LightBlue.ToMonoColor())
        {
            Parent = this,
            Position = (50, 2)
        };
        int width = 25, height = 7;
        c.Texture = Canvas.CreateTexture(width * c.FontSize.X + 6, height * c.FontSize.Y + 7);
        c.DefaultBackground = Color.Red.ToMonoColor();
        c.Clear();
        var s = new Description(c, (0, 0), Color.Yellow, c.CellArea.Width, c.CellArea.Height);
        s.Print(1, "ScreenSurface (Yellow)");
        s.Print(2, $"Size: {s.Surface.Width} x {s.Surface.Height}");
        s.Print(4, "Canvas (Red)");
        s.Print(5, $"Size: {c.Width} x {c.Height}");

        // starting with a small, empty texture, loading a larger one and swapping it with the first
        c = new Canvas(100, 50)
        {
            Parent = this,
            Position = (4, 8)
        };
        c.Texture = Canvas.LoadTexture("Res/Images/bird.jpg");
        s = new Description(c, (16, 1), Color.LightBlue, 20, 5);
        s.Print(1, $"Surface: {s.Surface.Width} x {s.Surface.Height}");
        s.Print(3, $"Canvas: {c.Width} x {c.Height}");
    }

    class Description : ScreenSurface
    {
        public Description(Canvas canvas, Point position, Color color, int w, int h) : base(w, h)
        {
            Parent = canvas;
            Position = position;
            Surface.DefaultBackground = color;
            Surface.DefaultForeground = Color.Black;
            Surface.Clear();
        }

        public void Print(int y, string t) => Surface.Print(1, y, t);
    }
}