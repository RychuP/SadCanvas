using Microsoft.Xna.Framework;
using SadCanvas.Shapes;
using System;
using Rectangle = SadCanvas.Shapes.Rectangle;

namespace SadCanvas;

// Pixel drawing methods.
public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Fills the area with a <see cref="Color"/>.
    /// </summary>
    /// <param name="color"><see cref="Color"/> to fill the <see cref="Canvas"/> with.</param>
    public void Fill(Color color)
    {
        Array.Fill(Buffer, color.ToMonoColor());
        IsDirty = true;
    }

    /// <summary>
    /// Clears the area with <see cref="DefaultBackground"/>.
    /// </summary>
    public void Clear() => Fill(DefaultBackground);

    /// <summary>
    /// Changes the <see cref="Color"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <param name="color"><see cref="Color"/> of the pixel.</param>
    public void SetPixel(Point position, Color color) => SetMonoPixel(position, color.ToMonoColor());

    /// <summary>
    /// Changes the <see cref="Color"/> of a pixel at the given position.
    /// </summary>
    /// <param name="x">Horizontal coordinate of the pixel.</param>
    /// <param name="y">Vertical coordinate of the pixel.</param>
    /// <param name="color"><see cref="Color"/> of the pixel.</param>
    public void SetPixel(int x, int y, Color color) => SetMonoPixel((x, y), color.ToMonoColor());

    private void SetMonoPixel(Point position, MonoColor color)
    {
        int index = position.ToIndex(Width);
        if (index >= 0 && index < Size)
        {
            Buffer[index] = color;
            IsDirty = true;
        }
    }

    /// <summary>
    /// Retrieves the <see cref="Color"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <returns><see cref="Color"/> of the pixel.</returns>
    public Color GetPixel(Point position) => GetMonoPixel(position).ToSadRogueColor();

    /// <summary>
    /// Retrieves the <see cref="Color"/> of a pixel at the given position.
    /// </summary>
    /// <param name="x">Horizontal coordinate of the pixel.</param>
    /// <param name="y">Vertical coordinate of the pixel.</param>
    /// <returns><see cref="Color"/> of the pixel.</returns>
    public Color GetPixel(int x, int y) => GetMonoPixel((x, y)).ToSadRogueColor();

    private MonoColor GetMonoPixel(Point position)
    {
        int index = position.ToIndex(Width);

        // position is out of bounds
        if (index < 0 || index >= Size)
            return MonoColor.Transparent;

        return Buffer[index];
    }

    /// <summary>
    /// Draws various objects of type <see cref="Shape"/>.
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="drawFilled">Draw the polygon filled with <see cref="Polygon.FillColor"/>.</param>
    public void Draw(Shape shape, bool drawFilled = false)
    {
        if (Area.Intersects(shape.Bounds))
        {
            if (shape is Line line) DrawLine(line);
            else if (shape is Polygon polygon) DrawPolygon(polygon, drawFilled);
        }
    }

    /// <summary>
    /// Draws a <see cref="Line"/> with provided <paramref name="color"/>.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    /// <param name="color"><see cref="Color"/> for the line.</param>
    public void DrawLine(Point start, Point end, Color? color = null) => DrawLine(new Line(start, end, color));

    /// <summary>
    /// Draws a <see cref="Line"/>.
    /// </summary>
    /// <param name="line">Line to draw.</param>
    public void DrawLine(Line line)
    {
        if (line.Start == line.End)
            SetPixel(line.Start, line.Color);
        else
        {
            Algorithms.Line(line.Start.X, line.Start.Y, line.End.X, line.End.Y, processor);

            bool processor(int x, int y)
            {
                Point p = (x, y);
                if (IsValidPosition(p))
                    SetPixel(p, line.Color);
                return false;
            }
        }
    }

    FloodFiller filler;

    /// <summary>
    /// Draws a <see cref="Polygon"/>.
    /// </summary>
    /// <param name="polygon">Polygon to draw.</param>
    /// <param name="drawFilled">Draw the polygon filled with <see cref="Polygon.FillColor"/>.</param>
    public void DrawPolygon(Polygon polygon, bool drawFilled = false)
    {
        // draw an outline and fill it with color
        if (drawFilled)
        {
            int w = polygon.Bounds.Width;
            int h = polygon.Bounds.Height;
            Point origin = polygon.Bounds.Position;

            if (filler is null)
                filler = new(polygon);
            else
                filler.Draw(polygon);

            MonoColor edgeColor = polygon.Color.ToMonoColor();
            MonoColor fillColor = polygon.FillColor.ToMonoColor();
            MonoColor defColor = DefaultBackground.ToMonoColor();

            // fill with color
            //FillBoard.Fill(polygon.Center.ToSadPoint());

            // transfer data from the fill board to the buffer
            MonoColor c;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    c = filler[x, y] switch
                    {
                        Cell.Wall => edgeColor,
                        Cell.Color => fillColor,
                        _ => defColor
                    };
                    if (c == edgeColor || c == fillColor)
                        SetMonoPixel(origin + (x, y), c);
                }
            }

            IsDirty = true;
        }

        // draw an outline only
        else
        {
            foreach (var line in polygon.Edges)
                DrawLine(line);
        }
    }

    /// <summary>
    /// Draws an <see cref="Ellipse"/>.
    /// </summary>
    public void DrawEllipse(Ellipse ellipse) => 
        DrawPolygon(ellipse);

    /// <summary>
    /// Draws an <see cref="Ellipse"/> with the given <see cref="Color"/>.
    /// </summary>
    public void DrawEllipse(Point start, int radiusX, int radiusY, Color? color = null, 
        Color? fillColor = null, int? edgeCount = null) =>
        DrawPolygon(new Ellipse(start, radiusX, radiusY, color, fillColor, edgeCount));

    /// <summary>
    /// Draws a <see cref="Circle"/>.
    /// </summary>
    public void DrawCircle(Circle circle) => 
        DrawPolygon(circle);

    /// <summary>
    /// Draws a <see cref="Circle"/> with the given <see cref="Color"/>.
    /// </summary>
    public void DrawCircle(Point center, int radius, Color? color = null, 
        Color? fillColor = null, int? edgeCount = null) =>
        DrawPolygon(new Circle(center, radius, color, fillColor, edgeCount));

    /// <summary>
    /// Draws a <see cref="Rectangle"/>.
    /// </summary>
    public void DrawRectangle(Rectangle rectangle) =>
        DrawPolygon(rectangle);

    /// <summary>
    /// Draws a <see cref="SadRogue.Primitives.Rectangle"/> with the given <see cref="Color"/>.
    /// </summary>
    public void DrawRectangle(SadRogue.Primitives.Rectangle rectangle, Color? color = null, Color? fillColor = null) =>
        DrawPolygon(new Rectangle(rectangle.Position, rectangle.Width, rectangle.Height, color, fillColor));

    /// <summary>
    /// Draws a <see cref="Rectangle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawRectangle(Point position, int width, int height, Color? color = null, Color? fillColor = null) =>
        DrawPolygon(new Rectangle(position, width, height, color, fillColor));

    /// <summary>
    /// Draws a <see cref="Square"/>.
    /// </summary>
    public void DrawSquare(Square square) =>
        DrawPolygon(square);

    /// <summary>
    /// Draws a <see cref="Square"/> with the given <see cref="Color"/>.
    /// </summary>
    public void DrawSquare(Point position, int sideLength, Color? color = null, Color? fillColor = null) =>
        DrawPolygon(new Square(position, sideLength, color, fillColor));

    /// <summary>
    /// Draws a <see cref="Triangle"/>.
    /// </summary>
    public void DrawTriangle(Triangle triangle) =>
        DrawPolygon(triangle);

    /// <summary>
    /// Draws a <see cref="Triangle"/> with the given <see cref="Color"/>.
    /// </summary>
    public void DrawTriangle(Point position, Point corner1, Point corner2, Color? color = null, Color? fillColor = null) =>
        DrawPolygon(new Triangle(position, corner1, corner2, color, fillColor));
}
