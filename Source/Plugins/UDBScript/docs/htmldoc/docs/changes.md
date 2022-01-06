# Changes
This site lists all changes between different API version of UDBScript

## Verstion 4

- Moved all classes, object, and methods into the `UDB` namespace (everything has to be prefixed wiht `UDB.`)
- Added methods to report progress for long running scripts and script log output. See [Communicating with the user](gettingstartet.md#communicating-with-the-user) for more information

## Version 3

- Exported the classes `Linedef`, `Sector`, `Sidedef`, `Thing`, and `Vertex`, so that they can be used with `instanceof`
- `Map` class
    - the `getSidedefsFromSelectedLinedefs()` method now correctly only returns the `Sidedef`s of selected `Linedef`s in visual mode (and not also the highlighted one)
    - added a new `getSidedefsFromSelectedOrHighlightedLinedefs()` method as the equivalent to the other `getSelectedOrHighlighted*()` methods
- `Sector` class
    - added new `floorSelected`, `ceilingSelected`, `floorHighlighted`, and `ceilingHighlighted` properties. Those are mostly useful in visual mode, since they always return true when the `Sector` is selected or highlighted in the classic modes. The properties are read-only
- `Sidedef` class
    - added new `upperSelected`, `middleSelected`, `lowerSelected`, `upperHighlighted`, `middleHighlighted`, and `lowerHighlighted` properties. Those are mostly useful in visual mode, since they always return true when the parent `Linedef` is selected or highlighted in the classic modes. The properties are read-only

## Version 2

- `Pen` built-in library
    - the methods of the `Pen` class now return the instance of the Pen class to allow method chaining