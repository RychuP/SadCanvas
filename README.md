# SadCanvas
Canvas class is intended to work like a [ScreenSurface](https://github.com/Thraka/SadConsole/blob/master/SadConsole/ScreenSurface.cs), 
but rather than representing a [CellSurface](https://github.com/Thraka/SadConsole/blob/master/SadConsole/CellSurface.cs) 
it allows direct manipulation of texture pixels used to render it to the screen.

Canvas derives from a [ScreenObject](https://github.com/Thraka/SadConsole/blob/master/SadConsole/ScreenObject.cs). 
It forms a new branch and, since there is no intermediate class 
that offers basic renderable functionalities between the ScreenObject and the ScreenSurface to inherit from,
Canvas copies many properties and methods from the latter (all credit to [Thraka](https://github.com/Thraka)).

What is new are instantiating directly from an image file, pixel drawing and texture import methods.

Available to download as [Nuget](https://www.nuget.org/packages/SadCanvas/).

Alpha version. Work in progress.

## Screenshots:

![Parrot](/screenshot.jpg)