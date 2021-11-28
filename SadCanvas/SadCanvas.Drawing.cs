using SadCanvas.Shapes;
using Rectangle = SadCanvas.Shapes.Rectangle;

namespace SadCanvas;

// Pixel drawing methods.
public partial class Canvas : ScreenObject, IDisposable
{
    /// <summary>
    /// Fills the area with a <see cref="MonoColor"/>.
    /// </summary>
    /// <param name="color"><see cref="MonoColor"/> to fill the <see cref="Canvas"/> with.</param>
    public void Fill(MonoColor color)
    {
        Array.Fill(Buffer, color);
        IsDirty = true;
    }

    /// <summary>
    /// Clears the area with <see cref="DefaultBackground"/>.
    /// </summary>
    public void Clear() =>
        Fill(DefaultBackground);

    /// <summary>
    /// Changes the <see cref="MonoColor"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <param name="color"><see cref="MonoColor"/> of the pixel.</param>
    public void SetPixel(Point position, MonoColor color)
    {
        int index = position.ToIndex(Width);
        Buffer[index] = color;
        IsDirty = true;
    }

    /// <summary>
    /// Retrieves the <see cref="MonoColor"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <returns><see cref="MonoColor"/> of the pixel.</returns>
    public MonoColor GetPixel(Point position)
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
    public void Draw(Shape shape)
    {
        if (shape is Line line) DrawLine(line);
        else if (shape is Polygon polygon) DrawPolygon(polygon);
    }

    /// <summary>
    /// Draws a <see cref="Line"/>.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    public void DrawLine(Point start, Point end) => DrawLine(new Line(start, end));

    /// <summary>
    /// Draws a <see cref="Line"/> with provided <paramref name="color"/>.
    /// </summary>
    /// <param name="start">Start <see cref="Point"/> for the line.</param>
    /// <param name="end">End <see cref="Point"/> for the line.</param>
    /// <param name="color"><see cref="MonoColor"/> for the line.</param>
    public void DrawLine(Point start, Point end, MonoColor color) => DrawLine(new Line(start, end, color));

    /// <summary>
    /// Draws a <see cref="Line"/>.
    /// </summary>
    /// <param name="line">Line to draw.</param>
    public void DrawLine(Line line)
    {
        Algorithms.Line(line.Start.X, line.Start.Y, line.End.X, line.End.Y, processor);

        bool processor(int x, int y)
        {
            Point p = (x, y);
            if (IsValidPosition(p))
                SetPixel(p, line.LineColor);
            return false;
        }
    }

    /// <summary>
    /// Draws a <see cref="Polygon"/>.
    /// </summary>
    /// <param name="polygon">Polygon to draw.</param>
    public void DrawPolygon(Polygon polygon)
    {
        if (polygon.Vertices.Length <= 1) return;

        Point end, start = polygon.Vertices[0];
        Line line;

        // iterate through all vertices and draw lines between them
        for (int i = 1, length = polygon.Vertices.Length; i < length; i++)
        {
            end = polygon.Vertices[i];
            line = new Line(start, end, polygon.LineColor);
            DrawLine(line);
            start = end;
        }

        // draw the last line connecting first and last vertices
        end = polygon.Vertices[0];
        line = new Line(start, end, polygon.LineColor);
        DrawLine(line);
    }

    /// <summary>
    /// Draws a <see cref="Polygon"/> and fills it with Polygon.FillColor.
    /// </summary>
    /// <param name="polygon">Polygon to draw.</param>
    public void DrawPolygonFilled(Polygon polygon)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Draws an <see cref="Ellipse"/>.
    /// </summary>
    public void DrawEllipse(Ellipse ellipse) => 
        DrawPolygon(ellipse);

    /// <summary>
    /// Draws an <see cref="Ellipse"/>.
    /// </summary>
    public void DrawEllipse(Point start, int radiusX, int radiusY, int noOfSides) =>
        DrawPolygon(new Ellipse(start, radiusX, radiusY, noOfSides));

    /// <summary>
    /// Draws an <see cref="Ellipse"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawEllipse(Point start, int radiusX, int radiusY, int noOfSides, MonoColor lineColor) =>
        DrawPolygon(new Ellipse(start, radiusX, radiusY, noOfSides, lineColor));

    /// <summary>
    /// Draws a <see cref="Circle"/>.
    /// </summary>
    public void DrawCircle(Circle circle) => 
        DrawPolygon(circle);

    /// <summary>
    /// Draws a <see cref="Circle"/>.
    /// </summary>
    public void DrawCircle(Point center, int radius, int noOfSides) =>
        DrawPolygon(new Circle(center, radius, noOfSides));

    /// <summary>
    /// Draws a <see cref="Circle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawCircle(Point center, int radius, int noOfSides, MonoColor lineColor) =>
        DrawPolygon(new Circle(center, radius, noOfSides, lineColor));

    /// <summary>
    /// Draws a <see cref="Rectangle"/>.
    /// </summary>
    public void DrawRectangle(Rectangle rectangle) =>
        DrawPolygon(rectangle);

    /// <summary>
    /// Draws a <see cref="SadRogue.Primitives.Rectangle"/>.
    /// </summary>
    public void DrawRectangle(SadRogue.Primitives.Rectangle rectangle) =>
        DrawPolygon(new Rectangle(rectangle.Position, rectangle.Width, rectangle.Height));

    /// <summary>
    /// Draws a <see cref="SadRogue.Primitives.Rectangle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawRectangle(SadRogue.Primitives.Rectangle rectangle, MonoColor lineColor) =>
        DrawPolygon(new Rectangle(rectangle.Position, rectangle.Width, rectangle.Height, lineColor));

    /// <summary>
    /// Draws a <see cref="Rectangle"/>.
    /// </summary>
    public void DrawRectangle(Point position, int width, int height) =>
        DrawPolygon(new Rectangle(position, width, height));

    /// <summary>
    /// Draws a <see cref="Rectangle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawRectangle(Point position, int width, int height, MonoColor lineColor) =>
        DrawPolygon(new Rectangle(position, width, height, lineColor));

    /// <summary>
    /// Draws a <see cref="Square"/>.
    /// </summary>
    public void DrawSquare(Square square) =>
        DrawPolygon(square);

    /// <summary>
    /// Draws a <see cref="Square"/>.
    /// </summary>
    public void DrawSquare(Point position, int sideLength) =>
        DrawPolygon(new Square(position, sideLength));

    /// <summary>
    /// Draws a <see cref="Square"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawSquare(Point position, int sideLength, MonoColor lineColor) =>
        DrawPolygon(new Square(position, sideLength, lineColor));

    /// <summary>
    /// Draws a <see cref="Triangle"/>.
    /// </summary>
    public void DrawTriangle(Triangle triangle) =>
        DrawPolygon(triangle);

    /// <summary>
    /// Draws a <see cref="Triangle"/>.
    /// </summary>
    public void DrawTriangle(Point position, Point corner1, Point corner2) =>
        DrawPolygon(new Triangle(position, corner1, corner2));

    /// <summary>
    /// Draws a <see cref="Triangle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawTriangle(Point position, Point corner1, Point corner2, MonoColor lineColor) =>
        DrawPolygon(new Triangle(position, corner1, corner2, lineColor));
}
