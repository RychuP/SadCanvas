namespace SadCanvas.Shapes;

/// <summary>
/// A primitive elliptical <see cref="Shape"/>.
/// </summary>
public class Ellipse : Polygon
{
    /// <summary>
    /// Horizontal radius length.
    /// </summary>
    public int RadiusX { get; init; }

    /// <summary>
    /// Vertical radius length.
    /// </summary>
    public int RadiusY { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    /// <param name="color">Color of the edges.</param>
    /// <param name="fillColor">Color of the interior area.</param>
    public Ellipse(Point center, int radiusX, int radiusY, Color? color = null, Color? fillColor = null, int? edgeCount = null) :
        base(GetVertices(center, radiusX, radiusY, edgeCount), color, fillColor)
    {
        (Center, RadiusX, RadiusY) = (center.ToVector2(), RadiusX, RadiusY);
    }

    /// <inheritdoc/>
    public override Ellipse Clone(Transform? transform = null)
    {
        var ellipse = new Ellipse(Center.ToSadPoint(), RadiusX, RadiusY, Color, FillColor, Vertices.Length);
        if (transform is Transform t)
            ellipse.Apply(t);
        return ellipse;
    }

    /// <summary>
    /// Generates vertices for an <see cref="Ellipse"/>.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radiusX">Horizontal radius.</param>
    /// <param name="radiusY">Vertical radius.</param>
    /// <param name="edgeCount">Number of edges.</param>
    /// <returns>Points that will form an <see cref="Ellipse"/>.</returns>
    /// <exception cref="ArgumentException"></exception>
    static Vector2[] GetVertices(Point center, int radiusX, int radiusY, int? edgeCount = null)
    {
        if (radiusX <= 0 || radiusY <= 0) throw new ArgumentException("Radius cannot be 0 or negative.");

        edgeCount = edgeCount is null ?
            Math.Max(radiusX, radiusY) :
            edgeCount < 3 ? 3 : edgeCount.Value;

        var c = center.ToVector2();
        Vector2[] points = new Vector2[edgeCount.Value];
        double currentAngle = 0d;
        double step = 2.0 * Math.PI / edgeCount.Value;
        for (var i = 0; i < edgeCount.Value; i++, currentAngle += step)
        {
            var x = Convert.ToInt32(radiusX * Math.Cos(currentAngle));
            var y = Convert.ToInt32(radiusY * Math.Sin(currentAngle));
            points[i] = c + new Vector2(x, y);
        }
        return points;
    }
}