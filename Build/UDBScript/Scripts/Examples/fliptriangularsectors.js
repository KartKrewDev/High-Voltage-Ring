`#name Flip Triangular Sectors`;

`#description Flips two selected triangular sectors, so that the connecting linedef is between the vertices that previously didn't share a linedef`;

let sectors = Map.getSelectedSectors();
let vertices = new Set();
let sharedline = null;

if(sectors.length != 2)
    die('You have to select exactly 2 sectors');

// Make sure we have triangular sectors selected, and collect all vertices
sectors.forEach(s => {
    let sidedefs = s.getSidedefs();

    if(sidedefs.length != 3)
        die(s + ' does not have exactly 3 sides');
    
    sidedefs.forEach(sd => {
        // Does this sidedef belong to the linedef that's shared between the sectors?
        if(sd.other != null && sectors.includes(sd.sector) && sectors.includes(sd.other.sector))
            sharedline = sd.line;

        // Add the vertices to the set
        vertices.add(sd.line.start)
        vertices.add(sd.line.end)
    });
});

// Delete the vertices of the shared line from the set
vertices.delete(sharedline.start);
vertices.delete(sharedline.end);

// There should be exactly 2 vertices
if(vertices.size != 2)
    die('Expected to find 2 vertices to draw the new line between, but got ' + vertices.size);

// Merge the sectors...
Map.mergeSectors(sectors);

// ... and draw the new line
Map.drawLines(Array.from(vertices, v => v.position));