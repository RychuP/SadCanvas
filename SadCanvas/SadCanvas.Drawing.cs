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
    public void SetPixel(Vector2 position, MonoColor color)
    {
        int index = Convert.ToInt32(position.Y * Width + position.X);
        if (index >= 0 && index < Size)
            Buffer[index] = color;
        IsDirty = true;
    }

    /// <summary>
    /// Retrieves the <see cref="MonoColor"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <returns><see cref="MonoColor"/> of the pixel.</returns>
    public MonoColor GetPixel(Vector2 position)
    {
        int index = Convert.ToInt32(position.Y * Width + position.X);

        // position is out of bounds
        if (index < 0 || index >= Size)
            return MonoColor.Transparent;

        return Buffer[index];
    }

    /// <summary>
    /// Changes the <see cref="MonoColor"/> of a pixel at the given position.
    /// </summary>
    /// <param name="position">Position of the pixel.</param>
    /// <param name="color"><see cref="MonoColor"/> of the pixel.</param>
    public void SetPixel(Point position, MonoColor color)
    {
        int index = position.ToIndex(Width);
        if (index >= 0 && index < Size)
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
    /// <param name="color"><see cref="MonoColor"/> for the line.</param>
    public void DrawLine(Point start, Point end, MonoColor? color = null) => DrawLine(new Line(start, end, color));

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

    /// <summary>
    /// Draws a <see cref="Polygon"/>.
    /// </summary>
    /// <param name="polygon">Polygon to draw.</param>
    /// <param name="drawFilled">Draw the polygon filled with <see cref="Polygon.FillColor"/>.</param>
    public void DrawPolygon(Polygon polygon, bool drawFilled = false)
    {
        if (drawFilled)
        {
            var bounds = polygon.Bounds;
            DrawingBoard db = new(bounds);

            // draw outlines
            foreach (var line in polygon.Edges)
                db.DrawLine(line);

            // fill with color
            db.Fill();

            // transfer data from the drawing board to the buffer
            for (int y = 0; y < db.Height; y++)
            {
                for (int x = 0; x < db.Width; x++)
                {
                    Point pointOnDrawingBoard = (x, y);
                    Point pointOnCanvas = bounds.Position + pointOnDrawingBoard;
                    int indexBuffer = pointOnCanvas.ToIndex(Width);
                    if (IsValidPosition(pointOnCanvas))
                    {
                        var cell = db[pointOnDrawingBoard.ToIndex(db.Width)];
                        if (cell is DrawingBoard.Cell.Wall)
                            Buffer[indexBuffer] = polygon.Color;
                        else if (cell is DrawingBoard.Cell.Color)
                            Buffer[indexBuffer] = polygon.FillColor;
                    }
                }
            }

            IsDirty = true;
            db.Dispose();
        }

        // drawing only an outline
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
    /// Draws an <see cref="Ellipse"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawEllipse(Point start, int radiusX, int radiusY, MonoColor? lineColor, int? edgeCount = null) =>
        DrawPolygon(new Ellipse(start, radiusX, radiusY, lineColor, edgeCount));

    /// <summary>
    /// Draws a <see cref="Circle"/>.
    /// </summary>
    public void DrawCircle(Circle circle) => 
        DrawPolygon(circle);

    /// <summary>
    /// Draws a <see cref="Circle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawCircle(Point center, int radius, MonoColor? color = null, int? edgeCount = null) =>
        DrawPolygon(new Circle(center, radius, color, edgeCount));

    /// <summary>
    /// Draws a <see cref="Rectangle"/>.
    /// </summary>
    public void DrawRectangle(Rectangle rectangle) =>
        DrawPolygon(rectangle);

    /// <summary>
    /// Draws a <see cref="SadRogue.Primitives.Rectangle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawRectangle(SadRogue.Primitives.Rectangle rectangle, MonoColor? color = null) =>
        DrawPolygon(new Rectangle(rectangle.Position, rectangle.Width, rectangle.Height, color));

    /// <summary>
    /// Draws a <see cref="Rectangle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawRectangle(Point position, int width, int height, MonoColor? lineColor = null) =>
        DrawPolygon(new Rectangle(position, width, height, lineColor));

    /// <summary>
    /// Draws a <see cref="Square"/>.
    /// </summary>
    public void DrawSquare(Square square) =>
        DrawPolygon(square);

    /// <summary>
    /// Draws a <see cref="Square"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawSquare(Point position, int sideLength, MonoColor? lineColor = null) =>
        DrawPolygon(new Square(position, sideLength, lineColor));

    /// <summary>
    /// Draws a <see cref="Triangle"/>.
    /// </summary>
    public void DrawTriangle(Triangle triangle) =>
        DrawPolygon(triangle);

    /// <summary>
    /// Draws a <see cref="Triangle"/> with the given <see cref="MonoColor"/>.
    /// </summary>
    public void DrawTriangle(Point position, Point corner1, Point corner2, MonoColor? lineColor = null) =>
        DrawPolygon(new Triangle(position, corner1, corner2, lineColor));
}
