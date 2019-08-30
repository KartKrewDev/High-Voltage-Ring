#pragma once

#include <memory>

class IOpenGLContext
{
public:
	virtual ~IOpenGLContext() = default;
	
	virtual void MakeCurrent() = 0;
	virtual void ClearCurrent() = 0;
	virtual void SwapBuffers() = 0;
	
	virtual int GetWidth() const = 0;
	virtual int GetHeight() const = 0;
	
	static std::unique_ptr<IOpenGLContext> Create(void* disp, void* window);
};
