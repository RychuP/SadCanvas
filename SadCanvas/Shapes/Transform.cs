namespace SadCanvas.Shapes;

/// <summary>
/// Set of transform values that can be applied to a <see cref="Shape"/>.
/// </summary>
public record Transform
{
    /// <summary>
    /// Offset to be applied.
    /// </summary>
    public Vector2 Offset { get; init; }

    /// <summary>
    /// Scale to be applied.
    /// </summary>
    public float Scale { get; init; }

    /// <summary>
    /// Rotation to be applied.
    /// </summary>
    public float Rotation { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="Transform"/> with given parameters.
    /// </summary>
    /// <param name="offset">Offset to be applied.</param>
    /// <param name="rotation">Rotation to be applied.</param>
    /// <param name="scale">Scale to be applied.</param>
    public Transform(Vector2? offset = null, float rotation = 0, float scale = 0)
    {
        (Rotation, Scale) = (rotation, scale);
        Offset = offset is null ? Vector2.Zero : offset.Value;
    }
}