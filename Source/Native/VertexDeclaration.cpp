
#include "Precomp.h"
#include "VertexDeclaration.h"

VertexDeclaration::VertexDeclaration(const VertexElement* elements, int count) : Elements(elements, elements + count)
{
}

VertexDeclaration* VertexDeclaration_New(const VertexElement* elements, int count)
{
	return new VertexDeclaration(elements, count);
}

void VertexDeclaration_Delete(VertexDeclaration* decl)
{
	delete decl;
}
