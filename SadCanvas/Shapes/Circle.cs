namespace SadCanvas.Shapes;

/// <summary>
/// A primitive circular <see cref="Shape"/>.
/// </summary>
public record Circle : Ellipse
{
    /// <summary>
    /// Radius length.
    /// </summary>
    public int Radius => RadiusX;

    /// <summary>
    /// Creates an instance with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radius">Length of radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    public Circle(Point center, int radius, int edgeCount) : 
        this(center, radius, edgeCount, DefaultColor)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Circle"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radius">Length of radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    /// <param name="color">Color of edges.</param>
    public Circle(Point center, int radius, int edgeCount, MonoColor color) :
        base(center, radius, radius, edgeCount, color)
    { }

    /// <summary>
    /// Generates a random <see cref="Circle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Circle"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusLength">Maximum radius length.</param>
    public static Circle GetRandomCircle(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusLength)
    {
        if (GetRandomEllipse(area, minRadiusLength, maxRadiusLength, maxRadiusLength) is Circle circle)
            return circle;
        else
            throw new Exception("Internal error during a random circle generation.");
    }
        
}