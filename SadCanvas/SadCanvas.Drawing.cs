using SadCanvas.Shapes;
namespace SadCanvas;

public partial class Canvas : ScreenObject, IDisposable
{
    readonly string OutOfRangeMsg = "Pixel position is out of range";

    /// <summary>
    /// Fills the area
    /// </summary>
    /// <param name="color"></param>
    public void Fill(MonoColor color)
    {
        Array.Fill(Cache, color);
        IsDirty = true;
    }

    public void SetPixel(Point position, MonoColor color)
    {
        int index = position.ToIndex(Width);
        if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(OutOfRangeMsg);
        Cache[index] = color;
        IsDirty = true;
    }

    public MonoColor GetPixel(Point position)
    {
        int index = position.ToIndex(Width);
        if (index < 0 || index >= Size) throw new ArgumentOutOfRangeException(OutOfRangeMsg);
        return Cache[index];
    }

    public void Draw(Shape shape)
    {

    }

    public void DrawLine()
    {

    }

    public void DrawCircle()
    {

    }

    public void DrawBox()
    {

    }
}
