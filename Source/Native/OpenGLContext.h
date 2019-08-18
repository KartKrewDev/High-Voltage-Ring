#pragma once

#ifdef WIN32

class OpenGLContext
{
public:
	OpenGLContext(void* window);
	~OpenGLContext();

	void Begin();
	void End();
	void SwapBuffers();

	int GetWidth() const;
	int GetHeight() const;

	explicit operator bool() const { return context != 0; }

private:
	HWND window;
	HDC dc;
	HGLRC context;
	int refcount = 0;
};

class OpenGLCreationHelper
{
public:
	OpenGLCreationHelper(HWND window);
	~OpenGLCreationHelper();

	HGLRC CreateContext(HDC hdc, int major_version, int minor_version, HGLRC share_context = 0);

private:
	HWND window;
	HDC hdc;
	HWND query_window = 0;
	HDC query_dc = 0;
	HGLRC query_context = 0;

	typedef HGLRC(WINAPI* ptr_wglCreateContextAttribsARB)(HDC, HGLRC, const int*);

	typedef BOOL(WINAPI* ptr_wglGetPixelFormatAttribivEXT)(HDC, int, int, UINT, int*, int*);
	typedef BOOL(WINAPI* ptr_wglGetPixelFormatAttribfvEXT)(HDC, int, int, UINT, int*, FLOAT*);
	typedef BOOL(WINAPI* ptr_wglChoosePixelFormatEXT)(HDC, const int*, const FLOAT*, UINT, int*, UINT*);
};

#else

#include <X11/Xlib.h>
#include <X11/Xutil.h>

typedef GLXContext(*ptr_glXCreateContextAttribs)(::Display* dpy, GLXFBConfig config, GLXContext share_list, Bool direct, const int* attrib_list);

class GL_GLXFunctions
{
public:
	typedef XVisualInfo* (GLFUNC* ptr_glXChooseVisual)(::Display* dpy, int screen, int* attrib_list);
	typedef void (GLFUNC* ptr_glXCopyContext)(::Display* dpy, GLXContext src, GLXContext dst, unsigned long mask);
	typedef GLXContext(GLFUNC* ptr_glXCreateContext)(::Display* dpy, XVisualInfo* vis, GLXContext share_list, Bool direct);
	typedef GLXPixmap(GLFUNC* ptr_glXCreateGLXPixmap)(::Display* dpy, XVisualInfo* vis, Pixmap pixmap);
	typedef void (GLFUNC* ptr_glXDestroyContext)(::Display* dpy, GLXContext ctx);
	typedef void (GLFUNC* ptr_glXDestroyGLXPixmap)(::Display* dpy, GLXPixmap pix);
	typedef int (GLFUNC* ptr_glXGetConfig)(::Display* dpy, XVisualInfo* vis, int attrib, int* value);
	typedef GLXContext(GLFUNC* ptr_glXGetCurrentContext)(void);
	typedef GLXDrawable(GLFUNC* ptr_glXGetCurrentDrawable)(void);
	typedef Bool(GLFUNC* ptr_glXIsDirect)(::Display* dpy, GLXContext ctx);
	typedef Bool(GLFUNC* ptr_glXMakeCurrent)(::Display* dpy, GLXDrawable drawable, GLXContext ctx);
	typedef Bool(GLFUNC* ptr_glXQueryExtension)(::Display* dpy, int* error_base, int* event_base);
	typedef Bool(GLFUNC* ptr_glXQueryVersion)(::Display* dpy, int* major, int* minor);
	typedef void (GLFUNC* ptr_glXSwapBuffers)(::Display* dpy, GLXDrawable drawable);
	typedef void (GLFUNC* ptr_glXUseXFont)(Font font, int first, int count, int list_base);
	typedef void (GLFUNC* ptr_glXWaitGL)(void);
	typedef void (GLFUNC* ptr_glXWaitX)(void);
	typedef const char* (GLFUNC* ptr_glXGetClientString)(::Display* dpy, int name);
	typedef const char* (GLFUNC* ptr_glXQueryServerString)(::Display* dpy, int screen, int name);
	typedef const char* (GLFUNC* ptr_glXQueryExtensionsString)(::Display* dpy, int screen);
	typedef ::Display* (GLFUNC* ptr_glXGetCurrentDisplay)(void);
	typedef GLXFBConfig* (GLFUNC* ptr_glXChooseFBConfig)(::Display* dpy, int screen, const int* attrib_list, int* nelements);
	typedef GLXContext(GLFUNC* ptr_glXCreateNewContext)(::Display* dpy, GLXFBConfig config, int render_type, GLXContext share_list, Bool direct);
	typedef GLXPbuffer(GLFUNC* ptr_glXCreatePbuffer)(::Display* dpy, GLXFBConfig config, const int* attrib_list);
	typedef GLXPixmap(GLFUNC* ptr_glXCreatePixmap)(::Display* dpy, GLXFBConfig config, Pixmap pixmap, const int* attrib_list);
	typedef GLXWindow(GLFUNC* ptr_glXCreateWindow)(::Display* dpy, GLXFBConfig config, Window win, const int* attrib_list);
	typedef void (GLFUNC* ptr_glXDestroyPbuffer)(::Display* dpy, GLXPbuffer pbuf);
	typedef void (GLFUNC* ptr_glXDestroyPixmap)(::Display* dpy, GLXPixmap pixmap);
	typedef void (GLFUNC* ptr_glXDestroyWindow)(::Display* dpy, GLXWindow win);
	typedef GLXDrawable(GLFUNC* ptr_glXGetCurrentReadDrawable)(void);
	typedef int (GLFUNC* ptr_glXGetFBConfigAttrib)(::Display* dpy, GLXFBConfig config, int attribute, int* value);
	typedef GLXFBConfig* (GLFUNC* ptr_glXGetFBConfigs)(::Display* dpy, int screen, int* nelements);
	typedef void (GLFUNC* ptr_glXGetSelectedEvent)(::Display* dpy, GLXDrawable draw, unsigned long* event_mask);
	typedef XVisualInfo* (GLFUNC* ptr_glXGetVisualFromFBConfig)(::Display* dpy, GLXFBConfig config);
	typedef Bool(GLFUNC* ptr_glXMakeContextCurrent)(::Display* display, GLXDrawable draw, GLXDrawable read, GLXContext ctx);
	typedef int (GLFUNC* ptr_glXQueryContext)(::Display* dpy, GLXContext ctx, int attribute, int* value);
	typedef void (GLFUNC* ptr_glXQueryDrawable)(::Display* dpy, GLXDrawable draw, int attribute, unsigned int* value);
	typedef void (GLFUNC* ptr_glXSelectEvent)(::Display* dpy, GLXDrawable draw, unsigned long event_mask);

	typedef __GLXextFuncPtr(GLFUNC* ptr_glXGetProcAddress) (const GLubyte*);
	typedef void (*(GLFUNC* ptr_glXGetProcAddressARB)(const GLubyte* procName))(void);

public:
	ptr_glXChooseVisual glXChooseVisual;
	ptr_glXCopyContext glXCopyContext;
	ptr_glXCreateContext glXCreateContext;
	ptr_glXCreateGLXPixmap glXCreateGLXPixmap;
	ptr_glXDestroyContext glXDestroyContext;
	ptr_glXDestroyGLXPixmap glXDestroyGLXPixmap;
	ptr_glXGetConfig glXGetConfig;
	ptr_glXGetCurrentContext glXGetCurrentContext;
	ptr_glXGetCurrentDrawable glXGetCurrentDrawable;
	ptr_glXIsDirect glXIsDirect;
	ptr_glXMakeCurrent glXMakeCurrent;
	ptr_glXQueryExtension glXQueryExtension;
	ptr_glXQueryVersion glXQueryVersion;
	ptr_glXSwapBuffers glXSwapBuffers;
	ptr_glXUseXFont glXUseXFont;
	ptr_glXWaitGL glXWaitGL;
	ptr_glXWaitX glXWaitX;
	ptr_glXGetClientString glXGetClientString;
	ptr_glXQueryServerString glXQueryServerString;
	ptr_glXQueryExtensionsString glXQueryExtensionsString;
	ptr_glXGetCurrentDisplay glXGetCurrentDisplay;
	ptr_glXChooseFBConfig glXChooseFBConfig;
	ptr_glXCreateNewContext glXCreateNewContext;
	ptr_glXCreatePbuffer glXCreatePbuffer;
	ptr_glXCreatePixmap glXCreatePixmap;
	ptr_glXCreateWindow glXCreateWindow;
	ptr_glXDestroyPbuffer glXDestroyPbuffer;
	ptr_glXDestroyPixmap glXDestroyPixmap;
	ptr_glXDestroyWindow glXDestroyWindow;
	ptr_glXGetCurrentReadDrawable glXGetCurrentReadDrawable;
	ptr_glXGetFBConfigAttrib glXGetFBConfigAttrib;
	ptr_glXGetFBConfigs glXGetFBConfigs;
	ptr_glXGetSelectedEvent glXGetSelectedEvent;
	ptr_glXGetVisualFromFBConfig glXGetVisualFromFBConfig;
	ptr_glXMakeContextCurrent glXMakeContextCurrent;
	ptr_glXQueryContext glXQueryContext;
	ptr_glXQueryDrawable glXQueryDrawable;
	ptr_glXSelectEvent glXSelectEvent;
	ptr_glXGetProcAddress glXGetProcAddress;
	ptr_glXGetProcAddressARB glXGetProcAddressARB;
};

class OpenGLContext
{
public:
	OpenGLContext(void* display, void* window);
	~OpenGLContext();

	void Begin();
	void End();
	void SwapBuffers();

	int GetWidth() const;
	int GetHeight() const;

	explicit operator bool() const { return opengl_context != 0; }

private:
	void CreateContext();

	::Display* disp = nullptr;
	::Window window = 0;
	GLXContext opengl_context = 0;

	XVisualInfo* opengl_visual_info = nullptr;
	GLXFBConfig fbconfig;

	GL_GLXFunctions glx;

	ProcAddress* get_proc_address(const char *function_name) const;

	GLXContext create_context_glx_1_3(GLXContext shared_context);
	void create_glx_1_3(::Display* disp);

	int refcount = 0;
};

#endif
