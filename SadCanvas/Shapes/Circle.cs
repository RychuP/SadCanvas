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
    public Circle(Point center, int radius, MonoColor? color = null, int ? edgeCount = null) :
        base(center, radius, radius, color, edgeCount)
    { }

    /// <summary>
    /// Generates a random <see cref="Circle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Circle"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusLength">Maximum radius length.</param>
    /// <param name="color">Color of the circle.</param>
    /// <param name="mode">Mode for generating an instance.</param>
    public Circle(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusLength,
        Mode mode = Mode.Random, MonoColor? color = null) :
        base(area, minRadiusLength, maxRadiusLength, maxRadiusLength, mode, color, true)
    {
        FillColor = Canvas.GetRandomColor();
    }

    /// <inheritdoc/>
    public override Circle Clone(Transform? transform = null)
    {
        var circle = new Circle(Center, Radius, Color)
            { FillColor = FillColor };
        if (transform is Transform t)
            Apply(t);
        return circle;
    }
}