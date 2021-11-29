namespace SadCanvas.Shapes;

/// <summary>
/// A primitive triangle that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Triangle : Polygon
{
    /// <summary>
    /// Creates and instance of <see cref="Triangle"/> with the given parameters.
    /// </summary>
    /// <param name="corner1">One of the corners..</param>
    /// <param name="corner2">Another one of the corners.</param>
    /// <param name="corner3">Another one of the corners.</param>
    public Triangle(Point corner1, Point corner2, Point corner3) :
        this(corner1, corner2, corner3, DefaultColor) 
    { }

    /// <summary>
    /// Creates and instance of <see cref="Polygon"/> with the given parameters.
    /// </summary>
    /// <param name="corner1">One of the corners..</param>
    /// <param name="corner2">Another one of the corners.</param>
    /// <param name="corner3">Another one of the corners.</param>
    /// <param name="color">Color of the edges.</param>
    public Triangle(Point corner1, Point corner2, Point corner3, MonoColor color) : 
        base(new Point[] { corner1, corner2, corner3 }, color) 
    { }

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
            var pos1 = area.GetRandomPosition();
            var pos2 = area.GetRandomPosition();
            var pos3 = area.GetRandomPosition();

            double side1 = Line.GetDistance(pos1, pos2);
            double side2 = Line.GetDistance(pos2, pos3);
            double side3 = Line.GetDistance(pos3, pos1);

            if (side1 >= minLineLength && side2 >= minLineLength && side3 >= minLineLength &&
                side1 <= maxLineLength && side2 <= maxLineLength && side3 <= maxLineLength)
                return new Triangle(pos1, pos2, pos3, Canvas.GetRandomColor());
        }
    }
}