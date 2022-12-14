using Test.Screen;
using SadCanvas.Shapes;
using SadConsole.Effects;

namespace Test.Pages;

internal class PixelDrawing : Page
{
    public PixelDrawing() : base("Pixel Drawing", "Tests of various drawing methods.")
    {
        _ = new Noise()
        {
            Parent = this,
            Position = (33, 1),
        };

        _ = new Lines()
        {
            Parent = this,
            Position = (1, 6),
        };

        var c = new Canvas(350, 250, Color.DarkGray)
        {
            Parent = this,
            Position = (26, 13),
        };

        ColoredString t = new("  Polygons...  ", Color.Yellow, Color.Black.SetAlpha(125));
        Info i = new(t, c);

        // c content
        Triangle triangle = new((120, 150), (23, 235), (155, 230), Color.Salmon, Color.SkyBlue);
        c.Draw(triangle, true);

        Circle circle = new((210, 45), 35, Color.Yellow, Color.LightGoldenrodYellow);
        c.Draw(circle, true);

        Square square = new((267, 161), 65, Program.GetRandomColor(), Color.Chartreuse);
        c.Draw(square, true);

        Polygon polygon = new(new Point[] {(37, 166), (113, 50), (47, 78), (304, 109), (294, 17), (219, 233), (130, 108)},
            Color.White, Color.LightSkyBlue);
        c.Draw(polygon, true);

        var p = Point.FromIndex(24078, polygon.Bounds.Width);
        p = polygon.Bounds.Position + p;
        if (!c.Area.Contains(p)) throw new Exception();
        c.SetPixel(p, Color.Yellow);
    }

    class Info : ScreenSurface
    {
        public Info(string title, ScreenObject parent) : this(title.Length, parent)
        {
            Surface.DefaultBackground = Color.Black.SetAlpha(125);
            Surface.DefaultForeground = Color.White;
            Surface.Print(2, 0, title);
        }

        public Info(ColoredString title, ScreenObject parent) : this(title.Length, parent)
        {
            Surface.Print(0, 0, title);
        }

        Info(int length, ScreenObject parent) : base(length + 4, 1)
        {
            Parent = parent;
            Position = (1, 1);
        }
    }

    class Lines : Canvas
    {
        public Lines() : base(350, 250, Color.LightBlue)
        {
            _ = new Info("Some line pattern applied...", this);
            DrawPattern();
        }

        void DrawPattern()
        {
            int linesInPattern = 3;
            int noOfPatterns = 8;
            int qty = linesInPattern * noOfPatterns;
            int w = Width / qty;
            int horizontalOffset = 7;
            Point s = (horizontalOffset, 0);
            Color lineColor = Color.Blue;

            for (int x = 0; x <= qty; x++)
            {
                DrawLine(s, s + (0, Height), lineColor);
                s += (w, 0);
            }

            int delta = w * linesInPattern;
            for (int y = 0; y < 3; y++)
            {
                int verticalSpacer = 12 + y * 40 + y * delta;
                for (int p = 0; p < noOfPatterns / 2; p++)
                {
                    int horizontalSpacer = p * delta * 2;
                    for (int x = 0; x <= delta; x++)
                    {
                        int diagonalSpacer = (int)(verticalSpacer + delta + x * 0.4);
                        s = (horizontalSpacer + x + horizontalOffset, diagonalSpacer);
                        Point t = s + (delta, -delta);
                        Color c = x % w == 0 ? lineColor : DefaultBackground;
                        DrawLine(s, t, c);
                    }
                }
            }
        }
    }

    class Noise : Canvas
    {
        int i = 0;
        string title = "  Random Noise  ";
        int timer = 0;
        Color c = new(0, 125, 125);
        Info info;

        public Noise() : base(350, 250)
        {
            info = new Info(title, this);
            info.Surface.DefaultBackground = Color.Black;
        }

        public override void Update(TimeSpan delta)
        {
            if (timer++ > 2)
            {
                i = i == 255 ? 0 : i + 1;
                c = new(i, 125, 125);
                timer = 0;
                info.Surface.Print(0, 0, string.Format("{0} {1:000}", title[..^1], i), c);
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        SetPixel((x, y), Program.GetRandomColor());
                    }
                }
            }
            base.Update(delta);
        }
    }
}