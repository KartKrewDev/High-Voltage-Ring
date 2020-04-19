//------------------------------------------------------------------------
//  Visplane Overflow Library
//------------------------------------------------------------------------
//
//  Copyright (C) 1993-1996 Id Software, Inc.
//  Copyright (C) 2005 Simon Howard
//  Copyright (C) 2012 Andrew Apted
//
//  This program is free software; you can redistribute it and/or
//  modify it under the terms of the GNU General Public License
//  as published by the Free Software Foundation; either version 2
//  of the License, or (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//------------------------------------------------------------------------

#ifndef __VPO_LOCAL_H__
#define __VPO_LOCAL_H__

#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#pragma warning(disable: 4244) // warning C4244: '=': conversion from '__int64' to 'int', possible loss of data
#pragma warning(disable: 4146) // warning C4146: unary minus operator applied to unsigned type, result still unsigned
#pragma warning(disable: 4267) // warning C4267: '=': conversion from 'size_t' to 'int', possible loss of data
#pragma warning(disable: 4996) // warning C4996: 'strdup': The POSIX name for this item is deprecated. Instead, use the ISO C and C++ conformant name: _strdup.
#endif

#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdarg.h>
#include <string.h>
#include <math.h>

#include "sys_type.h"
#include "sys_macro.h"
#include "sys_endian.h"

namespace vpo
{

#include "doomtype.h"
#include "doomdef.h"
#include "doomdata.h"

#include "m_bbox.h"
#include "m_fixed.h"

#include "w_file.h"
#include "w_wad.h"

#include "tables.h"
#include "r_defs.h"
#include "r_state.h"

#include "r_main.h"
#include "r_bsp.h"
#include "r_plane.h"


//------------------------------------------------------------

#define SHORT(x)  LE_S16(x)
#define LONG(x)   LE_S32(x)

const char * P_SetupLevel (const char *lumpname, bool *is_hexen );
void P_FreeLevelData (void);

void I_Error (const char *error, ...);

sector_t * X_SectorForPoint(fixed_t x, fixed_t y);

// exceptions thrown on overflows
class overflow_exception { };

//
// ClipWallSegment
// Clips the given range of columns
// and includes it in the new clip list.
//
typedef	struct
{
    int	first;
    int last;

} cliprange_t;

// andrewj: increased limit to 128 for Visplane Explorer
// #define MAXSOLIDSEGS		32
#define MAXSOLIDSEGS  128

typedef struct
{
	// Should be "IWAD" or "PWAD".
	char		identification[4];
	int			numlumps;
	int			infotableofs;

} PACKEDATTR wadinfo_t;


typedef struct
{
	int			filepos;
	int			size;
	char		name[8];

} PACKEDATTR filelump_t;

struct Context
{
	void M_ClearBox(fixed_t* box);
	void M_AddToBox(fixed_t* box, fixed_t x, fixed_t y);

	void LevelError(const char* msg, ...);
	void P_LoadVertexes(int lump);
	sector_t* GetSectorAtNullAddress();
	void P_LoadSegs(int lump);
	void P_LoadSubsectors(int lump);
	void ValidateSubsectors();
	void P_LoadSectors(int lump);
	bool isChildValid(unsigned short child);
	void P_LoadNodes(int lump);
	void LineDef_CommonSetup(line_t* ld);
	void P_LoadLineDefs(int lump);
	void P_LoadLineDefs_Hexen(int lump);
	void P_LoadSideDefs(int lump);
	void P_GroupLines();
	int HasManualDoor(const sector_t* sec);
	void CalcDoorAltHeight(sector_t* sec);
	void P_DetectDoorSectors();
	const char* P_SetupLevel(const char* lumpname, bool* is_hexen);
	void P_FreeLevelData();

	void R_ClearDrawSegs();
	void R_ClipSolidWallSegment(int first, int last);
	void R_ClipPassWallSegment(int first, int last);
	void R_ClearClipSegs();
	void R_AddLine(seg_t* line);
	boolean R_CheckBBox(fixed_t* bspcoord);
	void R_Subsector(int num);
	void R_RenderBSPNode(int bspnum);

	void R_AddPointToBox(int x, int y, fixed_t* box);
	int R_PointOnSide(fixed_t x, fixed_t y, node_t* node);
	int R_PointOnSegSide(fixed_t x, fixed_t y, seg_t* line);
	angle_t R_PointToAngle(fixed_t x, fixed_t y);
	angle_t R_PointToAngle2(fixed_t	x1, fixed_t	y1, fixed_t	x2, fixed_t	y2);
	fixed_t R_PointToDist(fixed_t x, fixed_t y);
	fixed_t R_ScaleFromGlobalAngle(angle_t visangle);
	void R_InitBuffer(int width, int height);
	void R_InitTextureMapping();
	void R_SetViewSize(int blocks, int detail);
	void R_Init();
	subsector_t* R_PointInSubsector(fixed_t x, fixed_t y);
	void R_SetupFrame(fixed_t x, fixed_t y, fixed_t z, angle_t angle);
	void R_RenderView(fixed_t x, fixed_t y, fixed_t z, angle_t angle);

	void R_ClearPlanes();
	visplane_t* R_FindPlane(fixed_t height, int picnum, int lightlevel);
	visplane_t* R_CheckPlane(visplane_t* pl, int  start, int  stop);

	void R_RenderSegLoop();
	void R_StoreWallRange(int start, int stop);

	void ClearError();
	void SetError(const char* msg, ...);

	void I_Error(const char* error, ...);
	int ClosestLine_CastingHoriz(fixed_t x, fixed_t y, int* side);
	sector_t* X_SectorForPoint(fixed_t x, fixed_t y);

	wad_file_t* W_OpenFile(const char* path);
	void W_CloseFile(wad_file_t* wad);
	size_t W_Read(wad_file_t* wad, unsigned int offset, void* buffer, size_t buffer_len);

	int CheckMapHeader(filelump_t* lumps, int num_after);
	bool W_AddFile(const char* filename);
	void W_RemoveFile();
	int W_NumLumps();
	int W_CheckNumForName(const char* name);
	int W_LumpLength(int lumpnum);
	void W_ReadLump(int lump, void* dest);
	byte* W_LoadLump(int lumpnum);
	void W_FreeLump(byte* data);
	void W_BeginRead();
	void W_EndRead();

	// the error message returned from P_SetupLevel()
	char level_error_msg[1024] = {};
	bool level_is_hexen = {};

	vertex_t* vertexes = {};
	int numvertexes = {};
	seg_t* segs = {};
	int numsegs = {};
	sector_t* sectors = {};
	int numsectors = {};
	subsector_t* subsectors = {};
	int numsubsectors = {};
	node_t* nodes = {};
	int numnodes = {};
	line_t* lines = {};
	int numlines = {};
	side_t* sides = {};
	int numsides = {};

	fixed_t  Map_bbox[4] = {};

	seg_t* curline = {};
	side_t* sidedef = {};
	line_t* linedef = {};
	sector_t* frontsector = {};
	sector_t* backsector = {};

	drawseg_t drawsegs[MAXDRAWSEGS + 10] = {};
	drawseg_t* ds_p = {};

	int total_drawsegs = {};

	// newend is one past the last valid seg
	cliprange_t	solidsegs[MAXSOLIDSEGS + 8] = {};
	cliprange_t* newend = {};

	int max_solidsegs = {};


	int			viewangleoffset = {};

	// increment every time a check is made
	int			validcount = 1;


	int			centerx = {};
	int			centery = {};

	fixed_t			centerxfrac = {};
	fixed_t			centeryfrac = {};
	fixed_t			projection = {};

	// just for profiling purposes
	int			framecount = {};

	int			sscount = {};
	int			linecount = {};
	int			loopcount = {};

	fixed_t			viewx = {};
	fixed_t			viewy = {};
	fixed_t			viewz = {};

	angle_t			viewangle = {};

	fixed_t			viewcos = {};
	fixed_t			viewsin = {};


	//
	// precalculated math tables
	//
	angle_t			clipangle = {};

	// The viewangletox[viewangle + FINEANGLES/4] lookup
	// maps the visible view angles to screen X coordinates,
	// flattening the arc to a flat projection plane.
	// There will be many angles mapped to the same X. 
	int			viewangletox[FINEANGLES / 2] = {};

	// The xtoviewangleangle[] table maps a screen pixel
	// to the lowest viewangle that maps back to x ranges
	// from clipangle to -clipangle.
	angle_t			xtoviewangle[SCREENWIDTH + 1] = {};


	// UNUSED.
	// The finetangentgent[angle+FINEANGLES/4] table
	// holds the fixed_t tangent values for view angles,
	// ranging from INT_MIN to 0 to INT_MAX.
	// fixed_t		finetangent[FINEANGLES/2];

	// fixed_t		finesine[5*FINEANGLES/4];
	const fixed_t* finecosine = &finesine[FINEANGLES / 4];


	// bumped light from gun blasts
	int			extralight = {};


	// from R_DRAW
	int		viewwidth = {};
	int		scaledviewwidth = {};
	int		viewheight = {};
	int		viewwindowx = {};
	int		viewwindowy = {};


	// from R_THINGS
	fixed_t  pspritescale = {};
	fixed_t  pspriteiscale = {};

	short  screenheightarray[SCREENWIDTH] = {};
	short  negonearray[SCREENWIDTH] = {};

	//
	// opening
	//

	// Here comes the obnoxious "visplane".
	visplane_t visplanes[MAXVISPLANES + 10] = {};
	visplane_t* lastvisplane = {};
	visplane_t* floorplane = {};
	visplane_t* ceilingplane = {};

	int total_visplanes = {};

	short openings[MAXOPENINGS + 400] = {};
	short* lastopening = {};

	int total_openings = {};


	//
	// Clip values are the solid pixel bounding the range.
	//  floorclip starts out SCREENHEIGHT
	//  ceilingclip starts out -1
	//
	short floorclip[SCREENWIDTH] = {};
	short ceilingclip[SCREENWIDTH] = {};

	//
	// spanstart holds the start of a plane span
	// initialized to 0 at start
	//
	int spanstart[SCREENHEIGHT] = {};
	int spanstop[SCREENHEIGHT] = {};

	//
	// texture mapping
	//
	fixed_t			planeheight = {};

	fixed_t			yslope[SCREENHEIGHT] = {};
	fixed_t			distscale[SCREENWIDTH] = {};
	fixed_t			basexscale = {};
	fixed_t			baseyscale = {};

	fixed_t			cachedheight[SCREENHEIGHT] = {};
	fixed_t			cacheddistance[SCREENHEIGHT] = {};
	fixed_t			cachedxstep[SCREENHEIGHT] = {};
	fixed_t			cachedystep[SCREENHEIGHT] = {};

	// OPTIMIZE: closed two sided lines as single sided

	// True if any of the segs textures might be visible.
	boolean		segtextured = {};

	// False if the back side is the same plane.
	boolean		markfloor = {};
	boolean		markceiling = {};

	boolean		maskedtexture = {};
	int		toptexture = {};
	int		bottomtexture = {};
	int		midtexture = {};


	angle_t		rw_normalangle = {};
	// angle to line origin
	int		rw_angle1 = {};

	//
	// regular wall
	//
	int		rw_x = {};
	int		rw_stopx = {};
	angle_t		rw_centerangle = {};
	fixed_t		rw_offset = {};
	fixed_t		rw_distance = {};
	fixed_t		rw_scale = {};
	fixed_t		rw_scalestep = {};
	fixed_t		rw_midtexturemid = {};
	fixed_t		rw_toptexturemid = {};
	fixed_t		rw_bottomtexturemid = {};

	int		worldtop = {};
	int		worldbottom = {};
	int		worldhigh = {};
	int		worldlow = {};

	fixed_t		pixhigh = {};
	fixed_t		pixlow = {};
	fixed_t		pixhighstep = {};
	fixed_t		pixlowstep = {};

	fixed_t		topfrac = {};
	fixed_t		topstep = {};

	fixed_t		bottomfrac = {};
	fixed_t		bottomstep = {};

	short* maskedtexturecol = {};

	// R_DRAW bits
	int              dc_x = {};
	int              dc_yl = {};
	int              dc_yh = {};
	fixed_t          dc_iscale = {};
	fixed_t          dc_texturemid = {};

	char error_buffer[1024] = {};
	char mapname_buffer[16] = {};

	// cache for the sector lookup
	int last_x = {};
	int last_y = {};
	sector_t* last_sector = {};

	// Location of each lump on disk.
	lumpinfo_t* lumpinfo = {};
	int numlumps = 0;
	char* wad_filename = {};
	wad_file_t* current_file = {};
};

} // namespace vpo


#endif  /* __VPO_LOCAL_H__ */

//--- editor settings ---
// vi:ts=4:sw=4:noexpandtab
