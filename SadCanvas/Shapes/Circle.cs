namespace SadCanvas.Shapes;

/// <summary>
/// A primitive circular <see cref="Shape"/>.
/// </summary>
public class Circle : Ellipse
{
    /// <summary>
    /// Radius length.
    /// </summary>
    public int Radius => RadiusX;

    /// <summary>
    /// Creates an instance of <see cref="Circle"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radius">Length of radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    /// <param name="color">Color of edges.</param>
    /// <param name="fillColor">Color of the interior area.</param>
    public Circle(Point center, int radius, MonoColor? color = null, MonoColor? fillColor = null, int? edgeCount = null) :
        base(center, radius, radius, color, fillColor, edgeCount)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Circle"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radius">Length of radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    /// <param name="randomColors">Colors will be random or default.</param>
    public Circle(Point center, int radius, bool randomColors = false, int? edgeCount = null) :
        base(center, radius, radius, randomColors, edgeCount)
    { }

    /// <summary>
    /// Generates a random <see cref="Circle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Circle"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusLength">Maximum radius length.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    /// <remarks>Colors are random by default.</remarks>
    public Circle(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusLength, Mode mode = Mode.Random) :
        base(area, minRadiusLength, maxRadiusLength, maxRadiusLength, mode, true)
    { }

    /// <inheritdoc/>
    public override Circle Clone(Transform? transform = null)
    {
        var circle = new Circle(Center.ToSadPoint(), Radius, Color, FillColor, Vertices.Length);
        if (transform is Transform t)
            circle.Apply(t);
        return circle;
    }
}