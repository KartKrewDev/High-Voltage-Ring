# UDB

## Properties

---
### Angle2D
Class containing methods related to angles. See [Angle2D](Angle2D.md) for more information.

```js
let rad = UDB.Angle2D.degToRad(46);
```

---
### Data
Class containing methods related to the game data. See [Data](Data.md) for more information.

```js
let hasfireblu = UDB.Data.textureExists('FIREBLU1');
```

---
### GameConfiguration
Class containing methods related to the game configuration. See [GameConfiguration](GameConfiguration.md) for more information.

---
### Line2D
Instantiable class that contains methods related to two-dimensional lines. See [Line2D](Line2D.md) for more information.

```js
let line = new UDB.Line2D([ 32, 64 ], [ 96, 128 ]);
```

---
### Map
Object containing methods related to the map. See [Map](Map.md) for more information.

```js
let sectors = UDB.Map.getSelectedOrHighlightedSectors();
```

---
### QueryOptions
Class containing methods and properties related to querying options from the user at runtime. See [QueryOptions](QueryOptions.md) for more information.

---
### ScriptOptions
Object containing the script options. See [Setting script options](gettingstarted.md#setting-script-options).

---
### UniValue
The `UniValue` class. Is only needed when trying to assign integer values to UDMF fields.

```js
s.fields.user_myintfield = new UDB.UniValue(0, 25);
```

---
### Vector2D
Instantiable class that contains methods related to two-dimensional vectors. See [Vector2D](Vector2D.md) for more information.

```js
let v = new UDB.Vector2D(32, 64);
```

---
### Vector3D
Instantiable class that contains methods related to three-dimensional vectors. See [Vector3D](Vector3D.md) for more information.
## Methods

---
### die(s=null)
Exist the script prematurely with undoing its changes.
#### Parameters
* s: Text to show in the status bar (optional)

---
### exit(s=null)
Exist the script prematurely without undoing its changes.
#### Parameters
* s: Text to show in the status bar (optional)

---
### log(text)
Adds a line to the script log. Also shows the script running dialog.
#### Parameters
* text: Line to add to the script log

---
### setProgress(value)
Set the progress of the script in percent. Value can be between 0 and 100. Also shows the script running dialog.
#### Parameters
* value: Number between 0 and 100

---
### showMessage(message)
Shows a message box with an "OK" button.
#### Parameters
* message: Message to show

---
### showMessageYesNo(message)
Shows a message box with an "Yes" and "No" button.
#### Parameters
* message: Message to show
#### Return value
true if "Yes" was clicked, false if "No" was clicked
