# Sector

## Properties

---
### brightness
The `Sector`'s brightness.

---
### ceilingHeight
Ceiling height of the `Sector`.

---
<span style="float:right;font-weight:normal;font-size:66%">Version: 3</span>
### ceilingHighlighted
If the `Sector`'s ceiling is highlighted or not. Will always return `true` in classic modes if the `Sector` is highlighted. Read-only.

---
<span style="float:right;font-weight:normal;font-size:66%">Version: 3</span>
### ceilingSelected
If the `Sector`'s ceiling is selected or not. Will always return `true` in classic modes if the `Sector` is selected. Read-only.

---
### ceilingSlopeOffset
The ceiling's slope offset.

---
### ceilingTexture
Ceiling texture of the `Sector`.

---
### fields
UDMF fields. It's an object with the fields as properties.

```js
s.fields.comment = 'This is a comment';
s.fields['comment'] = 'This is a comment'; // Also  works
s.fields.xscalefloor = 2.0;
t.fields.score = 100;
```
It is also possible to define new fields:

```js
s.fields.user_myboolfield = true;
```
There are some restrictions, though:

* it only works for fields that are not in the base UDMF standard, since those are handled directly in the respective class
* it does not work for flags. While they are technically also UDMF fields, they are handled in the `flags` field of the respective class (where applicable)
* JavaScript does not distinguish between integer and floating point numbers, it only has floating point numbers (of double precision). For fields where UDB knows that they are integers this it not a problem, since it'll automatically convert the floating point numbers to integers (dropping the fractional part). However, if you need to specify an integer value for an unknown or custom field you have to work around this limitation, using the `UniValue` class:

```js
s.fields.user_myintfield = new UDB.UniValue(0, 25); // Sets the 'user_myintfield' field to an integer value of 25
```
To remove a field you have to assign `null` to it:

```js
s.fields.user_myintfield = null;
```

---
### flags
`Sector` flags. It's an object with the flags as properties. Only available in UDMF.


```js
s.flags['noattack'] = true; // Monsters in this sector don't attack
s.flags.noattack = true; // Also works
```

---
### floorHeight
Floor height of the `Sector`.

---
<span style="float:right;font-weight:normal;font-size:66%">Version: 3</span>
### floorHighlighted
If the `Sector`'s floor is highlighted or not. Will always return `true` in classic modes if the `Sector` is highlighted. Read-only.

---
<span style="float:right;font-weight:normal;font-size:66%">Version: 3</span>
### floorSelected
If the `Sector`'s floor is selected or not. Will always return `true` in classic modes if the `Sector` is selected. Read-only.

---
### floorSlopeOffset
The floor's slope offset.

---
### floorTexture
Floor texture of the `Sector`.

---
### index
The `Sector`'s index. Read-only.

---
### marked
If the `Sector` is marked or not. It is used to mark map elements that were created or changed (for example after drawing new geometry).

---
### selected
If the `Sector` is selected or not.

---
### special
The `Sector`'s special type.

---
### tag
The `Sector`'s tag.
## Methods

---
### addTag(tag)
Adds a tag to the `Sector`. UDMF only. Supported game configurations only.
#### Parameters
* tag: Tag to add
#### Return value
`true` when the tag was added, `false` when the tag already exists

---
### clearFlags()
Clears all flags.

---
### copyPropertiesTo(s)
Copies the properties from this `Sector` to another.
#### Parameters
* s: the `Sector` to copy the properties to

---
### delete()
Deletes the `Sector` and its `Sidedef`s.

---
### getCeilingSlope()
Gets the ceiling's slope vector.
#### Return value
The ceiling's slope normal as a `Vector3D`

---
### getFloorSlope()
Gets the floor's slope vector.
#### Return value
The floor's slope normal as a `Vector3D`

---
### getSidedefs()
Returns an `Array` of all `Sidedef`s of the `Sector`.
#### Return value
`Array` of the `Sector`'s `Sidedef`s

---
### getTags()
Returns an `Array` of the `Sector`'s tags. UDMF only. Supported game configurations only.
#### Return value
`Array` of tags

---
### getTriangles()
Gets an array of `Vector2D` arrays, representing the vertices of the triangulated sector. Note that for sectors with islands some triangles may not always have their points on existing vertices.
#### Return value
Array of `Vector2D` arrays

---
### intersect(p)
Checks if the given point is in this `Sector` or not. The given point can be a `Vector2D` or an `Array` of two numbers.

```js
if(s.intersect(new Vector2D(32, 64)))
	UDB.showMessage('Point is in the sector!');

if(s.intersect([ 32, 64 ]))
	UDB.showMessage('Point is in the sector!');
```
#### Parameters
* p: Point to test
#### Return value
`true` if the point is in the `Sector`, `false` if it isn't

---
### join(other)
Joins this `Sector` with another `Sector`. Lines shared between the sectors will not be removed.
#### Parameters
* other: Sector to join with

---
### removeTag(tag)
Removes a tag from the `Sector`. UDMF only. Supported game configurations only.
#### Parameters
* tag: Tag to remove
#### Return value
`true` when the tag was removed successfully, `false` when the tag did not exist

---
### setCeilingSlope(normal)
Sets the ceiling's slope vector. The vector has to be normalized.
#### Parameters
* normal: The new slope vector as `Vector3D`

---
### setFloorSlope(normal)
Sets the floor's slope vector. The vector has to be normalized.
#### Parameters
* normal: The new slope vector as `Vector3D`
