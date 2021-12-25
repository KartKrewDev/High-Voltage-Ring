# Angle2D

## Static methods

---
### degToRad(deg)
Converts degrees to radians.
#### Parameters
* deg: Angle in degrees
#### Return value
Angle in radians

---
### doomToReal(doomangle)
Converts a Doom angle (where 0° is east) to a real world angle (where 0° is north).
#### Parameters
* doomangle: Doom angle in degrees
#### Return value
Doom angle in degrees

---
### doomToRealRad(doomangle)
Converts a Doom angle (where 0° is east) to a real world angle (where 0° is north) in radians.
#### Parameters
* doomangle: Doom angle in degrees
#### Return value
Doom angle in radians

---
### getAngle(p1, p2, p3)
Returns the angle between three positions.
#### Parameters
* p1: First position
* p2: Second position
* p3: Third position
#### Return value
Angle in degrees

---
### getAngleRad(p1, p2, p3)
Returns the angle between three positions in radians.
#### Parameters
* p1: First position
* p2: Second position
* p3: Third position
#### Return value
Angle in radians

---
### normalized(angle)
Normalizes an angle in degrees so that it is bigger or equal to 0° and smaller than 360°.
#### Parameters
* angle: Angle in degrees
#### Return value
Normalized angle in degrees

---
### normalizedRad(angle)
Normalizes an angle in radians so that it is bigger or equal to 0 and smaller than 2 Pi.
#### Parameters
* angle: Angle in radians
#### Return value
Normalized angle in radians

---
### radToDeg(rad)
Converts radians to degrees.
#### Parameters
* rad: Angle in radians
#### Return value
Angle in degrees

---
### realToDoom(realangle)
Converts a real world angle (where 0° is north) to a Doom angle (where 0° is east).
#### Parameters
* realangle: Real world angle in degrees
#### Return value
Doom angle in degrees

---
### realToDoomRad(realangle)
Converts a real world  angle (where 0° is north) to a Doom angle (where 0° is east) in radians.
#### Parameters
* realangle: Real world angle in radians
#### Return value
Doom angle in degrees
