using Test.Pages;

namespace Test.Screen;

internal class Demo : ScreenObject
{
    readonly Header _header = new();
    readonly DisplayArea _displayArea = new();

    readonly Page[] _pages = new Page[]
    {
        new LoadingImages()
    };

    public Demo()
    {
        Game.Instance.Screen = this;
        Game.Instance.DestroyDefaultStartingConsole();

        Children.Add(_header);
        Children.Add(_displayArea);

        _displayArea.Add(_pages[0]);
        _header.SetHeader(_pages[0]);
    }
}