class Pen
{
    constructor(pos = new Vector2D(0, 0))
    {
        this.angle = Math.PI / 2;
        this.snaptogrid = false;
        this.vertices = [];
        this.curpos = new Vector2D(pos);
    }

    moveTo(pos)
    {
        this.curpos = new Vector2D(pos);

        return this;
    }

    moveForward(distance)
    {
        this.curpos = new Vector2D(
            this.curpos.x + Math.cos(this.angle) * distance,
            this.curpos.y + Math.sin(this.angle) * distance
        );

        return this;
    }

    turnRightRadians(radians = Math.PI / 2)
    {
        this.angle -= radians;

        while(this.angle < 0)
            this.angle += Math.PI * 2;

        return this;
    }

    turnLeftRadians(radians = Math.PI / 2)
    {
        this.angle += radians;

        while(this.angle > Math.PI * 2)
            this.angle -= Math.PI * 2;

        return this;
    }

    turnRight(degrees = 90.0)
    {
        this.angle -= degrees * Math.PI / 180.0;

        while(this.angle < 0)
            this.angle += Math.PI * 2;

        return this;
    }

    turnLeft(degrees = 90.0)
    {
        this.angle += degrees * Math.PI / 180.0;

        while(this.angle > Math.PI * 2)
            this.angle -= Math.PI * 2;

        return this;
    }

    setAngleRadians(radians)
    {
        this.angle = Math.PI / 2 - radians;

        return this;
    }

    setAngle(degrees)
    {
        this.angle = (90 - degrees) * Math.PI / 180.0;

        return this;
    }

    drawVertex()
    {
        this.vertices.push(this.curpos);

        return this;
    }

    finishDrawing(close = false)
    {
        if(close)
            this.vertices.push(this.vertices[0]);

        var result = Map.drawLines(this.vertices);

        this.vertices = [];

        return result;
    }
}