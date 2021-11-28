namespace SadCanvas.Shapes;

/// <summary>
/// A primitive triangle that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Triangle : Polygon
{
    /// <summary>
    /// Creates and instance of <see cref="Triangle"/> with the given parameters.
    /// </summary>
    public Triangle(Point corner1, Point corner2, Point corner3) : base(new Point[] { corner1, corner2, corner3 }) { }

    /// <summary>
    /// Creates and instance of <see cref="Polygon"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Triangle"/>.</param>
    /// <param name="corner1">One of the corners of the <see cref="Triangle"/>.</param>
    /// <param name="corner2">Another one of the corners of the <see cref="Triangle"/>.</param>
    /// <param name="lineColor"><see cref="MonoColor"/> of the outline of the <see cref="Triangle"/>.</param>
    public Triangle(Point corner1, Point corner2, Point corner3, MonoColor lineColor) : this(corner1, corner2, corner3)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Triangle"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Triangle"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Triangle GetRandomTriangle(Canvas canvas, int minLineLength, int maxLineLength) =>
        GetRandomTriangle(canvas.Area, minLineLength, maxLineLength);

    /// <summary>
    /// Generates a random <see cref="Triangle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Triangle"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public static Triangle GetRandomTriangle(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength)
    {
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Triangle constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (area.Width < minLineLength || area.Height < minLineLength) throw new ArgumentException("Area width cannot be smaller than min length.");

        while (true)
        {
            var pos1 = Canvas.GetRandomPosition(area);
            var pos2 = Canvas.GetRandomPosition(area);
            var pos3 = Canvas.GetRandomPosition(area);

            double side1 = Line.GetDistance(pos1, pos2);
            double side2 = Line.GetDistance(pos2, pos3);
            double side3 = Line.GetDistance(pos3, pos1);

            if (side1 >= minLineLength && side2 >= minLineLength && side3 >= minLineLength &&
                side1 <= maxLineLength && side2 <= maxLineLength && side3 <= maxLineLength)
                return new Triangle(pos1, pos2, pos3, Canvas.GetRandomColor());
        }
    }
}