namespace Test;

internal class DisplayArea : ScreenSurface
{
    public DisplayArea() : base(Program.Width, Program.Height - Header.Height) { }

    public void Add(Page page) => Children.Add(page);
}