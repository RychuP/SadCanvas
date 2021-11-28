namespace SadCanvas;

// Methods and properties relating to buffer.
public partial class Canvas : ScreenObject, IDisposable
{
    private MonoColor[] _buffer = Array.Empty<MonoColor>();

    private void InitializeBuffer()
    {
        _buffer = new MonoColor[Size];
        _texture.GetData(_buffer);
    }

    /// <summary>
    /// Buffer of <see cref="MonoColor"/> pixels in backing texture.
    /// </summary>
    public MonoColor[] Buffer
    {
        get
        {
            if (_buffer.Length == Size)
                return _buffer;
            else
            {
                InitializeBuffer();
                return _buffer;
            }
        }
    }

    /// <summary>
    /// Frees up memory taken by the buffer.
    /// </summary>
    public void FreeBuffer() =>
        _buffer = Array.Empty<MonoColor>();

    /// <summary>
    /// Refreshes buffer with current data from texture.
    /// </summary>
    public void ReloadBuffer() =>
        _texture.GetData(_buffer);
}