using SadConsole.Input;
using Test.Pages;

namespace Test.Screen;

internal class Demo : ScreenObject
{
    readonly Header _header;
    readonly DisplayArea _displayArea = new();

    readonly Page[] _pages = new Page[]
    {
        new CustomPolygons(),
        new Workshop(),
        new RotatingDecagons(),
        new PixelDrawing(),
        new LoadingImages(),
        new ResizingCanvas(),
        new ChangingTextures(),
    };

    public Demo()
    {
        _header = new(_pages[0], _pages.Length);

        Game.Instance.Screen = this;
        Game.Instance.DestroyDefaultStartingConsole();

        Children.Add(_header);
        Children.Add(_displayArea);

        var page = _pages[0];
        _displayArea.Add(page);

        IsFocused = true;
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.HasKeysPressed)
        {
            if (keyboard.IsKeyPressed(Keys.Left))
                return PrevPage();
            else if (keyboard.IsKeyPressed(Keys.Right))
                return NextPage();
        }
        return true;
    }

    bool NextPage() => ChangePage(_pages.Length - 1, 1, _pages[0]);

    bool PrevPage() => ChangePage(0, -1, _pages.Last());

    bool ChangePage(int testIndex, int step, Page overlappingPage)
    {
        // get the index of current page
        int currentPageIndex = Array.IndexOf(_pages, _displayArea.Children[0]);
        _displayArea.Children.Clear();

        // pull the next page from array and display it
        int nextIndex = currentPageIndex + step;
        var newPage = currentPageIndex == testIndex ? overlappingPage : _pages[nextIndex];
        _displayArea.Children.Add(newPage);

        // change header title and summary to describe the page
        _header.SetHeader(newPage, step == -1 ? Direction.Left : Direction.Right);

        // handled
        return true;
    }
}