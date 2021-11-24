namespace Test;

internal class Page : ScreenSurface
{
    public string Title { get; init; }

    public string Summary { get; init; }

    public Page(string title, string summary) : base(Program.Width, Program.Height - Header.Height)
    {
        Title = title;
        Summary = summary;
    }

    protected void Add(Canvas canvas)
    {
        Children.Add(canvas);
        canvas.UsePixelPositioning = true;
        canvas.Position = (Settings.Rendering.RenderWidth / 2 - canvas.Width / 2, 
            (Settings.Rendering.RenderHeight - 32) / 2 - canvas.Height / 2);
    }
}