# Vector3D

## Constructors

---
### Vector3D(v)
Creates a new `Vector3D` from a point.

```js
let v = new UDB.Vector3D([ 32, 64, 128 ]);
```
#### Parameters
* v: The vector to create the `Vector3D` from

---
### Vector3D(x, y, z)
Creates a new `Vector3D` from x and y coordinates

```js
let v = new UDB.Vector3D(32, 64, 128);
```
#### Parameters
* x: The x coordinate
* y: The y coordinate
* z: The z coordinate
## Static methods

---
### crossProduct(a, b)
Returns the cross product of two `Vector3D`s.
#### Parameters
* a: First `Vector3D`
* b: Second `Vector3D`
#### Return value
Cross product of the two vectors as `Vector3D`

---
### dotProduct(a, b)
Returns the dot product of two `Vector3D`s.
#### Parameters
* a: First `Vector3D`
* b: Second `Vector3D`
#### Return value
The dot product of the two vectors

---
### fromAngleXY(angle)
Creates a `Vector3D` from an angle in radians,
#### Parameters
* angle: Angle on the x/y axes in degrees
#### Return value
Vector as `Vector3D`

---
### fromAngleXYRad(angle)
Creates a `Vector3D` from an angle in radians
#### Parameters
* angle: Angle on the x/y axes in radians
#### Return value
Vector as `Vector3D`

---
### fromAngleXYZ(anglexy, anglez)
Creates a `Vector3D` from two angles in degrees
#### Parameters
* anglexy: Angle on the x/y axes in radians
* anglez: Angle on the z axis in radians
#### Return value
Vector as `Vector3D`

---
### fromAngleXYZRad(anglexy, anglez)
Creates a `Vector3D` from two angles in radians
#### Parameters
* anglexy: Angle on the x/y axes in radians
* anglez: Angle on the z axis in radians
#### Return value
Vector as `Vector3D`

---
### reflect(v, m)
Reflects a `Vector3D` over a mirror `Vector3D`.
#### Parameters
* v: `Vector3D` to reflect
* m: Mirror `Vector3D`
#### Return value
The reflected vector as `Vector3D`

---
### reversed(v)
Returns a reversed `Vector3D`.
#### Parameters
* v: `Vector3D` to reverse
#### Return value
The reversed vector as `Vector3D`
## Properties

---
### x
The `x` value of the vector.

---
### y
The `y` value of the vector.

---
### z
The `z` value of the vector.
## Methods

---
### getAngleXY()
Returns the angle of the `Vector3D` in degrees.
#### Return value
The angle of the `Vector3D` in degrees

---
### getAngleXYRad()
Returns the x/y angle of the `Vector3D` in radians.
#### Return value
The x/y angle of the `Vector3D` in radians

---
### getAngleZ()
Returns the z angle of the `Vector3D` in degrees.
#### Return value
The z angle of the `Vector3D` in degrees

---
### getAngleZRad()
Returns the z angle of the `Vector3D` in radians.
#### Return value
The z angle of the `Vector3D` in radians

---
### getLength()
Returns the length of the `Vector3D`.
#### Return value
The length of the `Vector3D`

---
### getLengthSq()
Returns the square length of the `Vector3D`.
#### Return value
The square length of the `Vector3D`

---
### getNormal()
Returns the normal of the `Vector3D`.
#### Return value
The normal as `Vector3D`

---
### getScaled(scale)
Return the scaled `Vector3D`.
#### Parameters
* scale: Scale, where 1.0 is unscaled
#### Return value
The scaled `Vector3D`

---
### isFinite()
Checks if the `Vector3D` is finite or not.
#### Return value
`true` if `Vector3D` is finite, otherwise `false`

---
### isNormalized()
Checks if the `Vector3D` is normalized or not.
#### Return value
`true` if `Vector3D` is normalized, otherwise `false`
