namespace SadCanvas.Shapes;

/// <summary>
/// A primitive form consisting of at least one point that can be drawn on the screen.
/// </summary>
public abstract record Shape
{
    /// <summary>
    /// Default outline color.
    /// </summary>
    public static readonly MonoColor DefaultColor = MonoColor.White;

    /// <summary>
    /// Color of the outline.
    /// </summary>
    public MonoColor Color { get; set; }

    /// <summary>
    /// Calculates mean position of all vertices.
    /// </summary>
    public abstract Point GetCenter();

    /// <summary>
    /// All coordinates that form this <see cref="Shape"/>.
    /// </summary>
    public abstract Point[] Vertices { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Shape"/> with the given <paramref name="color"/>.
    /// </summary>
    /// <param name="color"></param>
    public Shape(MonoColor? color = null)
    {
        Color = color is null ? DefaultColor : color.Value;
    }

    /// <summary>
    /// Rotates all vertices around the <see cref="GetCenter"/> by the given <paramref name="angle"/>.
    /// </summary>
    /// <param name="angle">Angle in radians.</param>
    public void Rotate(float angle)
    {
        var cos = (float)Math.Cos(angle);
        var sin = (float)Math.Sin(angle);
        var center = GetCenter();

        for (int i = 0; i < Vertices.Length; i++)
        {
            Point temp = Vertices[i] - center;
            Vertices[i] = (Convert.ToInt32(cos * temp.X - sin * temp.Y) + center.X,
               Convert.ToInt32(sin * temp.X + cos * temp.Y) + center.Y);
        }
    }

    /// <summary>
    /// Scales the <see cref="Shape"/> from its center point.
    /// </summary>
    /// <param name="scale"></param>
    public void Scale(float scale)
    {
        var center = GetCenter();
        for (int i = 0; i < Vertices.Length; i++)
        {
            Point temp = Vertices[i] - center;
            Vertices[i] = temp * scale + center;
        }
    }

    /// <summary>
    /// Moves all vertices by x and y value of the <paramref name="vector"/>.
    /// </summary>
    /// <param name="vector">Delta change to be applied to all vertices.</param>
    public void Offset(Point vector)
    {
        for (int i = 0; i < Vertices.Length; i++)
            Vertices[i] += vector;
    }

    /// <summary>
    /// Moves all vertices by x and y values.
    /// </summary>
    /// <param name="x">Delta X to be applied to all vertices.</param>
    /// <param name="y">Delta Y to be applied to all vertices.</param>
    public void Offset(int x, int y) => Offset(new Point(x, y));

    /// <summary>
    /// A bounding rectangle that encloses this <see cref="Shape"/>.
    /// </summary>
    public abstract SadRogue.Primitives.Rectangle Bounds { get; }

    /// <summary>
    /// Left most X coordinate.
    /// </summary>
    public abstract int Left { get; }

    /// <summary>
    /// Right most X coordinate.
    /// </summary>
    public abstract int Right { get; }

    /// <summary>
    /// Top most Y coordinate.
    /// </summary>
    public abstract int Top { get; }

    /// <summary>
    /// Bottom most Y coordinate.
    /// </summary>
    public abstract int Bottom { get; }

    /// <summary>
    /// Mode for generation of instances of <see cref="Shape"/>.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Random <see cref="Shape"/> that will fit within given 
        /// </summary>
        Random,

        /// <summary>
        /// <see cref="Shape"/> that will fit within <see cref="SadRogue.Primitives.Rectangle"/> area as best as it can.
        /// </summary>
        Fit
    }
}