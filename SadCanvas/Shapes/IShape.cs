namespace SadCanvas.Shapes;

/// <summary>
/// A basic shape that can be drawn on the <see cref="Canvas"/>.
/// </summary>
public interface IShape
{
    /// <summary>
    /// Area covered by this <see cref="IShape"/>.
    /// </summary>
    double Area { get; }

    /// <summary>
    /// Length of the perimeter of this <see cref="IShape"/>.
    /// </summary>
    double Perimeter { get; }

    /// <summary>
    /// Points that make up this <see cref="IShape"/>.
    /// </summary>
    Point[] Vertices { get; }

    /// <summary>
    /// <see cref="MonoColor"/> used in drawing the outline of this <see cref="IShape"/>.
    /// </summary>
    MonoColor OutlineColor { get; }

    /// <summary>
    /// <see cref="MonoColor"/> used in filling the area of this <see cref="IShape"/>.
    /// </summary>
    MonoColor FillColor { get; }
}