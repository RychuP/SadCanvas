namespace SadCanvas.Shapes;

/// <summary>
/// A primitive triangular <see cref="Shape"/>.
/// </summary>
public class Triangle : Polygon
{
    /// <summary>
    /// Creates and instance of <see cref="Triangle"/> with the given parameters.
    /// </summary>
    /// <param name="corner1">First of the corners.</param>
    /// <param name="corner2">Another corner.</param>
    /// <param name="corner3">Another corner.</param>
    /// <param name="color">Color of the edges.</param>
    /// <param name="fillColor">Color of the interior area.</param>
    public Triangle(Point corner1, Point corner2, Point corner3, MonoColor? color = null, MonoColor? fillColor = null) : 
        base(new Vector2[] { corner1.ToVector(), corner2.ToVector(), corner3.ToVector() }, color, fillColor) 
    { }

    /// <summary>
    /// Creates and instance of <see cref="Triangle"/> with the given parameters.
    /// </summary>
    /// <param name="corner1">First of the corners.</param>
    /// <param name="corner2">Another corner.</param>
    /// <param name="corner3">Another corner.</param>
    /// <param name="randomColors">Colors will be random or default.</param>
    public Triangle(Point corner1, Point corner2, Point corner3, bool randomColors = false) :
        base(new Vector2[] { corner1.ToVector(), corner2.ToVector(), corner3.ToVector() }, randomColors)
    { }

    /// <summary>
    /// Generates a <see cref="Triangle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Triangle"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    /// <param name="mode">Mode of generating an instance.</param>
    /// <remarks>Colors are random by default.</remarks>
    public Triangle(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength, Mode mode = Mode.Random) :
        base(GetRandomTriangle(area, minLineLength, maxLineLength, mode), true)
    { }

    /// <inheritdoc/>
    public override Triangle Clone(Transform? transform = null)
    {
        var triangle = new Triangle(Vertices[0].ToSadPoint(), Vertices[1].ToSadPoint(), Vertices[2].ToSadPoint(), Color, FillColor);
        if (transform is Transform t)
            triangle.Apply(t);
        return triangle;
    }

    static Vector2[] GetRandomTriangle(SadRogue.Primitives.Rectangle area, int minLineLength, int maxLineLength, Mode mode)
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
                return new Vector2[] { pos1.ToVector(), pos2.ToVector(), pos3.ToVector() };
        }
    }
}