namespace SadCanvas.Shapes;

/// <summary>
/// A primitive triangular <see cref="Shape"/>.
/// </summary>
public record Triangle : Polygon
{
    /// <summary>
    /// Creates and instance of <see cref="Triangle"/> with the given parameters.
    /// </summary>
    /// <param name="corner1">First of the corners.</param>
    /// <param name="corner2">Another corner.</param>
    /// <param name="corner3">Another corner.</param>
    /// <param name="color">Color of the edges.</param>
    public Triangle(Point corner1, Point corner2, Point corner3, MonoColor? color = null) : 
        base(new Point[] { corner1, corner2, corner3 }, color) 
    { }

    /// <summary>
    /// Generates a <see cref="Triangle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Triangle"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    /// <param name="mode">Mode of generating an instance.</param>
    /// <param name="color">Color of the triangle.</param>
    public Triangle(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength,
        Mode mode = Mode.Random, MonoColor? color = null) :
        base(GenerateTriangle(area, minLineLength, maxLineLength, mode),
            color is null ? Canvas.GetRandomColor() : color.Value)
    {
        FillColor = Canvas.GetRandomColor();
    }

    static Point[] GenerateTriangle(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength, Mode mode)
    {
        if (mode == Mode.Fit) throw new NotImplementedException();
        if (minLineLength <= 0 || maxLineLength <= 0) throw new ArgumentException("Triangle constraints cannot be 0 or negative.");
        if (maxLineLength < minLineLength) throw new ArgumentException("Max length cannot be smaller than min length.");
        if (area.Width < minLineLength || area.Height < minLineLength) throw new ArgumentException("Area width cannot be smaller than min length.");

        int minLineLengthSquared = minLineLength * minLineLength;
        int maxLineLengthSquared = maxLineLength * maxLineLength;

        while (true)
        {
            var pos1 = area.GetRandomPosition();
            var pos2 = area.GetRandomPosition();
            var pos3 = area.GetRandomPosition();

            double side1Squared = Point.EuclideanDistanceMagnitude(pos1, pos2);
            double side2Squared = Point.EuclideanDistanceMagnitude(pos2, pos3);
            double side3Squared = Point.EuclideanDistanceMagnitude(pos3, pos1);

            if (side1Squared >= minLineLengthSquared && side2Squared >= minLineLengthSquared && side3Squared >= minLineLengthSquared &&
                side1Squared <= maxLineLengthSquared && side2Squared <= maxLineLengthSquared && side3Squared <= maxLineLengthSquared)
                return new Point[] { pos1, pos2, pos3 };
        }
    }
}