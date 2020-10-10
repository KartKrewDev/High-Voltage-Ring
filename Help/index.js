
var listview = document.getElementById("listview");
var content = document.getElementById("content");

var contents = [
	{ name: "Introduction", src: "introduction.html" },
	{ name: "Terminology", src: "terminology.html" },
	{ name: "User Interface", src: "userinterface.html", items: [
		{ name: "About the User Interface", src: "userinterface.html" },
		{ name: "Main Window", src: "w_mainwindow.html" },
		{ name: "Custom Fields Editor", src: "w_customfields.html" },
		{ name: "Errors and Warnings Window", src: "w_errorsandwarnings.html" },
		{ name: "Game Configurations Window", src: "w_gameconfigurations.html" },
		{ name: "Grid Setup Window", src: "w_gridsetup.html" },
		{ name: "Image Browser Window", src: "w_imagesbrowser.html" },
		{ name: "Linedef Properties Window", src: "w_linedefedit.html" },
		{ name: "Map Options Window", src: "w_mapoptions.html" },
		{ name: "Open Map Window", src: "w_openmapoptions.html" },
		{ name: "Preferences Window", src: "w_preferences.html" },
		{ name: "Resource Options Window", src: "w_resourceoptions.html" },
		{ name: "Script Editor Window", src: "w_scripteditor.html" },
		{ name: "Sector Properties Window", src: "w_sectoredit.html" },
		{ name: "Texture Sets Window", src: "w_textureset.html" },
		{ name: "Thing Properties Window", src: "w_thingedit.html" },
		{ name: "Things Filters Window", src: "w_thingsfilters.html" },
		{ name: "Vertex Properties Window", src: "w_vertexedit.html" }
	]},
	{ name: "Editing Modes", src: "editingmodes.html", items: [
		{ name: "About Editing Modes", src: "editingmodes.html" },
		{ name: "Curve Linedefs Mode", src: "e_curvelinedefs.html" },
		{ name: "Draw Geometry Mode", src: "e_drawgeometry.html" },
		{ name: "Edit Selection Mode", src: "e_editselection.html" },
		{ name: "Find & Replace Mode", src: "e_findreplace.html" },
		{ name: "Linedefs Mode", src: "e_linedefs.html" },
		{ name: "Make Sectors Mode", src: "e_makesectors.html" },
		{ name: "Map Analysis Mode", src: "e_mapanalysis.html" },
		{ name: "Sectors Mode", src: "e_sectors.html" },
		{ name: "Things Mode", src: "e_things.html" },
		{ name: "Vertices Mode", src: "e_vertices.html" },
		{ name: "Visual Mode", src: "e_visual.html" }
	]},
	{ name: "Configurations", src: "configurations.html", items: [
		{ name: "About Configurations", src: "configurations.html" },
		{ name: "Configuration Syntax", src: "configstructure.html" },
		{ name: "Compiler Configurations", src: "compilerconfigs.html", items: [
			{ name: "About Compiler Configurations", src: "compilerconfigs.html" }
		]},
		{ name: "Game Configurations", src: "gameconfigs.html", items: [
			{ name: "About Game Configurations", src: "gameconfigs.html" },
			{ name: "Basic Settings", src: "gc_basicsettings.html" },
			{ name: "Map Format Settings", src: "gc_formatsettings.html" },
			{ name: "Resource Settings", src: "gc_resourcesettings.html" },
			{ name: "Sectors Settings", src: "gc_sectorsettings.html" },
			{ name: "Linedefs Settings", src: "gc_linedefsettings.html" },
			{ name: "Sidedef Settings", src: "gc_sidedefsettings.html" },
			{ name: "Things Settings", src: "gc_thingsettings.html" },
			{ name: "Action Argument Settings", src: "gc_argumentsettings.html" }
		]},
		{ name: "Scripting Configurations", src: "scriptingconfigs.html", items: [
			{ name: "About Scripting Configurations", src: "scriptingconfigs.html" }
		]},
		{ name: "DECORATE keys", src: "gc_decoratekeys.html" }
	]},
	{ name: "Ultimate Doom Builder manual", src: "gz_introduction.html", items: [
		{ name: "Features", src: "gzdb/features/features.html", items: [
			{ name: "General Interface", src: "gzdb/features/features.html#general", items: [
				{ name: "Rendering toolbar", src: "gzdb/features/general/rendering_toolbar.html" },
				{ name: "Multiple engines per game configuration", src: "gzdb/features/general/multi_engines.html" }
			]},
			{ name: "Scripting", src: "gzdb/features/features.html#scripting", items: [
				{ name: "Enhanced scripting workflow", src: "gzdb/features/scripting/acs.html" },
				{ name: "Code Snippets", src: "gzdb/features/scripting/snippets.html" }
			]},
			{ name: "New features in Classic and Visual modes", src: "gzdb/features/features.html#allmodes", items: [
				{ name: "Enhanced Tag support", src: "gzdb/features/all_modes/tag_support.html" },
				{ name: "Event lines", src: "gzdb/features/all_modes/event_lines.html" },
				{ name: "Testing map from view", src: "gzdb/features/all_modes/test_from_view.html" },
				{ name: "Synchronizing selection", src: "gzdb/features/all_modes/synch_selection.html" },
				{ name: "Synchronizing camera position", src: "gzdb/features/all_modes/synch_camera.html" },
				{ name: "Color Picker plugin", src: "gzdb/features/all_modes/colorpicker.html" },
				{ name: "Tag Explorer plugin", src: "gzdb/features/all_modes/tagexplorer.html" },
				{ name: "Image Browser", src: "gzdb/features/all_modes/texture_browser.html" }
			]},
			{ name: "New features in Classic modes", src: "gzdb/features/features.html#classic", items: [
				{ name: "Draw Settings Panel", src: "gzdb/features/classic_modes/drawsettingspanel.html" },
				{ name: "Custom linedef colors", src: "gzdb/features/classic_modes/linedef_color_presets.html" },
				{ name: "Enhanced rectangular selection", src: "gzdb/features/classic_modes/selection.html" }
			]},
			{ name: "New features in Sectors mode", src: "gzdb/features/features.html#sectors" },
			{ name: "New features in Linedefs mode", src: "gzdb/features/features.html#linedefs" },
			{ name: "Draw Rectangle mode", src: "gzdb/features/classic_modes/mode_drawrect.html" },
			{ name: "Draw Ellipse mode", src: "gzdb/features/classic_modes/mode_drawellipse.html" },
			{ name: "Draw Curve mode", src: "gzdb/features/classic_modes/mode_drawcurve.html" },
			{ name: "Draw Grid mode", src: "gzdb/features/classic_modes/mode_drawgrid.html" },
			{ name: "Sound Propagation Mode", src: "gzdb/features/classic_modes/mode_soundpropagation.html" },
			{ name: "Sound Environment Mode", src: "gzdb/features/classic_modes/mode_soundenvironment.html" },
			{ name: "3D Floor Mode", src: "gzdb/features/classic_modes/mode_3dfloor.html" },
			{ name: "Draw Slope Mode/Slope Mode", src: "gzdb/features/classic_modes/mode_slopes.html" },
			{ name: "Randomize mode", src: "gzdb/features/all_modes/jitter.html" },
			{ name: "Bridge mode", src: "gzdb/features/classic_modes/mode_drawbridge.html" },
			{ name: "Import Terrain mode", src: "gzdb/features/classic_modes/mode_importterrain.html" },
			{ name: "Snap geometry mode", src: "gzdb/features/classic_modes/mode_snapelements.html" },
			{ name: "New features in Things mode", src: "gzdb/features/features.html#things", items: [
				{ name: "Assigning multiple thing types at once", src: "gzdb/features/things_mode/multiple_thing_types.html" },
				{ name: "\"Point Thing to Cursor\" action", src: "gzdb/features/things_mode/pointthing.html" }
			]},
			{ name: "New features in Vertices mode", src: "gzdb/features/features.html#vertices" },
			{ name: "GZDB Visual mode", src: "gzdb/features/features.html#visual", items: [
				{ name: "Visual vertices (UDMF)", src: "gzdb/features/visual_mode/visual_verts.html" },
				{ name: "Using Auto Align Textures actions on floors and ceilings (UDMF)", src: "gzdb/features/visual_mode/autoalignfloors.html" },
				{ name: "\"Fit Texture\" actions", src: "gzdb/features/visual_mode/texturefit.html" },
				{ name: "\"Auto-align Textures to Selection\" actions", src: "gzdb/features/visual_mode/autoalign_to_selection.html" }
			]},
			{ name: "New features in Map Analysis mode", src: "gzdb/features/features.html#mapanalysis" },
			{ name: "New features in Find and Replace mode", src: "gzdb/features/features.html#findreplace" },
			{ name: "Custom Fields", src: "gzdb/features/features.html#fields", items: [
				{ name: "New custom field types", src: "gzdb/features/custom_fields/newfieldtypes.html" }
			]},
			{ name: "(G)ZDoom features support", src: "gzdb/features/features.html#formats" }
		]},
		{ name: "(G)ZDoom text lumps support", src: "gzdb/text_lumps.html" },
		{ name: "List of deprecated plugins", src: "gzdb/deprecated_plugins.html" },
		{ name: "Compiling GZDoom Builder", src: "gzdb/compilingtheeditor.html" }
	]},
	{ name: "Frequently asked questions", src: "gzdb/faq.html" },
	{ name: "Command Line Parameters", src: "commandlineparams.html" },
	{ name: "System Requirements", src: "systemrequirements.html" }
];

var listviewItems = [];

function addClass(element, name) {
	var classes = getClasses(element);
	var index = classes.indexOf(name);
	if (index == -1) {
		classes.push(name);
		setClasses(element, classes);
	}
}

function removeClass(element, name) {
	var classes = getClasses(element);
	var index = classes.indexOf(name);
	if (index != -1) {
		classes.splice(index, 1);
		setClasses(element, classes);
	}
}

function getClasses(element) {
	var classes = element.getAttribute("class");
	if (classes == null) return [];
	return classes.split(" ");
}

function setClasses(element, classes) {
	element.setAttribute("class", classes.join(" "));
}

function itemSelected(element, item) {
	listviewItems.forEach(x => removeClass(x, "selected"));
	addClass(element, "selected");
	content.setAttribute("src", item.src);
}

function addItem(parent, item, level) {
	var element = document.createElement("div");
	var text = document.createElement("div");
	addClass(element, "listviewitem");
	addClass(text, "listviewitem-text");
	text.style.paddingLeft = (level * 20) + "px";
	text.innerText = item.name;
	text.addEventListener('click', event => itemSelected(element, item) );
	element.appendChild(text);
	listviewItems.push(element);
	parent.appendChild(element);
	if (item.items != undefined) {
		item.items.forEach(childitem => addItem(element, childitem, level + 1));
	}
}

contents.forEach(item => addItem(listview, item, 1));
