namespace Test;

internal class Header : ScreenSurface
{
    public const int Height = 2;
    public Header() : base(Program.Width, Height)
    {
        Surface.DefaultBackground = Color.DarkGray;
        Surface.DefaultForeground = Color.Yellow;
    }

    public void SetHeader(Page page)
    {
        Surface.Clear();
        Surface.Print(1, 0, page.Title.ToUpper());
        Surface.Print(1, 1, page.Summary, Color.Gray);
    }
}