`#name Calculate sector area`;

`#description Calculates the area of the selected sectors.`

let sectors = Map.getSelectedSectors();

if(sectors.length == 0)
    die('You need to select at least one sector.');

let area = 0;

sectors.forEach(s => {
    s.getTriangles().forEach(t => {
        area += 0.5 * Math.abs(t[0].x * (t[1].y - t[2].y) + t[1].x * (t[2].y - t[0].y) + t[2].x * (t[0].y - t[1].y));
    });
});

showMessage('The area of the selected sectors is ' + area + ' mu².');