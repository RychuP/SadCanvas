namespace Test;

internal class DisplayArea : ScreenSurface
{
    public DisplayArea() : base(Program.Width, Program.Height - Header.Height)
    {
        Position = (0, 2);
    }

    public void Add(Page page) => Children.Add(page);
}