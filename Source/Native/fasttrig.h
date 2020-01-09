/*
** fastsin.h
** a table/linear interpolation-based sine function that is both
** precise and fast enough for most purposes.
**
**---------------------------------------------------------------------------
** Copyright 2015 Christoph Oelckers
** All rights reserved.
**
** Redistribution and use in source and binary forms, with or without
** modification, are permitted provided that the following conditions
** are met:
**
** 1. Redistributions of source code must retain the above copyright
**    notice, this list of conditions and the following disclaimer.
** 2. Redistributions in binary form must reproduce the above copyright
**    notice, this list of conditions and the following disclaimer in the
**    documentation and/or other materials provided with the distribution.
** 3. The name of the author may not be used to endorse or promote products
**    derived from this software without specific prior written permission.
**
** THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
** IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
** OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
** IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
** INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
** NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
** DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
** THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
** (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
** THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**---------------------------------------------------------------------------
**
*/

#pragma once

#ifndef M_PI
#define M_PI 3.14159265358979323846
#endif

#ifdef WIN32
#define FORCEINLINE __forceinline
#else
#define FORCEINLINE
#endif

// This uses a sine table with linear interpolation
// For in-game calculations this is precise enough
// and this code is more than 10x faster than the
// Cephes sin and cos function.

struct FFastTrig
{
	static const int TBLPERIOD = 8192;
	static const int BITSHIFT = 19;
	static const int REMAINDER = (1 << BITSHIFT) - 1;
	float sinetable[2049];

	FORCEINLINE double sinq1(uint32_t bangle)
	{
		unsigned int index = bangle >> BITSHIFT;

		if ((bangle &= (REMAINDER)) == 0)	// This is to avoid precision problems at 180 degrees
		{
			return double(sinetable[index]);
		}
		else
		{
			return (double(sinetable[index]) * (REMAINDER - bangle) + double(sinetable[index + 1]) * bangle) * (1. / REMAINDER);
		}
	}

public:
	FFastTrig()
	{
		const double pimul = M_PI * 2 / TBLPERIOD;

		for (int i = 0; i < 2049; i++)
		{
			sinetable[i] = (float)std::sin(i * pimul);
		}
	}

	double sin(uint32_t bangle)
	{
		switch (bangle & 0xc0000000)
		{
		default:
			return sinq1(bangle);

		case 0x40000000:
			return sinq1(0x80000000 - bangle);

		case 0x80000000:
			return -sinq1(bangle - 0x80000000);

		case 0xc0000000:
			return -sinq1(0 - bangle);
		}
	}

	double cos(uint32_t bangle)
	{
		switch (bangle & 0xc0000000)
		{
		default:
			return sinq1(0x40000000 - bangle);

		case 0x40000000:
			return -sinq1(bangle - 0x40000000);

		case 0x80000000:
			return -sinq1(0xc0000000 - bangle);

		case 0xc0000000:
			return sinq1(bangle - 0xc0000000);
		}
	}
};

static FFastTrig fasttrig;

// This must use xs_Float to guarantee proper integer wraparound.
#define DEG2BAM(f)		((uint32_t)std::round((f) * (0x40000000/90.)))
#define RAD2BAM(f)		((uint32_t)std::round((f) * (0x80000000/3.14159265358979323846)))

inline double fastcosdeg(double v)
{
	return fasttrig.cos(DEG2BAM(v));
}

inline double fastsindeg(double v)
{
	return fasttrig.sin(DEG2BAM(v));
}

inline double fastcos(double v)
{
	return fasttrig.cos(RAD2BAM(v));
}

inline double fastsin(double v)
{
	return fasttrig.sin(RAD2BAM(v));
}
