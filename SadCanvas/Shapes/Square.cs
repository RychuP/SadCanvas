namespace SadCanvas.Shapes;

/// <summary>
/// A primitive square <see cref="Shape"/>.
/// </summary>
public class Square : Rectangle
{
    /// <summary>
    /// Creates an instance of <see cref="Square"/> with the given parameters.
    /// </summary>
    /// <param name="origin">Origin point.</param>
    /// <param name="sideLength">Length of each side.</param>
    /// <param name="color">Color of the edges.</param>
    /// <param name="fillColor">Color of the interior area.</param>
    public Square(Point origin, int sideLength, Color? color = null, Color? fillColor = null) : 
        base(origin, sideLength, sideLength, color, fillColor)
    { }

    /// <inheritdoc/>
    public override Square Clone(Transform? transform = null)
    {
        var square = new Square(Origin, Width, Color, FillColor);
        if (transform is Transform t)
            square.Apply(t);
        return square;
    }
}