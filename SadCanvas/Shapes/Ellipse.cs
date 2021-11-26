namespace SadCanvas.Shapes;

/// <summary>
/// A basic ellipse that can be drawn on <see cref="Canvas"/>.
/// </summary>
public struct Ellipse : IShape
{
    /// <inheritdoc/>
    public double Area => throw new NotImplementedException();

    /// <inheritdoc/>
    public double Perimeter => throw new NotImplementedException();

    /// <inheritdoc/>
    public Point[] Vertices => throw new NotImplementedException();

    /// <inheritdoc/>
    public MonoColor OutlineColor => throw new NotImplementedException();

    /// <inheritdoc/>
    public MonoColor FillColor => throw new NotImplementedException();
}
