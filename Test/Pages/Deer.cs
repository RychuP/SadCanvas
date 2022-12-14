using SadConsole.Input;
using System;
using Test.Screen;
namespace Test.Pages;

internal class Deer : Page
{
    Info _info;
    
    public Deer() : base("Deer", "Trying to trace the picture of the deer.")
    {
        _ = new Canvas("Res/Images/deer.png")
        {
            Parent = this
        };

        //var c = new Tracer()
        //{
        //    Parent = this
        //};

        _info = new("Mouse Position is: ", this);
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        _info.Print(state.SurfacePixelPosition.ToString());
        return base.ProcessMouse(state);
    }
}

class Tracer : ScreenSurface
{
    

    public Tracer() : base(Settings.Rendering.RenderWidth, Settings.Rendering.RenderHeight - 32)
    {
        
    }


}

class Info : ScreenSurface
{
    string _title;

    public Info(string title, ScreenObject parent) : base(title.Length + 10, 1)
    {
        _title = title;
        Surface.DefaultBackground = Color.White;
        Surface.DefaultForeground = Color.Black;
        Surface.Clear();
        Surface.Print(2, 0, title);
        Parent = parent;
        Position = (1, 1);
    }

    public void Print(string t)
    {
        Surface.Print(2, 0, _title + t);
    }
}
