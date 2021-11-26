using Test.Screen;

namespace Test.Pages;

internal class ResizingCanvas : Page
{
    public ResizingCanvas() : base("Resize Methods", "Size can be changed by calling Resize method or swapping Texture.")
    {
        var a = HorizontalAlignment.Left;

        // resizing with a default start point of (0, 0)
        var c = new Parrot()
        {
            Parent = this,
            Position = (1, 2)
        };
        c.Resize(c.Width / 2, c.Height / 2);
        c.Children.Add(new Description(c, a, ".Resize(Width/2, Height/2);"));

        // resizing with a custom start point
        c = new Parrot()
        {
            Parent = this,
            Position = (37, 9)
        };
        var startPoint = (238, 20);
        c.Resize(c.Width / 2, c.Height / 2, startPoint);
        _ = new Description(c, a, $".Resize(Width/2, Height/2, {startPoint});");

        // resizing and filling empty space with background color
        c = new Parrot()
        {
            Parent = this,
            Position = (7, 18),
            DefaultBackground = MonoColor.Yellow
        };
        startPoint = (344, 271);
        c.Resize(c.Width / 2, c.Height / 2, startPoint);
        _ = new Description(c, a, $".Resize(Width/2, Height/2, {startPoint});");
    }

    class Parrot : Canvas
    {
        public Parrot() : base("Res/Images/parrot.jpg")
        {

        }
    }
}