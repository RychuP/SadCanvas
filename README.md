# SadCanvas
Canvas class is intended to work like a [SadConsole.ScreenSurface](https://github.com/Thraka/SadConsole/blob/master/SadConsole/ScreenSurface.cs), 
but rather than representing a [CellSurface](https://github.com/Thraka/SadConsole/blob/master/SadConsole/CellSurface.cs) 
it allows direct manipulation of texture pixels used to render it to the screen.

Canvas derives from [ScreenObject](https://github.com/Thraka/SadConsole/blob/master/SadConsole/ScreenObject.cs). 
It forms a new renderable branch and, since there is no intermediate class 
that offers basic renderable funtionalities between ScreenObject and ScreenSurface that Canvas could inherit from,
it copies many properties and methods from ScreenSurface (so as not to reinvent the wheel).

What is new are the pixel drawing methods, creating the class directly from an image file, pixel positioning by default
(can be switched to cell positioning which is based on the FontSize)... 

Work in progress.

## Screenshots:

![Parrot](/screenshot.jpg)