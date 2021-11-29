namespace SadCanvas.Shapes;

/// <summary>
/// A primitive form that can be drawn on the screen.
/// </summary>
/// <param name="Color">Color of the figure.</param>
public abstract record Shape(MonoColor Color)
{
    /// <summary>
    /// Default color for a shape.
    /// </summary>
    public static readonly MonoColor DefaultColor = MonoColor.White;

    /// <summary>
    /// Mean position of all the points in the figure.
    /// </summary>
    public abstract Point Center { get; }
}