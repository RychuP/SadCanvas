# SadCanvas v0.1.125
Canvas is a fairly simple class deriving from 
[ScreenObject](https://github.com/Thraka/SadConsole/blob/master/SadConsole/ScreenObject.cs) 
that allows direct manipulation of texture pixels used to render it to the screen. 
It offers basic texture management and pixel drawing tools.

Available to download as [Nuget](https://www.nuget.org/packages/SadCanvas/).

## Buffer, Texture and IsDirty flag
- Drawing methods set IsDirty flag to true automatically. Set it yourself only when you've done some work with the buffer
using outside methods and want its data sent to texture during Update phase. 
- If you work with Canvas texture using its SetData/GetData methods, make sure to not trigger setting IsDirty to true, 
otherwise buffer will overwrite texture with its data during Update.
- Buffer gets allocated in memory only when needed. It can be offloaded from memory, reloaded with current texture data, 
sent to texture when IsDirty is set to true or it doesn't need to be used at all. There are many ways to work with Canvas.

## Screenshots:

![Loading Images](https://github.com/RychuP/SadCanvas/blob/master/screenshot.png)

![Resizing](https://github.com/RychuP/SadCanvas/blob/master/screenshot_resize.png)

![Texture Utilities](https://github.com/RychuP/SadCanvas/blob/master/screenshot_texture_utilities.png)

![Pixel Drawing](https://github.com/RychuP/SadCanvas/blob/master/screenshot_drawing.png)