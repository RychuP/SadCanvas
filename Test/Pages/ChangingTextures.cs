using Test.Screen;

namespace Test.Pages;

internal class ChangingTextures : Page
{
    public ChangingTextures() : base("Texture Utilities", 
        "Canvas includes static methods to load and create textures.")
    {
        var a = HorizontalAlignment.Center;

        // starting with a small, empty texture and changing it to a larger one
        var c = new Canvas(100, 50, Color.LightBlue.ToMonoColor())
        {
            Parent = this,
            Position = (50, 15)
        };
        
        // create a new, empty texture, fill it with color and replace the old one
        int width = 25, height = 7;
        c.Texture = Canvas.CreateTexture(width * c.FontSize.X + 6, height * c.FontSize.Y + 7);
        c.DefaultBackground = Color.Red.ToMonoColor();
        c.Clear();

        // info about sizes
        var s = new InfoSurface(c, (0, 0), Color.Yellow, c.CellWidth, c.CellHeight);
        s.Print(1, "ScreenSurface (Yellow)");
        s.Print(2, $"Size: {s.Surface.Width} x {s.Surface.Height} cells");
        s.Print(4, "Canvas (Red)");
        s.Print(5, $"Size: {c.Width} x {c.Height} pixels");

        // canvas description
        _ = new Description(c, a, "Yellow is the child", "of red canvas and was",
            "created with the use", "of CellArea property.", "Cells do not fully", "cover this canvas.", "");

        // starting with a small, empty texture, loading a larger one and swapping it with the first
        c = new Canvas(100, 50)
        {
            Parent = this,
            Position = (4, 6)
        };

        // load new texture to replace the old one
        c.Texture = Canvas.LoadTexture("Res/Images/bird.jpg");

        // info about sizes
        s = new InfoSurface(c, (16, 1), Color.LightBlue, 20, 5);
        s.Print(1, $"This box: {s.Surface.Width} x {s.Surface.Height}");
        s.Print(3, $"Canvas: {c.Width} x {c.Height}");

        // canvas description
        _ = new Description(c, a, "This instance started off as", "an empty 100 x 50 canvas. Texture",
            "was replaced with an image file", "effectively resizing it.", "");
    }

    class InfoSurface : ScreenSurface
    {
        public InfoSurface(Canvas canvas, Point position, Color color, int w, int h) : base(w, h)
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