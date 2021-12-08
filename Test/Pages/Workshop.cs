using Test.Screen;
using SadCanvas.Shapes;
using SadCanvas;

namespace Test.Pages
{
    internal class Workshop : Page
    {
        public Workshop() : base("Workshop", "Transforming shapes and an example of using a polygon creator.")
        {
            Add(new DrawingBoard());
        }

        internal class DrawingBoard : Canvas
        {
            public DrawingBoard() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
            {
                // tests of line drawing, offsetting and rotating
                var line = new Line((10, 10), (300, 170), GetRandomColor());
                DrawLine(line);
                SetPixel(line.Center, MonoColor.Red);

                line.Offset(new Vector2(150, 70));
                line.Color = GetRandomColor();
                Draw(line);

                line.Rotate(1.5f);
                line.Color = GetRandomColor();
                Draw(line);

                line.Scale(0.5f);
                line.Offset(50, 0);
                Draw(line);

                // small, filled square
                var square = new Square((10, 60), 70, true);
                square.Offset(new Vector2(10, 30));
                Draw(square, true);

                // move and rotate the square -> draw with no fill color
                square.Color = GetRandomColor();
                square.Rotate(0.8f);
                square.Offset(130, 70);
                Draw(square);

                // a tank sort of thing
                var creator = Polygon.Create(210, 450).
                    GoHorizontal(400);

                for (var i = 0; i < 5; i++)
                {
                    creator.
                        SetArcCenter().
                        GoVertical(-50).
                        MakeArc(-1f, 10).
                        GoHorizontal(-20);
                }

                var polygon = creator.
                    GoHorizontal(-200).
                    TurnLeft(35).
                    TurnBy(1.6f, 80).
                    TurnLeft(40).
                    TurnLeft(60).
                    TurnBy(1f, 50).
                    GetPolygon(true);

                Draw(polygon, true);

                // a rough circle -> could be easily replaced with just a new Circle()...
                float angle = (float) Math.Tau / 11;
                creator.Start(160, 260).
                    SetArcCenter().
                    MoveArcCenterBy(70, -70);

                for (int i = 0; i < 11; i++)
                    creator.
                        MakeArc(angle, 1);

                // get the polygon from creator and draw
                polygon = creator.GetPolygon(true);
                Draw(polygon);

                // single pixel for debugging
                SetPixel(polygon.Edges[0].Start, MonoColor.Red);

                // move the polygon and draw it filled
                polygon.Offset(new Vector2(300, -50));
                Draw(polygon, true);
            }
        }
    }
}