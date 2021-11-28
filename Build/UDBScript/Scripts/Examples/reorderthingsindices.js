`#name Reorder Things Indices`;

`#description Reorderts the thing inddices of the selected things, so that the thing indices are ascending in the order the things were selected.`;

let things = Map.getSelectedThings();

if(things.length < 2)
    die('You have to select at least 2 things.');

let sorted = [...things].sort((a, b) => a.index - b.index);

let copies = things.map(t => {
    let nt = Map.createThing([ 0, 0 ]);
    t.copyPropertiesTo(nt);
    return nt;
});

for(let i=0; i < sorted.length; i++)
    copies[i].copyPropertiesTo(sorted[i]);


// Create new things and copy the properties of the original thing to them
//things.forEach(t => {
//    let nt = Map.createThing(t.position);
//    t.copyPropertiesTo(nt);
//});

// Delete the old things. Have to do it in an extra loop sice it'd just fill
// up the old thing indexes
//things.reverse().forEach(t => t.delete());
copies.reverse().forEach(t => t.delete());