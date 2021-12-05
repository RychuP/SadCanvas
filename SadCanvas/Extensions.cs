﻿namespace SadCanvas;

/// <summary>
/// Extension methods used by the library
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Converts <see cref="Microsoft.Xna.Framework.Rectangle"/> to <see cref="Rectangle"/>.
    /// </summary>
    public static Rectangle ToSadRectangle(this Microsoft.Xna.Framework.Rectangle rect) =>
        new(rect.X, rect.Y, rect.Width, rect.Height);

    /// <summary>
    /// Returns a random <see cref="Point"/> with the area.
    /// </summary>
    /// <param name="area">Area of the <see cref="Rectangle"/>.</param>
    public static Point GetRandomPosition(this Rectangle area) =>
        (Canvas.GetRandomInt(area.X, area.X + area.Width), Canvas.GetRandomInt(area.Y, area.Y + area.Height));

    /// <summary>
    /// Converts a <see cref="Vector2"/> to <see cref="Point"/>.
    /// </summary>
    public static Point ToSadPoint(this Vector2 v) =>
        new(Convert.ToInt32(v.X), Convert.ToInt32(v.Y));

    /// <summary>
    /// Returns a unit vector of the line formed by the two points.
    /// </summary>
    /// <param name="v">Delta change.</param>
    public static Vector2 ToUnitVector(this Vector2 v)
    {
        v.Normalize();
        return v;
    }

    /// <summary>
    /// Returns a <see cref="Vector2"/> from a delta change point.
    /// </summary>
    /// <param name="p">Delta change.</param>
    public static Vector2 ToVector(this Point p) =>
        new( p.X, p.Y);

    /// <summary>
    /// Rotates the vector around the origin (0, 0) by the given angle .
    /// </summary>
    /// <param name="v">Current vector.</param>
    /// <param name="angle">Angle in radians.</param>
    /// <returns>New vector rotated around the origin.</returns>
    public static Vector2 Rotate(this Vector2 v, float angle)
    {
        var cos = (float)Math.Cos(angle);
        var sin = (float)Math.Sin(angle);
        var newVector = new Vector2(cos * v.X - sin * v.Y, sin * v.X + cos * v.Y);
        return newVector;
    }
}