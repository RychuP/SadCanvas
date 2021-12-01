namespace SadCanvas.Shapes;

/// <summary>
/// Provides methods for creating polygons.
/// </summary>
public class PolygonCreator
{
    readonly List<Point> _vertices;
    Point _arcCenter;

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
    public void GoTo(Point destination)
    {
        _vertices.Add(destination);
    }

    /// <summary>
    /// Creates a line that will turn left 90 degrees from the last line.
    /// </summary>
    /// <param name="length">Length of the new line.</param>
    public void TurnLeft(int length)
    {
        int count = _vertices.Count;
        if (count == 0) throw new ArgumentException("This command needs at least two previous points to work with.");

        var line = new Line(_vertices[count - 1], _vertices[count]);

    }

    /// <summary>
    /// Set the center point for creating an arc.
    /// </summary>
    /// <param name="arcCenter">Point around which an arc can be created.</param>
    public void SetArcCenter(Point arcCenter)
    {
        _arcCenter = arcCenter;
    }

    /// <summary>
    /// Converts the list of vertices to an array and returns it.
    /// </summary>
    public Point[] GetVertices() => _vertices.ToArray();

    /// <summary>
    /// Creates a polygon from the saved vertices.
    /// </summary>
    public Polygon GetPolygon() => new Polygon(GetVertices());
}