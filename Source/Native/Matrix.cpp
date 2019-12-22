/*
**  BuilderNative Renderer
**  Copyright (c) 2019 Magnus Norddahl
**
**  This software is provided 'as-is', without any express or implied
**  warranty.  In no event will the authors be held liable for any damages
**  arising from the use of this software.
**
**  Permission is granted to anyone to use this software for any purpose,
**  including commercial applications, and to alter it and redistribute it
**  freely, subject to the following restrictions:
**
**  1. The origin of this software must not be misrepresented; you must not
**     claim that you wrote the original software. If you use this software
**     in a product, an acknowledgment in the product documentation would be
**     appreciated but is not required.
**  2. Altered source versions must be plainly marked as such, and must not be
**     misrepresented as being the original software.
**  3. This notice may not be removed or altered from any source distribution.
*/

#include "Precomp.h"
#include <stdexcept>
#include <cstdarg>
#include <algorithm>
#include <cmath>
#include "fasttrig.h"

#ifndef NO_SSE
#include <xmmintrin.h>
#endif

extern "C"
{

#ifdef NO_SSE

void Matrix_Null(float result[4][4])
{
	memset(result, 0, sizeof(float) * 16);
}

#else

void Matrix_Null(float result[4][4])
{
	__m128 zero = _mm_setzero_ps();
	_mm_storeu_ps(result[0], zero);
	_mm_storeu_ps(result[1], zero);
	_mm_storeu_ps(result[2], zero);
	_mm_storeu_ps(result[3], zero);
}

#endif

void Matrix_Identity(float result[4][4])
{
	Matrix_Null(result);
	result[0][0] = 1.0f;
	result[1][1] = 1.0f;
	result[2][2] = 1.0f;
	result[3][3] = 1.0f;
}

void Matrix_Translation(float x, float y, float z, float result[4][4])
{
	Matrix_Null(result);
	result[0][0] = 1.0f;
	result[1][1] = 1.0f;
	result[2][2] = 1.0f;
	result[3][0] = x;
	result[3][1] = y;
	result[3][2] = z;
	result[3][3] = 1.0f;
}

void Matrix_RotationX(float angle, float result[4][4])
{
	float cos = fastcos(angle);
	float sin = fastsin(angle);

	Matrix_Null(result);
	result[0][0] = 1.0f;
	result[1][1] = cos;
	result[1][2] = sin;
	result[2][1] = -sin;
	result[2][2] = cos;
	result[3][3] = 1.0f;
}

void Matrix_RotationY(float angle, float result[4][4])
{
	float cos = fastcos(angle);
	float sin = fastsin(angle);

	Matrix_Null(result);
	result[0][0] = cos;
	result[0][2] = -sin;
	result[1][1] = 1.0f;
	result[2][0] = sin;
	result[2][2] = cos;
	result[3][3] = 1.0f;
}

void Matrix_RotationZ(float angle, float result[4][4])
{
	float cos = fastcos(angle);
	float sin = fastsin(angle);

	Matrix_Null(result);
	result[0][0] = cos;
	result[0][1] = sin;
	result[1][0] = -sin;
	result[1][1] = cos;
	result[2][2] = 1.0f;
	result[3][3] = 1.0f;
}

void Matrix_Scaling(float x, float y, float z, float result[4][4])
{
	Matrix_Null(result);
	result[0][0] = x;
	result[1][1] = y;
	result[2][2] = z;
	result[3][3] = 1.0f;
}

#ifdef NO_SSE

void Matrix_Multiply(const float* left, const float* right, float* result)
{
	result[0 * 4 + 0] = left[0 * 4 + 0] * right[0 * 4 + 0] + left[0 * 4 + 1] * right[1 * 4 + 0] + left[0 * 4 + 2] * right[2 * 4 + 0] + left[0 * 4 + 3] * right[3 * 4 + 0];
	result[0 * 4 + 1] = left[0 * 4 + 0] * right[0 * 4 + 1] + left[0 * 4 + 1] * right[1 * 4 + 1] + left[0 * 4 + 2] * right[2 * 4 + 1] + left[0 * 4 + 3] * right[3 * 4 + 1];
	result[0 * 4 + 2] = left[0 * 4 + 0] * right[0 * 4 + 2] + left[0 * 4 + 1] * right[1 * 4 + 2] + left[0 * 4 + 2] * right[2 * 4 + 2] + left[0 * 4 + 3] * right[3 * 4 + 2];
	result[0 * 4 + 3] = left[0 * 4 + 0] * right[0 * 4 + 3] + left[0 * 4 + 1] * right[1 * 4 + 3] + left[0 * 4 + 2] * right[2 * 4 + 3] + left[0 * 4 + 3] * right[3 * 4 + 3];
	result[1 * 4 + 0] = left[1 * 4 + 0] * right[0 * 4 + 0] + left[1 * 4 + 1] * right[1 * 4 + 0] + left[1 * 4 + 2] * right[2 * 4 + 0] + left[1 * 4 + 3] * right[3 * 4 + 0];
	result[1 * 4 + 1] = left[1 * 4 + 0] * right[0 * 4 + 1] + left[1 * 4 + 1] * right[1 * 4 + 1] + left[1 * 4 + 2] * right[2 * 4 + 1] + left[1 * 4 + 3] * right[3 * 4 + 1];
	result[1 * 4 + 2] = left[1 * 4 + 0] * right[0 * 4 + 2] + left[1 * 4 + 1] * right[1 * 4 + 2] + left[1 * 4 + 2] * right[2 * 4 + 2] + left[1 * 4 + 3] * right[3 * 4 + 2];
	result[1 * 4 + 3] = left[1 * 4 + 0] * right[0 * 4 + 3] + left[1 * 4 + 1] * right[1 * 4 + 3] + left[1 * 4 + 2] * right[2 * 4 + 3] + left[1 * 4 + 3] * right[3 * 4 + 3];
	result[2 * 4 + 0] = left[2 * 4 + 0] * right[0 * 4 + 0] + left[2 * 4 + 1] * right[1 * 4 + 0] + left[2 * 4 + 2] * right[2 * 4 + 0] + left[2 * 4 + 3] * right[3 * 4 + 0];
	result[2 * 4 + 1] = left[2 * 4 + 0] * right[0 * 4 + 1] + left[2 * 4 + 1] * right[1 * 4 + 1] + left[2 * 4 + 2] * right[2 * 4 + 1] + left[2 * 4 + 3] * right[3 * 4 + 1];
	result[2 * 4 + 2] = left[2 * 4 + 0] * right[0 * 4 + 2] + left[2 * 4 + 1] * right[1 * 4 + 2] + left[2 * 4 + 2] * right[2 * 4 + 2] + left[2 * 4 + 3] * right[3 * 4 + 2];
	result[2 * 4 + 3] = left[2 * 4 + 0] * right[0 * 4 + 3] + left[2 * 4 + 1] * right[1 * 4 + 3] + left[2 * 4 + 2] * right[2 * 4 + 3] + left[2 * 4 + 3] * right[3 * 4 + 3];
	result[3 * 4 + 0] = left[3 * 4 + 0] * right[0 * 4 + 0] + left[3 * 4 + 1] * right[1 * 4 + 0] + left[3 * 4 + 2] * right[2 * 4 + 0] + left[3 * 4 + 3] * right[3 * 4 + 0];
	result[3 * 4 + 1] = left[3 * 4 + 0] * right[0 * 4 + 1] + left[3 * 4 + 1] * right[1 * 4 + 1] + left[3 * 4 + 2] * right[2 * 4 + 1] + left[3 * 4 + 3] * right[3 * 4 + 1];
	result[3 * 4 + 2] = left[3 * 4 + 0] * right[0 * 4 + 2] + left[3 * 4 + 1] * right[1 * 4 + 2] + left[3 * 4 + 2] * right[2 * 4 + 2] + left[3 * 4 + 3] * right[3 * 4 + 2];
	result[3 * 4 + 3] = left[3 * 4 + 0] * right[0 * 4 + 3] + left[3 * 4 + 1] * right[1 * 4 + 3] + left[3 * 4 + 2] * right[2 * 4 + 3] + left[3 * 4 + 3] * right[3 * 4 + 3];
}

#else

void Matrix_Multiply(const float a[4][4], const float b[4][4], float result[4][4])
{
	__m128 otherRow0 = _mm_loadu_ps(b[0]);
	__m128 otherRow1 = _mm_loadu_ps(b[1]);
	__m128 otherRow2 = _mm_loadu_ps(b[2]);
	__m128 otherRow3 = _mm_loadu_ps(b[3]);

	__m128 newRow0 = _mm_mul_ps(otherRow0, _mm_set1_ps(a[0][0]));
	newRow0 = _mm_add_ps(newRow0, _mm_mul_ps(otherRow1, _mm_set1_ps(a[0][1])));
	newRow0 = _mm_add_ps(newRow0, _mm_mul_ps(otherRow2, _mm_set1_ps(a[0][2])));
	newRow0 = _mm_add_ps(newRow0, _mm_mul_ps(otherRow3, _mm_set1_ps(a[0][3])));

	__m128 newRow1 = _mm_mul_ps(otherRow0, _mm_set1_ps(a[1][0]));
	newRow1 = _mm_add_ps(newRow1, _mm_mul_ps(otherRow1, _mm_set1_ps(a[1][1])));
	newRow1 = _mm_add_ps(newRow1, _mm_mul_ps(otherRow2, _mm_set1_ps(a[1][2])));
	newRow1 = _mm_add_ps(newRow1, _mm_mul_ps(otherRow3, _mm_set1_ps(a[1][3])));

	__m128 newRow2 = _mm_mul_ps(otherRow0, _mm_set1_ps(a[2][0]));
	newRow2 = _mm_add_ps(newRow2, _mm_mul_ps(otherRow1, _mm_set1_ps(a[2][1])));
	newRow2 = _mm_add_ps(newRow2, _mm_mul_ps(otherRow2, _mm_set1_ps(a[2][2])));
	newRow2 = _mm_add_ps(newRow2, _mm_mul_ps(otherRow3, _mm_set1_ps(a[2][3])));

	__m128 newRow3 = _mm_mul_ps(otherRow0, _mm_set1_ps(a[3][0]));
	newRow3 = _mm_add_ps(newRow3, _mm_mul_ps(otherRow1, _mm_set1_ps(a[3][1])));
	newRow3 = _mm_add_ps(newRow3, _mm_mul_ps(otherRow2, _mm_set1_ps(a[3][2])));
	newRow3 = _mm_add_ps(newRow3, _mm_mul_ps(otherRow3, _mm_set1_ps(a[3][3])));

	_mm_storeu_ps(result[0], newRow0);
	_mm_storeu_ps(result[1], newRow1);
	_mm_storeu_ps(result[2], newRow2);
	_mm_storeu_ps(result[3], newRow3);
}
#endif

}
