# SadCanvas v0.1.145
Canvas is a fairly simple class deriving from 
[ScreenObject](https://github.com/Thraka/SadConsole/blob/master/SadConsole/ScreenObject.cs) 
that allows direct manipulation of texture pixels used to render it to the screen. 
It offers basic texture management and pixel drawing tools.

Available to download as [Nuget](https://www.nuget.org/packages/SadCanvas/).

Alpha state. Everything is subject to change.

## Buffer, Texture and IsDirty flag
Ultimately, [Texture](https://github.com/RychuP/SadCanvas/blob/master/SadCanvas/SadCanvas.Texture.cs) defines what will be drawn
on the screen. It provides its own SetData/GetData methods that can be used to change it. It is, however, not recommended to use them
often per Update. Even the most efficient overload that sets a single pixel is extremely slow when done repeatedly. 

For this reason Canvas uses a [Buffer](https://github.com/RychuP/SadCanvas/blob/master/SadCanvas/SadCanvas.Buffer.cs) for its
pixel drawing methods. When all the pixel manipulations are finished and the IsDirty flag is set to true (done automatically
by drawing methods), the buffer data gets sent to texture using its SetData method.

It is assumed that not all Canvas instances will need a buffer, therefore it is not allocated straight away, only when first needed.

If you do any work with Buffer with outside methods, make sure to set the IsDirty flag to true.

If you do any work with Texture with outside methods, make sure to not trigger setting the IsDirty flag to true,
so as not to overwrite your texture changes with buffer data. Saying that, Buffer defines methods to synchronise it with 
texture again if needed, or to dispose of it and free the memory.

## Shapes
Drawing methods use locally defined [Shapes](https://github.com/RychuP/SadCanvas/tree/master/SadCanvas/Shapes) and work also
with some of the shapes defined by SadRogue.Primitives.

Supported are basic polygons that can be drawn as an outline or filled with color. They can be transformed with Offset, Scale
and Rotate methods.

## Screenshots:

![Loading Images](https://github.com/RychuP/SadCanvas/blob/master/screenshot.png)

![Resizing](https://github.com/RychuP/SadCanvas/blob/master/screenshot_resize.png)

![Texture Utilities](https://github.com/RychuP/SadCanvas/blob/master/screenshot_texture_utilities.png)

![Pixel Drawing](https://github.com/RychuP/SadCanvas/blob/master/screenshot_drawing_filled.png)