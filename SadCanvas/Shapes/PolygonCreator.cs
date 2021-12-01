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
    public PolygonCreator GoTo(Point destination)
    {
        _vertices.Add(destination);
        return this;
    }

    /// <summary>
    /// Makes a straight line from the last point to the coordinates x and y.
    /// </summary>
    public PolygonCreator GoTo(int x, int y) => GoTo(new Point(x, y));

    public PolygonCreator GoHorizontal(int length)
    {
        _vertices.Add(new Point(_vertices.Last().X + length, _vertices.Last().Y));
        return this;
    }

    public PolygonCreator GoVertical(int length)
    {
        _vertices.Add(new Point(_vertices.Last().X, _vertices.Last().Y + length));
        return this;
    }

    /// <summary>
    /// Creates a line that will turn 90 degrees left from the last line.
    /// </summary>
    /// <param name="length">Length of the new line.</param>
    public PolygonCreator TurnLeft(int length)
    {
        Turn90(length);
        return this;
    }

    /// <summary>
    /// Creates a line that will turn 90 degrees right from the last line.
    /// </summary>
    /// <param name="length">Length of the new line.</param>
    public PolygonCreator TurnRight(int length)
    {
        Turn90(length, false);
        return this;
    }

    void Turn90(int length, bool left = true)
    {
        int count = _vertices.Count;
        if (count < 2) throw new ArgumentException("This command needs at least two previous points to work with.");

        var endPoint = _vertices[count - 1];
        var line = new Line(_vertices[count - 2], endPoint);
        var unit = line.GetUnit();

        var unitTurned90 = left ? new Point(-unit.Y, unit.X) : new Point(unit.Y, -unit.X);

        _vertices.Add(endPoint + unitTurned90 * length);
    }

    /// <summary>
    /// Set the center point for creating an arc.
    /// </summary>
    /// <param name="arcCenter">Point around which an arc can be created.</param>
    public PolygonCreator SetArcCenter(Point arcCenter)
    {
        _arcCenter = arcCenter;
        return this;
    }

    public PolygonCreator SetArcCenter() => SetArcCenter(_vertices.Last());

    public PolygonCreator SetArcCenter(int x, int y) => SetArcCenter(new Point(x, y));

    public PolygonCreator SetArcCenterBy(Point deltaChange) => SetArcCenter(_vertices.Last() + deltaChange);

    public PolygonCreator SetArcCenterBy(int x, int y) => SetArcCenter(_vertices.Last() + new Point(x, y));

    public PolygonCreator MoveArcCenterBy(Point deltaChage) => SetArcCenter(_arcCenter + deltaChage);

    public PolygonCreator MoveArcCenterBy(int x, int y) => MoveArcCenterBy(new Point(x, y));

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
    public Polygon GetPolygon(MonoColor? color = null) => new(GetVertices(), color);
}