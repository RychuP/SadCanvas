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
    public Triangle(Point corner1, Point corner2, Point corner3, Color? color = null, Color? fillColor = null) : 
        base(new Vector2[] { corner1.ToVector2(), corner2.ToVector2(), corner3.ToVector2() }, color, fillColor) 
    { }

    /// <inheritdoc/>
    public override Triangle Clone(Transform? transform = null)
    {
        var triangle = new Triangle(Vertices[0].ToSadPoint(), Vertices[1].ToSadPoint(), Vertices[2].ToSadPoint(), Color, FillColor);
        if (transform is Transform t)
            triangle.Apply(t);
        return triangle;
    }
}