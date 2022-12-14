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
    public Circle(Point center, int radius, Color? color = null, Color? fillColor = null, int? edgeCount = null) :
        base(center, radius, radius, color, fillColor, edgeCount)
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