# SadCanvas
Canvas class is intended to work very much like a SadConsole.ScreenSurface, but rather than represent a CellSurface 
it allows direct manipulation of texture pixels used to render it to the screen.

Canvas derives from ScreenObject. It forms a new renderable branch and, since there is no intermediate class 
that offers basic renderable funtionalities between ScreenObject and ScreenSurface that Canvas could inherit from,
it copies many properties and methods from ScreenSurface (so as not to reinvent the wheel). 
Please refer to Thraka's implementation of [that class](https://github.com/Thraka/SadConsole/blob/master/SadConsole/ScreenSurface.cs) 
and make your own comparisons to see what was copied.

## Screenshots:

![Parrot](/screenshot.jpg)