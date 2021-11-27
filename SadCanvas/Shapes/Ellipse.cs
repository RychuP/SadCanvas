namespace SadCanvas.Shapes;

/// <summary>
/// A primitive ellipse that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Ellipse : Polygon
{
    /// <summary>
    /// Horizontal radius of the <see cref="Ellipse"/>.
    /// </summary>
    public int RadiusX { get; init; }

    /// <summary>
    /// Vertical radius of the <see cref="Ellipse"/>.
    /// </summary>
    public int RadiusY { get; init; }

    /// <summary>
    /// Number of sides of the <see cref="Ellipse"/> (more means smoother edges).
    /// </summary>
    public int NoOfSides { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    public Ellipse(Point center, int radiusX, int radiusY, int noOfSides)
    {
        (Position, RadiusX, RadiusY, NoOfSides) = (center, radiusX, radiusY, noOfSides);
        Vertices = new Point[NoOfSides];
        CalculateVertices();
    }

    /// <summary>
    /// Creates an instance of <see cref="Ellipse"/> with the given parameters.
    /// </summary>
    public Ellipse(Point center, int radiusX, int radiusY, int noOfSides, MonoColor lineColor) :
        this(center, radiusX, radiusY, noOfSides)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Ellipse"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Ellipse"/> for.</param>
    /// <param name="minRadiusLength">Minumum radius length.</param>
    /// <param name="maxRadiusLength">Maximum radius length.</param>
    public Ellipse(Canvas canvas, int minRadiusLength = MinLength, int maxRadiusLength = MaxLength)
    {
        while (true)
        {
            var pos = canvas.GetRandomPosition();
            int xDif = canvas.Width - pos.X;
            int yDif = canvas.Height - pos.Y;
            int maxRadiusXFromPos = Math.Min(xDif, pos.X);
            int maxRadiusYFromPos = Math.Min(yDif, pos.Y);
            if (maxRadiusXFromPos >= minRadiusLength && maxRadiusYFromPos >= minRadiusLength)
            {
                int radiusX = Game.Instance.Random.Next(minRadiusLength, maxRadiusXFromPos);
                int radiusY = Game.Instance.Random.Next(minRadiusLength, maxRadiusYFromPos);
                if (radiusX <= maxRadiusLength && radiusY <= maxRadiusLength)
                {
                    NoOfSides = Math.Max(radiusX, radiusY);
                    Position = pos;
                    RadiusX = radiusX;
                    RadiusY = radiusY;
                    Vertices = new Point[NoOfSides];
                    LineColor = Canvas.GetRandomColor();
                    CalculateVertices();
                    break;
                }
            }
        }
    }

    void CalculateVertices()
    {
        var t = 0.0;
        var dt = 2.0 * Math.PI / NoOfSides;
        for (var i = 0; i < NoOfSides; i++, t += dt)
        {
            var x = Convert.ToInt32(RadiusX * Math.Cos(t));
            var y = Convert.ToInt32(RadiusY * Math.Sin(t));
            Vertices[i] = Position + (x, y);
        }
    }
}