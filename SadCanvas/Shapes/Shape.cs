﻿namespace SadCanvas.Shapes;

/// <summary>
/// A primitive form consisting of at least one point that can be drawn on the screen.
/// </summary>
public abstract class Shape
{
    /// <summary>
    /// Mean position of all vertices.
    /// </summary>
    Point? _center;

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
    public Point Center
    {
        get
        {
            if (_center.HasValue)
                return _center.Value;
            else
            {
                Point sumOfAllVertices = (0, 0);
                foreach (var point in Vertices)
                    sumOfAllVertices += point;
                _center = sumOfAllVertices / Vertices.Length;
                return _center.Value;
            }
        }
        protected init => _center = value;
    }

    /// <summary>
    /// All coordinates that form this <see cref="Shape"/>.
    /// </summary>
    public abstract Point[] Vertices { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Shape"/> with the given <paramref name="color"/>.
    /// </summary>
    /// <param name="color">Color of the outline.</param>
    public Shape(MonoColor? color = null)
    {
        Color = color is null ? DefaultColor : color.Value;
    }

    /// <summary>
    /// Rotates all vertices around the <see cref="Center"/> by the given <paramref name="angle"/>.
    /// </summary>
    /// <param name="angle">Angle in radians.</param>
    public void Rotate(float angle)
    {
        var cos = (float)Math.Cos(angle);
        var sin = (float)Math.Sin(angle);

        for (int i = 0; i < Vertices.Length; i++)
        {
            Point temp = Vertices[i] - Center;
            Vertices[i] = (Convert.ToInt32(cos * temp.X - sin * temp.Y) + Center.X,
               Convert.ToInt32(sin * temp.X + cos * temp.Y) + Center.Y);
        }
    }

    /// <summary>
    /// Scales the <see cref="Shape"/> from its center point.
    /// </summary>
    /// <param name="scale"></param>
    public void Scale(float scale)
    {
        for (int i = 0; i < Vertices.Length; i++)
        {
            Point temp = Vertices[i] - Center;
            Vertices[i] = temp * scale + Center;
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
        _center = Center + vector;
    }

    /// <summary>
    /// Moves all vertices by x and y values.
    /// </summary>
    /// <param name="x">Delta X to be applied to all vertices.</param>
    /// <param name="y">Delta Y to be applied to all vertices.</param>
    public void Offset(int x, int y) => Offset(new Point(x, y));

    /// <summary>
    /// Applies offset, rotation and scale from the <paramref name="transform"/>.
    /// </summary>
    /// <param name="transform">Transform to be applied.</param>
    public void Apply(Transform transform)
    {
        Rotate(transform.Rotation);
        Scale(transform.Scale);
        Offset(transform.Offset);
    }

    /// <summary>
    /// Creates a copy with the optionally applied <see cref="Transform"/>.
    /// </summary>
    /// <param name="transform">Transform that can be applied during cloning.</param>
    /// <returns>A copy of the <see cref="Shape"/> instance.</returns>
    public abstract Shape Clone(Transform? transform = null);

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