
all: builder native

builder:
	msbuild -p:Configuration=Debug BuilderMono.sln

native:
	g++ -O2 --shared -g3 -o Build/libBuilderNative.so -fPIC -I Source/Native Source/Native/*.cpp Source/Native/gl_load/*.c -lX11 -ldl
