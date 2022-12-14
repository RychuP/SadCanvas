using SadCanvas.Shapes;
using Test.Screen;
namespace Test.Pages;

internal class ColoredTriangles : Page
{
    public ColoredTriangles() : base("Colored Triangles", "Procedurally generated triangles in columns with random colors.")
    {
        Add(new ProceduralTriangles());
    }

    class ProceduralTriangles : Canvas
    {
        public ProceduralTriangles() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
        {
            int colHeight = 15;
            int colWidth = 12;
            int w = Width / colWidth;
            int h = Height / colHeight;
            int w2 = w / 2;
            int h2 = h / 2;
            Point s = (0, 0);

            for (int x = 0; x <= colWidth; x++)
            {
                for (int y = 0; y < colHeight; y++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        s = (Side)i switch
                        {
                            Side.Top => s,
                            Side.Right => s + (w, 0),
                            Side.Bottom => s + (0, h),
                            _ => s - (w, 0)
                        };

                        Color c = Program.GetRandomColor();
                        Triangle t = (Side)i switch
                        {
                            Side.Top => new Triangle(s, s + (w, 0), s + (w2, h2), c, c),
                            Side.Right => new Triangle(s, s + (0, h), s + (-w2, h2), c, c),
                            Side.Bottom => new Triangle(s, s - (w, 0), s - (w2, h2), c, c),
                            _ => new Triangle(s, s - (0, h), s + (w2, -h2), c, c)
                        };

                        DrawPolygon(t, true);
                    }
                }
                s = (x * w, 0);
            }
        }

        enum Side { Top, Right, Bottom, Left }
    }
}
