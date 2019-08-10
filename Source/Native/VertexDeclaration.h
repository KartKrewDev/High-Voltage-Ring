#pragma once

enum class DeclarationType : int32_t { Float2, Float3, Color };
enum class DeclarationUsage : int32_t { Position, Color, TextureCoordinate, Normal };

struct VertexElement
{
	int16_t Stream;
	int16_t Offset;
	DeclarationType Type;
	DeclarationUsage Usage;
};

class VertexDeclaration
{
public:
	VertexDeclaration(const VertexElement* elements, int count);

	std::vector<VertexElement> Elements;
};
