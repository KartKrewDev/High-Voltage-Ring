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
    }

    moveForward(distance)
    {
        this.curpos = new Vector2D(
            this.curpos.x + Math.cos(this.angle) * distance,
            this.curpos.y + Math.sin(this.angle) * distance
        );
    }

    turnRightRadians(radians = Math.PI / 2)
    {
        this.angle -= radians;

        while(this.angle < 0)
            this.angle += Math.PI * 2;        
    }

    turnLeftRadians(radians = Math.PI / 2)
    {
        this.angle += radians;

        while(this.angle > Math.PI * 2)
            this.angle -= Math.PI * 2;   
    }

    turnRight(degrees = 90.0)
    {
        this.angle -= degrees * Math.PI / 180.0;

        while(this.angle < 0)
            this.angle += Math.PI * 2;
    }

    turnLeft(degrees = 90.0)
    {
        this.angle += degrees * Math.PI / 180.0;

        while(this.angle > Math.PI * 2)
            this.angle -= Math.PI * 2;        
    }

    setAngleRadians(radians)
    {
        this.angle = Math.PI / 2 - radians;        
    }

    setAngle(degrees)
    {
        this.angle = (90 - degrees) * Math.PI / 180.0;
    }

    drawVertex()
    {
        this.vertices.push(this.curpos);
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