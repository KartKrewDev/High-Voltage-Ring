
#include "RenderDevice.h"

RenderDevice::RenderDevice()
{
}

RenderDevice* RenderDevice_New()
{
	return new RenderDevice();
}

void RenderDevice_Delete(RenderDevice* device)
{
	delete device;
}
