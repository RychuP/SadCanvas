using Test.Screen;
using SadCanvas.Shapes;
namespace Test.Pages;

internal class Workshop : Page
{
    public Workshop() : base("Workshop", "Transforming shapes.")
    {
        Add(new DrawingBoard());
    }

    internal class DrawingBoard : Canvas
    {
        public DrawingBoard() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
        {
            // tests of line drawing, offsetting and rotating
            var line = new Line((10, 10), (300, 170), Program.GetRandomColor());
            DrawLine(line);

            line.Offset(new Vector2(150, 70));
            line.Color = Program.GetRandomColor();
            Draw(line);

            line.Rotate(1.5f);
            line.Color = Program.GetRandomColor();
            Draw(line);

            line.Scale(0.5f);
            line.Offset(50, 0);
            Draw(line);

            // small, filled square
            var square = new Square((10, 60), 70);
            square.Offset(new Vector2(10, 30));
            Draw(square, true);

            // move and rotate the square -> draw with no fill color
            square.Color = Program.GetRandomColor();
            square.Rotate(0.8f);
            square.Offset(130, 70);
            Draw(square);
        }
    }
}