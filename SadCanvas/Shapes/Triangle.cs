namespace SadCanvas.Shapes;

/// <summary>
/// A primitive triangle that can be drawn on <see cref="Canvas"/>.
/// </summary>
public record Triangle : Polygon
{
    /// <summary>
    /// Creates and instance of <see cref="Polygon"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Triangle"/>.</param>
    /// <param name="corner1">One of the corners of the <see cref="Triangle"/>.</param>
    /// <param name="corner2">Another one of the corners of the <see cref="Triangle"/>.</param>
    public Triangle(Point position, Point corner1, Point corner2) : base(position, corner1, corner2) { }

    /// <summary>
    /// Creates and instance of <see cref="Polygon"/> with the given parameters.
    /// </summary>
    /// <param name="position">Position of the <see cref="Triangle"/>.</param>
    /// <param name="corner1">One of the corners of the <see cref="Triangle"/>.</param>
    /// <param name="corner2">Another one of the corners of the <see cref="Triangle"/>.</param>
    /// <param name="lineColor"><see cref="MonoColor"/> of the outline of the <see cref="Triangle"/>.</param>
    public Triangle(Point position, Point corner1, Point corner2, MonoColor lineColor) : this(position, corner1, corner2)
    {
        LineColor = lineColor;
    }

    /// <summary>
    /// Generates a random <see cref="Triangle"/> that will fit within the constraints of the <paramref name="canvas"/>.
    /// </summary>
    /// <param name="canvas"><see cref="Canvas"/> to generate a random <see cref="Triangle"/> for.</param>
    /// <param name="maxLineLength">Maximum line length.</param>
    /// <param name="minLineLength">Minimum line length.</param>
    public Triangle(Canvas canvas, int minLineLength = MinLength, int maxLineLength = MaxLength)
    {
        while (true)
        {
            var pos1 = canvas.GetRandomPosition();
            var pos2 = canvas.GetRandomPosition();
            var pos3 = canvas.GetRandomPosition();

            double side1 = Line.GetDistance(pos1, pos2);
            double side2 = Line.GetDistance(pos2, pos3);
            double side3 = Line.GetDistance(pos3, pos1);

            if(side1 >= minLineLength && side2 >= minLineLength && side3 >= minLineLength &&
                side1 <= maxLineLength && side2 <= maxLineLength && side3 <= maxLineLength)
            {
                Position = pos1;
                Vertices = new[] { pos1, pos2, pos3 };
                LineColor = Canvas.GetRandomColor();
                break;
            }
        }
    }
}