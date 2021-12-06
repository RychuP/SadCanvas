namespace SadCanvas.Shapes;

/// <summary>
/// Provides methods for creating polygons.
/// </summary>
public class PolygonCreator
{
    readonly List<Vector2> _vertices;

    /// <summary>
    /// Center vector used to create arcs.
    /// </summary>
    public Vector2 ArcCenter { get; set; } = Vector2.Zero;

    /// <summary>
    /// Creates a new instance of <see cref="PolygonCreator"/>.
    /// </summary>
    /// <param name="x">X coordinate of the start point.</param>
    /// <param name="y">Y coordinate of the start point.</param>
    public PolygonCreator(int x, int y)
    {
        _vertices = new List<Vector2>() { new Vector2(x, y) };
    }

    /// <summary>
    /// Removes all vertices and starts again with the new point.
    /// </summary>
    /// <param name="x">X coordinate of the start point.</param>
    /// <param name="y">Y coordinate of the start point.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator Start(int x, int y)
    {
        _vertices.Clear();
        _vertices.Add(new Vector2(x, y));
        return this;
    }

    /// <summary>
    /// Makes a straight line from the last point to the coordinates x and y.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator GoTo(int x, int y) =>
        GoTo(new Vector2(x, y));

    /// <summary>
    /// Makes a straight line from the last point to the coordinates defined by the vector '<paramref name="destination"/>'.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator GoTo(Vector2 destination)
    {
        if (destination != _vertices.Last())
            _vertices.Add(destination);
        return this;
    }

    /// <summary>
    /// Makes a straight, horizontal line.
    /// </summary>
    /// <param name="length">Length of the line (can be positive or negative).</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator GoHorizontal(int length)
    {
        if (length != 0)
            _vertices.Add(_vertices.Last() + new Vector2(length, 0));
        return this;
    }

    /// <summary>
    /// Makes a straight, vertical line.
    /// </summary>
    /// <param name="length">Length of the line (can be positive or negative).</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator GoVertical(int length)
    {
        if (length != 0)
            _vertices.Add(_vertices.Last() + new Vector2(0, length));
        return this;
    }

    /// <summary>
    /// Creates a line that will turn 90 degrees left from the last line.
    /// </summary>
    /// <param name="length">Length of the new line.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator TurnLeft(int length) =>
        Turn(length, left: true);

    /// <summary>
    /// Creates a line that will turn 90 degrees right from the last line.
    /// </summary>
    /// <param name="length">Length of the new line.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator TurnRight(int length) =>
        Turn(length, right: true);

    /// <summary>
    /// Creates a line of given <paramref name="length"/> that will form the <paramref name="angle"/> with the previous line.
    /// </summary>
    /// <param name="angle">Angle in radians by which to turn from the last line.</param>
    /// <param name="length">Length of the new line to be created.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    /// <exception cref="ArgumentException"></exception>
    public PolygonCreator TurnBy(float angle, int length) =>
        Turn(length, angle);

    PolygonCreator Turn(int length, float angle = 0, bool right = false, bool left = false)
    {
        int count = _vertices.Count;
        if (count < 2) throw new ArgumentException("This command needs at least two previous points to work with.");
        if (length <= 0) return this;

        var endPoint = _vertices.Last();
        var startPoint = _vertices[count - 2];

        if (right || left)
        {
            var v = (endPoint - startPoint).ToUnitVector();
            Vector2 unitVectorTurned90 = left ? new(v.Y, -v.X) : new(-v.Y, v.X);
            _vertices.Add(endPoint + (unitVectorTurned90 * length));
        }
        else
        {
            var rotatedVector = (endPoint - startPoint).Rotate(angle).ToUnitVector() * length;
            _vertices.Add(endPoint + rotatedVector);
        }

        return this;
    }

    /// <summary>
    /// Sets the last added point as the center for commands creating arcs.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator SetArcCenter()
    {
        ArcCenter = _vertices.Last();
        return this;
    }

    /// <summary>
    /// Sets the point with given coordinates as the center for commands creating arcs.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator SetArcCenter(int x, int y)
    {
        ArcCenter = new Vector2(x, y);
        return this;
    }

    /// <summary>
    /// Sets the sum of the last added point and the provided coordinates x and y as the center for commands creating arcs.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator SetArcCenterBy(int x, int y)
    {
        ArcCenter = _vertices.Last() + new Vector2(x, y);
        return this;
    }

    /// <summary>
    /// Moves the arc center by the provided vector coordinates x and y.
    /// </summary>
    /// <param name="x">Value to be added to the current x coordinate of the arc center.</param>
    /// <param name="y">Value to be added to the current y coordinate of the arc center.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator MoveArcCenterBy(int x, int y)
    {
        ArcCenter += new Vector2(x, y);
        return this;
    }

    /// <summary>
    /// Creates an arc around the previously set arc center going from the last added point by the given <paramref name="angle"/> in radians.
    /// </summary>
    /// <param name="angle">Length of the arc in radians.</param>
    /// <param name="edgeCount">Edge count of the arc (more means smoother).</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator MakeArc(float angle, int edgeCount)
    {
        if (angle == 0) return this;
        if (edgeCount < 1) edgeCount = 1;

        var point = _vertices.Last();
        float step = angle / edgeCount;
        var cos = (float)Math.Cos(step);
        var sin = (float)Math.Sin(step);

        for (int i = 0; i < edgeCount; i++)
        {
            point -= ArcCenter;
            point = new Vector2(cos * point.X - sin * point.Y, sin * point.X + cos * point.Y) + ArcCenter;
            _vertices.Add(point);
        }

        return this;
    }

    /// <summary>
    /// Converts the list of vertices to an array and returns it.
    /// </summary>
    public Vector2[] GetVertices() => 
        _vertices.ToArray();

    /// <summary>
    /// Creates a polygon from the saved vertices.
    /// </summary>
    /// <param name="color">Color of the polygon.</param>
    /// <returns>A new instance of <see cref="Polygon"/>.</returns>
    public Polygon GetPolygon(MonoColor? color = null) => 
        new(GetVertices(), color);

    /// <summary>
    /// Last point added to the list of vertices as a vector.
    /// </summary>
    public Vector2 LastVector =>
        _vertices.Last();
}