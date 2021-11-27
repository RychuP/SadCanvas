namespace SadCanvas.Shapes;

/// <summary>
/// A primitive circle that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Circle : Ellipse
{
    /// <summary>
    /// Radius of the <see cref="Circle"/>.
    /// </summary>
    public int Radius { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Circle"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center <see cref="Point"/> of the <see cref="Circle"/>.</param>
    /// <param name="radius">Radius of the <see cref="Circle"/>.</param>
    /// <param name="noOfSides">Number of sides of the <see cref="Circle"/> (more means smoother edges).</param>
    public Circle(Point center, int radius, int noOfSides) : base(center, radius, radius, noOfSides)
    {
        Radius = radius;
    }

    /// <summary>
    /// Creates an instance of <see cref="Circle"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center <see cref="Point"/> of the <see cref="Circle"/>.</param>
    /// <param name="radius">Radius of the <see cref="Circle"/>.</param>
    /// <param name="noOfSides">Number of sides of the <see cref="Circle"/> (more means smoother edges).</param>
    /// <param name="lineColor"><see cref="MonoColor"/> of the outline.</param>
    public Circle(Point center, int radius, int noOfSides, MonoColor lineColor) : base(center, radius, radius, noOfSides)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Circle"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Circle"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusLength">Maximum radius length.</param>
    public Circle(Canvas canvas, int minRadiusLength = MinLength, int maxRadiusLength = MaxLength) : 
        base(canvas, minRadiusLength, maxRadiusLength)
    {
        Radius = RadiusX;
    }
}