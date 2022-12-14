namespace SadCanvas.Shapes;

/// <summary>
/// A primitive rectangular <see cref="Shape"/>.
/// </summary>
public class Rectangle : Polygon
{
    /// <summary>
    /// Length of the horizontal side.
    /// </summary>
    public int Width { get; init; }

    /// <summary>
    /// Length of the vertical side.
    /// </summary>
    public int Height { get; init; }

    /// <summary>
    /// Start position (top left) from which the rest of the vertices was generated from.
    /// </summary>
    public Point Origin { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Rectangle"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="width">Length of the horizontal side.</param>
    /// <param name="height">Length of the vertical side.</param>
    /// <param name="color">Color of the edges.</param>
    /// <param name="fillColor">Color of the interior area.</param>
    public Rectangle(Point origin, int width, int height, Color? color = null, Color? fillColor = null) : 
        base(GetVertices(origin, width, height), color, fillColor)
    {
        Origin = origin;
        (Width, Height) = (width, height);
    }

    /// <inheritdoc/>
    public override Rectangle Clone(Transform? transform = null)
    {
        var rect = new Rectangle(Origin, Width, Height, Color, FillColor);
        if (transform is Transform t)
            rect.Apply(t);
        return rect;
    }

    static Vector2[] GetVertices(Point origin, int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentException("Width and height cannot be 0 or negative.");

        Vector2 v = origin.ToVector2();
        return new Vector2[]
        {
            v,
            v + new Vector2(width, 0),
            v + new Vector2(width, height),
            v + new Vector2(0, height)
        };
    }
}