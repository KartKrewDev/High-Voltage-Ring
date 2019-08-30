
all:
	msbuild -p:Configuration=Release BuilderMono.sln
	g++ -O2 --shared -o Build/BuilderNative.so -fPIC -I Source/Native Source/Native/*.cpp Source/Native/gl_load/*.c
