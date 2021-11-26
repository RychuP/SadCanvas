namespace Test.Screen;

/// <summary>
/// Description that can be attached to instances of a <see cref="Canvas"/> class.
/// </summary>
internal class Description : ScreenSurface
{
    public Description(Canvas parent, HorizontalAlignment alignment, params string[] headers) : base(parent.CellArea.Width, headers.Length)
    {
        Parent = parent;
        Position = (0, -headers.Length);
        Surface.DefaultBackground = Color.Gray;
        Surface.Clear();

        for (int y = 0; y < headers.Length; y++)
        {
            Surface.Print(0, y, headers[y].Align(alignment, Surface.Width));
        }
    }
}