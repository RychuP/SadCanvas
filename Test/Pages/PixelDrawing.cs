using Test.Screen;
using SadCanvas.Shapes;

namespace Test.Pages;

internal class PixelDrawing : Page
{
    public PixelDrawing() : base("Pixel Drawing", "Implemented are methods for drawing various, basic shapes.")
    {
        var board = new DrawingBoard(MonoColor.LightBlue)
        {
            Parent = this,
            Position = (2, 2),
        };

        var line = new Line((10, 10), (200, 100)) { OutlineColor = MonoColor.Black };
        board.DrawLine(line);
    }

    internal class DrawingBoard : Canvas
    {
        public DrawingBoard(MonoColor color) : base(300, 150, color)
        {

        }
    }
}