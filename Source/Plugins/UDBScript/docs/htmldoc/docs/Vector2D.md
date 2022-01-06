# Vector2D

## Constructors

---
### Vector2D(v)
Creates a new `Vector2D` from a point.

```js
let v = new UDB.Vector2D([ 32, 64 ]);
```
#### Parameters
* v: The vector to create the `Vector2D` from

---
### Vector2D(x, y)
Creates a new `Vector2D` from x and y coordinates

```js
let v = new UDB.Vector2D(32, 64);
```
#### Parameters
* x: The x coordinate
* y: The y coordinate
## Static methods

---
### crossProduct(a, b)
Returns the cross product of two `Vector2D`s.
#### Parameters
* a: First `Vector2D`
* b: Second `Vector2D`
#### Return value
Cross product of the two vectors as `Vector2D`

---
### dotProduct(a, b)
Returns the dot product of two `Vector2D`s.
#### Parameters
* a: First `Vector2D`
* b: Second `Vector2D`
#### Return value
The dot product of the two vectors

---
### fromAngle(angle)
Creates a `Vector2D` from an angle in degrees,
#### Parameters
* angle: Angle in degrees
#### Return value
Vector as `Vector2D`

---
### fromAngleRad(angle)
Creates a `Vector2D` from an angle in radians,
#### Parameters
* angle: Angle in radians
#### Return value
Vector as `Vector2D`

---
### getAngle(a, b)
Returns the angle between two `Vector2D`s in degrees.
#### Parameters
* a: First `Vector2D`
* b: Second `Vector2D`
#### Return value
Angle in degrees

---
### getAngleRad(a, b)
Returns the angle between two `Vector2D`s in radians
#### Parameters
* a: First `Vector2D`
* b: Second `Vector2D`
#### Return value
Angle in radians

---
### getDistance(a, b)
Returns the distance between two `Vector2D`s.
#### Parameters
* a: First `Vector2D`
* b: Second `Vector2D`
#### Return value
The distance

---
### getDistanceSq(a, b)
Returns the square distance between two `Vector2D`s.
#### Parameters
* a: First `Vector2D`
* b: Second `Vector2D`
#### Return value
The squared distance

---
### reflect(v, m)
Reflects a `Vector2D` over a mirror `Vector2D`.
#### Parameters
* v: `Vector2D` to reflect
* m: Mirror `Vector2D`
#### Return value
The reflected vector as `Vector2D`

---
### reversed(v)
Returns a reversed `Vector2D`.
#### Parameters
* v: `Vector2D` to reverse
#### Return value
The reversed vector as `Vector2D`
## Properties

---
### x
The `x` value of the vector.

---
### y
The `y` value of the vector.
## Methods

---
### getAngle()
Returns the angle of the `Vector2D` in degree.
#### Return value
The angle of the `Vector2D` in degree

---
### getAngleRad()
Returns the angle of the `Vector2D` in radians.
#### Return value
The angle of the `Vector2D` in radians

---
### getInverseTransformed(invoffsetx, invoffsety, invscalex, invscaley)
Returns the inverse transformed vector as `Vector2D`.
#### Parameters
* invoffsetx: X offset
* invoffsety: Y offset
* invscalex: X scale
* invscaley: Y scale
#### Return value
The inverse transformed vector as `Vector2D`

---
### getLength()
Returns the length of the `Vector2D`.
#### Return value
The length of the `Vector2D`

---
### getLengthSq()
Returns the square length of the `Vector2D`.
#### Return value
The square length of the `Vector2D`

---
### getNormal()
Returns the normal of the `Vector2D`.
#### Return value
The normal as `Vector2D`

---
### getPerpendicular()
Returns the perpendicular to the `Vector2D`.
#### Return value
The perpendicular as `Vector2D`

---
### getRotated(theta)
Returns the rotated vector as `Vector2D`.
#### Parameters
* theta: Angle in degree to rotate by
#### Return value
The rotated `Vector2D`

---
### getRotatedRad(theta)
Returns the rotated vector as `Vector2D`.
#### Parameters
* theta: Angle in radians to rotate by
#### Return value
The rotated `Vector2D`

---
### getSign()
Returns a `Vector2D` with the sign of all components.
#### Return value
A `Vector2D` with the sign of all components

---
### getTransformed(offsetx, offsety, scalex, scaley)
Returns the transformed vector as `Vector2D`.
#### Parameters
* offsetx: X offset
* offsety: Y offset
* scalex: X scale
* scaley: Y scale
#### Return value
The transformed vector as `Vector2D`

---
### isFinite()
Checks if the `Vector2D` is finite or not.
#### Return value
`true` if `Vector2D` is finite, otherwise `false`
