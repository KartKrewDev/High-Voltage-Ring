`#name Apply textures to selected surfaces`;

`#description Applies LAVA1 to the selected floors/ceilings, and FIREBLU1 to the selected upper/middle/lower sidedefs. Mostly useful in visual mode`;

`#version 3`;

// Get all selected or highlighted sectors and sidedefs
let elements = Map.getSelectedOrHighlightedSectors().concat(Map.getSidedefsFromSelectedOrHighlightedLinedefs());

// Since the array might contain both selected sectors and highlighted sidedefs (or vice versa)
// we have to filter the array, so that we really only work on the correct map elements, i.e.
// either the single highlighted one, or all selected ones
elements.filter(e => {
    if( elements.length == 1 ||
        (e instanceof Sector && (e.floorSelected || e.ceilingSelected)) ||
        (e instanceof Sidedef && (e.upperSelected || e.middleSelected || e.lowerSelected))
    ) return true;
    return false;
}).forEach(e => {
    // Check for each sector and sidedef which part is selected/highlighted and
    // apply the textures accordingly
    if(e instanceof Sector)
    {
        if(e.floorSelected || e.floorHighlighted)
            e.floorTexture = 'LAVA1';

        if(e.ceilingSelected || e.ceilingHighlighted)
            e.ceilingTexture = 'LAVA1';
    }
    else if(e instanceof Sidedef)
    {
        if(e.lowerSelected || e.lowerHighlighted)
            e.lowerTexture = 'FIREBLU1';

        if(e.middleSelected || e.middleHighlighted)
            e.middleTexture = 'FIREBLU1';

        if(e.upperSelected || e.upperHighlighted)
            e.upperTexture = 'FIREBLU1';
    }
});