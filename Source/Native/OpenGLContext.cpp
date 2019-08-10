
#include "Precomp.h"
#include "OpenGLContext.h"
#include <CommCtrl.h>

#ifdef WIN32

#define WGL_CONTEXT_MAJOR_VERSION_ARB           0x2091
#define WGL_CONTEXT_MINOR_VERSION_ARB           0x2092
#define WGL_CONTEXT_LAYER_PLANE_ARB             0x2093
#define WGL_CONTEXT_FLAGS_ARB                   0x2094
#define WGL_CONTEXT_DEBUG_BIT_ARB               0x0001
#define WGL_CONTEXT_FORWARD_COMPATIBLE_BIT_ARB  0x0002
#define ERROR_INVALID_VERSION_ARB               0x2095

#define WGL_NUMBER_CL_PIXEL_FORMATS             0x2000
#define WGL_DRAW_TO_WINDOW                   0x2001
#define WGL_DRAW_TO_BITMAP                   0x2002
#define WGL_ACCELERATION                     0x2003
#define WGL_NEED_PALETTE                     0x2004
#define WGL_NEED_SYSTEM_PALETTE              0x2005
#define WGL_SWAP_LAYER_BUFFERS               0x2006
#define WGL_SWAP_METHOD                      0x2007
#define WGL_NUMBER_OVERLAYS                  0x2008
#define WGL_NUMBER_UNDERLAYS                 0x2009
#define WGL_TRANSPARENT                      0x200A
#define WGL_TRANSPARENT_VALUE                0x200B
#define WGL_SHARE_DEPTH                      0x200C
#define WGL_SHARE_STENCIL                    0x200D
#define WGL_SHARE_ACCUM                      0x200E
#define WGL_SUPPORT_GDI                      0x200F
#define WGL_SUPPORT_OPENGL                   0x2010
#define WGL_DOUBLE_BUFFER                    0x2011
#define WGL_STEREO                           0x2012
#define WGL_PIXEL_TYPE                       0x2013
#define WGL_COLOR_BITS                       0x2014
#define WGL_RED_BITS                         0x2015
#define WGL_RED_SHIFT                        0x2016
#define WGL_GREEN_BITS                       0x2017
#define WGL_GREEN_SHIFT                      0x2018
#define WGL_BLUE_BITS                        0x2019
#define WGL_BLUE_SHIFT                       0x201A
#define WGL_ALPHA_BITS                       0x201B
#define WGL_ALPHA_SHIFT                      0x201C
#define WGL_ACCUM_BITS                       0x201D
#define WGL_ACCUM_RED_BITS                   0x201E
#define WGL_ACCUM_GREEN_BITS                 0x201F
#define WGL_ACCUM_BLUE_BITS                  0x2020
#define WGL_ACCUM_ALPHA_BITS                 0x2021
#define WGL_DEPTH_BITS                       0x2022
#define WGL_STENCIL_BITS                     0x2023
#define WGL_AUX_BUFFERS                      0x2024
#define WGL_NO_ACCELERATION                  0x2025
#define WGL_GENERIC_ACCELERATION             0x2026
#define WGL_FULL_ACCELERATION                0x2027
#define WGL_SWAP_EXCHANGE                    0x2028
#define WGL_SWAP_COPY                        0x2029
#define WGL_SWAP_UNDEFINED                   0x202A
#define WGL_TYPE_RGBA                        0x202B
#define WGL_TYPE_COLORINDEX                  0x202C
#define WGL_SAMPLE_BUFFERS                   0x2041
#define WGL_SAMPLES                          0x2042

#endif

/////////////////////////////////////////////////////////////////////////////

class OpenGLLoadFunctions
{
public:
	OpenGLLoadFunctions() { ogl_LoadFunctions(); }
};

/////////////////////////////////////////////////////////////////////////////

OpenGLContext::OpenGLContext(HWND window) : window(window)
{
	dc = GetDC(window);
	OpenGLCreationHelper helper(window);
	context = helper.CreateContext(dc, 3, 2);
	if (context)
	{
		Begin();
		static OpenGLLoadFunctions loadFunctions;
		End();
	}
}

OpenGLContext::~OpenGLContext()
{
	if (context)
		wglDeleteContext(context);
	if (dc)
		ReleaseDC(window, dc);
}

void OpenGLContext::Begin()
{
	wglMakeCurrent(dc, context);
}

void OpenGLContext::End()
{
	wglMakeCurrent(0, 0);
}

void OpenGLContext::SwapBuffers()
{
	Begin();
	::SwapBuffers(dc);
	End();
}

/////////////////////////////////////////////////////////////////////////////

OpenGLCreationHelper::OpenGLCreationHelper(HWND window) : window(window)
{
	WINDOWINFO window_info;
	memset(&window_info, 0, sizeof(WINDOWINFO));
	window_info.cbSize = sizeof(WINDOWINFO);
	GetWindowInfo(window, &window_info);

	query_window = CreateWindowEx(
		0,
		WC_STATIC,
		TEXT(""),
		WS_CHILD,
		window_info.rcWindow.left,
		window_info.rcWindow.top,
		window_info.rcWindow.right - window_info.rcWindow.left,
		window_info.rcWindow.bottom - window_info.rcWindow.top,
		window, 0, GetModuleHandle(0), 0);
	if (query_window == 0)
		return;

	query_dc = GetDC(query_window);
	if (query_dc == 0)
	{
		DestroyWindow(query_window);
		query_window = 0;
		return;
	}

	PIXELFORMATDESCRIPTOR pfd;
	memset(&pfd, 0, sizeof(PIXELFORMATDESCRIPTOR));
	pfd.nSize = sizeof(PIXELFORMATDESCRIPTOR);
	pfd.nVersion = 1;
	pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL;
	pfd.iPixelType = PFD_TYPE_RGBA;
	pfd.cColorBits = 24;

	int pixelformat = ChoosePixelFormat(query_dc, &pfd);
	SetPixelFormat(query_dc, pixelformat, &pfd);

	query_context = wglCreateContext(query_dc);
	if (query_context == 0)
	{
		DeleteDC(query_dc);
		DestroyWindow(query_window);
		query_dc = 0;
		query_window = 0;
	}
}

OpenGLCreationHelper::~OpenGLCreationHelper()
{
	wglDeleteContext(query_context);
	DeleteDC(query_dc);
	DestroyWindow(query_window);
}

HGLRC OpenGLCreationHelper::CreateContext(HDC hdc, int major_version, int minor_version, HGLRC share_context)
{
	if (query_context == 0)
		return 0;

	PIXELFORMATDESCRIPTOR pfd;
	memset(&pfd, 0, sizeof(PIXELFORMATDESCRIPTOR));
	pfd.nSize = sizeof(PIXELFORMATDESCRIPTOR);
	pfd.nVersion = 1;
	pfd.dwFlags = PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL;
	pfd.iPixelType = PFD_TYPE_RGBA;
	pfd.dwFlags |= PFD_DOUBLEBUFFER;
	pfd.cColorBits = 24;
	pfd.cRedBits = 4;
	pfd.cGreenBits = 4;
	pfd.cBlueBits = 4;
	pfd.cAlphaBits = 4;
	pfd.cDepthBits = 24;
	pfd.cStencilBits = 0;
	int pixelformat = ChoosePixelFormat(hdc, &pfd);
	SetPixelFormat(hdc, pixelformat, &pfd);

	wglMakeCurrent(query_dc, query_context);

	ptr_wglCreateContextAttribsARB wglCreateContextAttribsARB = (ptr_wglCreateContextAttribsARB)wglGetProcAddress("wglCreateContextAttribsARB");

	HGLRC opengl3_context = 0;
	if (wglCreateContextAttribsARB)
	{
		std::vector<int> int_attributes;

		int_attributes.push_back(WGL_CONTEXT_MAJOR_VERSION_ARB);
		int_attributes.push_back(major_version);
		int_attributes.push_back(WGL_CONTEXT_MINOR_VERSION_ARB);
		int_attributes.push_back(minor_version);

		int_attributes.push_back(0x2094); // WGL_CONTEXT_FLAGS_ARB
		int_attributes.push_back(0x2); // WGL_CONTEXT_FORWARD_COMPATIBLE_BIT_ARB

		int_attributes.push_back(0x9126); // WGL_CONTEXT_PROFILE_MASK_ARB
		int_attributes.push_back(0x1); // WGL_CONTEXT_CORE_PROFILE_BIT_ARB

		int_attributes.push_back(0);

		opengl3_context = wglCreateContextAttribsARB(hdc, share_context, int_attributes.data());
	}

	wglMakeCurrent(0, 0);

	return opengl3_context;
}
