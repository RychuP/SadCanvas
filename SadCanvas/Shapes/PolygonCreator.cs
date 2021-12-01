namespace SadCanvas.Shapes;

/// <summary>
/// Provides methods for creating polygons.
/// </summary>
public class PolygonCreator
{
    readonly List<Point> _vertices;
    Point _arcCenter = (0, 0);

    /// <summary>
    /// Creates a new instance with the given <paramref name="start"/> <see cref="Point"/>.
    /// </summary>
    /// <param name="start">Start point for the new polygon.</param>
    public PolygonCreator(Point start)
    {
        _vertices = new List<Point>() { start };
    }

    /// <summary>
    /// Makes a straight line from the last point to the <paramref name="destination"/>.
    /// </summary>
    /// <param name="destination">Destination point to make a line to.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator GoTo(Point destination)
    {
        _vertices.Add(destination);
        return this;
    }

    /// <summary>
    /// Makes a straight line from the last point to the coordinates x and y.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator GoTo(int x, int y) => GoTo(new Point(x, y));

    /// <summary>
    /// Makes a straight, horizontal line.
    /// </summary>
    /// <param name="length">Length of the line (can be positive or negative).</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator GoHorizontal(int length)
    {
        _vertices.Add(new Point(_vertices.Last().X + length, _vertices.Last().Y));
        return this;
    }

    /// <summary>
    /// Makes a straight, vertical line.
    /// </summary>
    /// <param name="length">Length of the line (can be positive or negative).</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator GoVertical(int length)
    {
        _vertices.Add(new Point(_vertices.Last().X, _vertices.Last().Y + length));
        return this;
    }

    /// <summary>
    /// Creates a line that will turn 90 degrees left from the last line.
    /// </summary>
    /// <param name="length">Length of the new line.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator TurnLeft(int length)
    {
        Turn90(length);
        return this;
    }

    /// <summary>
    /// Creates a line that will turn 90 degrees right from the last line.
    /// </summary>
    /// <param name="length">Length of the new line.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator TurnRight(int length)
    {
        Turn90(length, false);
        return this;
    }

    /// <summary>
    /// Creates a line of given <paramref name="length"/> that will form the <paramref name="angle"/> with the previous line.
    /// </summary>
    /// <param name="angle">Angle in radians by which to turn from the last line.</param>
    /// <param name="length">Length of the new line to be created.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    /// <exception cref="ArgumentException"></exception>
    public PolygonCreator TurnBy(float angle, int length)
    {
        int count = _vertices.Count;
        if (count < 2) throw new ArgumentException("This command needs at least two previous points to work with.");

        var endPoint = _vertices.Last();
        var prevPoint = _vertices[count - 2];
        var cos = (float)Math.Cos(angle);
        var sin = (float)Math.Sin(angle);

        var newPoint = endPoint - prevPoint;
        newPoint = (Convert.ToInt32(cos * newPoint.X - sin * newPoint.Y) + prevPoint.X,
           Convert.ToInt32(sin * newPoint.X + cos * newPoint.Y) + prevPoint.Y);

        var line = new Line(endPoint, newPoint);
        var (X, Y) = line.GetUnit();
        int x = Convert.ToInt32(X * length);
        int y = Convert.ToInt32(Y * length);
        _vertices.Add(endPoint + (x, y));

        return this;
    }

    void Turn90(int length, bool left = true)
    {
        int count = _vertices.Count;
        if (count < 2) throw new ArgumentException("This command needs at least two previous points to work with.");

        var endPoint = _vertices[count - 1];
        var line = new Line(_vertices[count - 2], endPoint);
        var (X, Y) = line.GetUnit();

        // unit vector turned 90 degrees left or right
        var unitTurned90 = left ? (-Y, X) : (Y, -X);
        int x = Convert.ToInt32(unitTurned90.Item1 * length);
        int y = Convert.ToInt32(unitTurned90.Item2 * length);
        _vertices.Add(endPoint + (x, y));
    }

    /// <summary>
    /// Set the center point for creating an arc.
    /// </summary>
    /// <param name="arcCenter">Point around which an arc can be created.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator SetArcCenter(Point arcCenter)
    {
        _arcCenter = arcCenter;
        return this;
    }

    /// <summary>
    /// Sets the last added point as the center for commands creating arcs.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator SetArcCenter() => SetArcCenter(_vertices.Last());

    /// <summary>
    /// Sets the point with given coordinates as the center for commands creating arcs.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator SetArcCenter(int x, int y) => SetArcCenter(new Point(x, y));

    /// <summary>
    /// Sets the sum of the last added point and the provided <paramref name="deltaChange"/> as the center for commands creating arcs.
    /// </summary>
    /// <param name="deltaChange"></param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator SetArcCenterBy(Point deltaChange) => SetArcCenter(_vertices.Last() + deltaChange);

    /// <summary>
    /// Sets the sum of the last added point and the provided coordinates x and y as the center for commands creating arcs.
    /// </summary>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator SetArcCenterBy(int x, int y) => SetArcCenter(_vertices.Last() + new Point(x, y));

    /// <summary>
    /// Moves the arc center by the provided vector <paramref name="deltaChage"/>.
    /// </summary>
    /// <param name="deltaChage">Point to be added to the current position of the arc center.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator MoveArcCenterBy(Point deltaChage) => SetArcCenter(_arcCenter + deltaChage);

    /// <summary>
    /// Moves the arc center by the provided vector coordinates x and y.
    /// </summary>
    /// <param name="x">X coordinate to be added to the current position of the arc center.</param>
    /// <param name="y">Y coordinate to be added to the current position of the arc center.</param>
    /// <returns>Current instance of the <see cref="PolygonCreator"/>.</returns>.
    public PolygonCreator MoveArcCenterBy(int x, int y) => MoveArcCenterBy(new Point(x, y));

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

        var temp = _vertices.Last();
        float step = angle / edgeCount;
        var cos = (float)Math.Cos(step);
        var sin = (float)Math.Sin(step);

        for (int i = 0; i < edgeCount; i++)
        {
            temp -= _arcCenter;
            temp = (Convert.ToInt32(cos * temp.X - sin * temp.Y) + _arcCenter.X,
               Convert.ToInt32(sin * temp.X + cos * temp.Y) + _arcCenter.Y);
            _vertices.Add(temp);
        }
        return this;
    }

    /// <summary>
    /// Converts the list of vertices to an array and returns it.
    /// </summary>
    public Point[] GetVertices() => _vertices.ToArray();

    /// <summary>
    /// Creates a polygon from the saved vertices.
    /// </summary>
    /// <param name="color">Color of the polygon.</param>
    /// <returns>A new instance of <see cref="Polygon"/>.</returns>
    public Polygon GetPolygon(MonoColor? color = null) => new(GetVertices(), color);
}