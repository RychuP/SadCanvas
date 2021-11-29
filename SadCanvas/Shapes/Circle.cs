namespace SadCanvas.Shapes;

/// <summary>
/// A primitive circle.
/// </summary>
public record Circle : Polygon
{
    readonly Point _center;

    /// <summary>
    /// Radius of the <see cref="Circle"/>.
    /// </summary>
    public int Radius { get; init; }

    /// <summary>
    /// Center point.
    /// </summary>
    public override Point Center => _center;

    /// <summary>
    /// Creates an instance with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radius">Length of radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    public Circle(Point center, int radius, int edgeCount) : 
        this(center, radius, edgeCount, DefaultColor)
    { }

    /// <summary>
    /// Creates an instance of <see cref="Circle"/> with the given parameters.
    /// </summary>
    /// <param name="center">Center point.</param>
    /// <param name="radius">Length of radius.</param>
    /// <param name="edgeCount">Number of edges (more means smoother outline).</param>
    /// <param name="color">Color of edges.</param>
    public Circle(Point center, int radius, int edgeCount, MonoColor color) :
        base(Ellipse.GetVertices(center, radius, radius, edgeCount), color)
    {
        (_center, Radius) = (center, Radius);
    }

    /// <summary>
    /// Generates a random <see cref="Circle"/> that will fit within the constraints of the <paramref name="area"/>.
    /// </summary>
    /// <param name="area">Area to generate a random <see cref="Circle"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusLength">Maximum radius length.</param>
    public static Circle GetRandomCircle(SadRogue.Primitives.Rectangle area, int minRadiusLength, int maxRadiusLength) 
    {
        if (minRadiusLength <= 0 || maxRadiusLength <= 0) throw new ArgumentException("Circle constraints cannot be 0 or negative.");
        if (maxRadiusLength < minRadiusLength) throw new ArgumentException("Max radius cannot be smaller than min radius.");
        if (area.Width < minRadiusLength * 2 || area.Height < minRadiusLength * 2) throw new ArgumentException("Area width and height cannot be smaller than min diameter of the circle.");

        while (true)
        {
            var pos = area.GetRandomPosition();
            int xDif = area.Width - pos.X;
            int yDif = area.Height - pos.Y;
            int maxRadiusXFromPos = Math.Min(xDif, pos.X);
            int maxRadiusYFromPos = Math.Min(yDif, pos.Y);
            int maxRadius = Math.Min(maxRadiusXFromPos, maxRadiusYFromPos);
            if (maxRadius >= minRadiusLength)
            {
                int radius = Canvas.GetRandomInt(minRadiusLength, maxRadius);
                if (radius <= maxRadiusLength)
                {
                    var sideCount = radius;
                    return new Circle(pos, radius, sideCount, Canvas.GetRandomColor());
                }
            }
        }
    }
}