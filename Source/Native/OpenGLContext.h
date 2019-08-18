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
	void* context = nullptr;
};

#endif
