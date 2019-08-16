#pragma once

class RawMouse
{
public:
	RawMouse(HWND ownerWindow);
	~RawMouse();

	float GetX();
	float GetY();

private:
	LRESULT OnMessage(INT message, WPARAM wparam, LPARAM lparam);
	static LRESULT CALLBACK WindowProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

	HWND handle = 0;
	int x = 0;
	int y = 0;

	friend class RawMouseWindowClass;
};
