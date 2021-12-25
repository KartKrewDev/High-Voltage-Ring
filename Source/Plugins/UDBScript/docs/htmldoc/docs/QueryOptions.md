# QueryOptions

The `QueryOptions` class is used to query the user for their input. It effectively works the same as specifying script options in the script's metadata, except that the `QueryOptions` class works at run-time.

Example:

```js
let qo = new QueryOptions();
qo.addOption('length', 'Length of the sides', 0, 128);
qo.addOption('numsides', 'Number of sides', 0, 5);
qo.addOption('direction', 'Direction to go', 11, 1, { 1: 'Up', 2: 'Down' }); // Enumeration
qo.query();

showMessage('You want ' + qo.options.numsides + ' sides with a length of ' + qo.options.length);
```
## Constructors

---
### QueryOptions()
Initializes a new `QueryOptions` object.
## Properties

---
### options
Object containing all the added options as properties.
## Methods

---
### addOption(name, description, type, defaultvalue)
Adds a parameter to query
#### Parameters
* name: Name of the variable that the queried value is stored in
* description: Textual description of the parameter
* type: UniversalType value of the parameter
* defaultvalue: Default value of the parameter

---
### addOption(name, description, type, defaultvalue, enumvalues)
Adds a parameter to query
#### Parameters
* name: Name of the variable that the queried value is stored in
* description: Textual description of the parameter
* type: UniversalType value of the parameter
* defaultvalue: Default value of the parameter

---
### clear()
Removes all parameters

---
### query()
Queries all parameters. Options a window where the user can enter values for the options added through `addOption()`.
#### Return value
True if OK was pressed, otherwise false
