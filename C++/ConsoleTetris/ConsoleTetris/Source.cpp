#include <iostream>
#include <vector>
#include <windows.h>
#include <thread>
using namespace std;

//screen dimensions
int nScreenWidth = 80;
int nScreenHeight = 30;

//game dimensions
int nLevelWidth = 12;
int nLevelHeight = 18;
unsigned char* pfield = nullptr;
unsigned char* nextPfield = nullptr;

//tetris pieces
wstring tetrisPieces[7];

int Rotate(int x, int y, int r) {
	int pi = 0;
	switch (r % 4) {
	case 0:
		pi = y * 4 + x;
		break;
	case 1:
		pi = 12 + y - (x * 4);
		break;
	case 2:
		pi = 15 - (y * 4) - x;
		break;
	case 3:
		pi = 3 - y + (x * 4);
		break;
	}
	return pi;
}


bool doesPieceFit(int piece, int r, int pieceX, int pieceY) {
	for (int x = 0; x < 4; x++) {
		for (int y = 0; y < 4; y++) {
			
			int pi = Rotate(x, y, r); // index into piece
			int fi = (pieceY + y) * nLevelWidth + (pieceX + x); // index into Level

			if (pieceX + x >= 0 && pieceX + x < nLevelWidth) {
				if (pieceY + y >= 0 && pieceY + y < nLevelHeight) {
					if (tetrisPieces[piece][pi] == L'X' && pfield[fi] != 0) {
						return false; // fail on first hit
					}
				}
			}

		}
	}
	
	return true;
}


int main() {
	//screen buffer creation
	wchar_t* screen = new wchar_t[nScreenWidth * nScreenHeight];
	for (int i = 0; i < nScreenWidth * nScreenHeight; i++)
	{
		screen[i] = L' ';
	}
	HANDLE hConsole = CreateConsoleScreenBuffer(GENERIC_READ | GENERIC_WRITE, 0, NULL, CONSOLE_TEXTMODE_BUFFER, NULL);
	SetConsoleActiveScreenBuffer(hConsole);
	//resizes window to the given screen height and width
	COORD buffer_size = { nScreenWidth, nScreenHeight };
	SetConsoleScreenBufferSize(hConsole, buffer_size);
	DWORD dwBytesWritten = 0;
	//prevents resizing of the window to  avoid any visual bugs
	HWND consoleWindow = GetConsoleWindow();
	SetWindowLong(consoleWindow, GWL_STYLE, GetWindowLong(consoleWindow, GWL_STYLE) & ~WS_MAXIMIZEBOX & ~WS_SIZEBOX);

	//scales the console
	CONSOLE_FONT_INFOEX cfi;
	cfi.cbSize = sizeof(cfi);
	cfi.nFont = 0;
	cfi.dwFontSize.X = 30;                   // Width of each character in the font
	cfi.dwFontSize.Y = 30;                  // Height
	cfi.FontFamily = FF_DONTCARE;
	cfi.FontWeight = FW_NORMAL;
	wcscpy_s(cfi.FaceName, L"Consolas"); // Choose your font
	SetCurrentConsoleFontEx(hConsole, FALSE, &cfi);


	//4x4 tetris pieces
	tetrisPieces[0].append(L"..X.");
	tetrisPieces[0].append(L"..X.");
	tetrisPieces[0].append(L"..X.");
	tetrisPieces[0].append(L"..X.");

	tetrisPieces[1].append(L"..X.");
	tetrisPieces[1].append(L".XX.");
	tetrisPieces[1].append(L"..X.");
	tetrisPieces[1].append(L"....");

	tetrisPieces[2].append(L"....");
	tetrisPieces[2].append(L".XX.");
	tetrisPieces[2].append(L".XX.");
	tetrisPieces[2].append(L"....");

	tetrisPieces[3].append(L"..X.");
	tetrisPieces[3].append(L".XX.");
	tetrisPieces[3].append(L".X..");
	tetrisPieces[3].append(L"....");

	tetrisPieces[4].append(L".X..");
	tetrisPieces[4].append(L".XX.");
	tetrisPieces[4].append(L"..X.");
	tetrisPieces[4].append(L"....");

	tetrisPieces[5].append(L".X..");
	tetrisPieces[5].append(L".X..");
	tetrisPieces[5].append(L".XX.");
	tetrisPieces[5].append(L"....");

	tetrisPieces[6].append(L"..X.");
	tetrisPieces[6].append(L"..X.");
	tetrisPieces[6].append(L".XX.");
	tetrisPieces[6].append(L"....");

	//assign Level values game screen
	pfield = new unsigned char[nLevelWidth * nLevelHeight]; // Create play field buffer
	for (int x = 0; x < nLevelWidth; x++) // Board Boundary
		for (int y = 0; y < nLevelHeight; y++)
			pfield[y * nLevelWidth + x] = (x == 0 || x == nLevelWidth - 1 || y == nLevelHeight - 1) ? 9 : 0;

	nextPfield = new unsigned char[8 * 8]; // Create play field buffer
	for (int x = 0; x < 8; x++) // Board Boundary
		for (int y = 0; y < 8; y++)
			nextPfield[y * 8 + x] = (x == 0 || x == 7 || y == 7 || y == 0) ? 2 : 0;

	// -------------------GAME LOOP----------------------------- 

	bool gameOver = false;;
	int currentPiece = rand() % 7;
	int nextPiece = rand() % 7;
	int currentPieceRotation = 0;	
	bool rotating = false;

	int pieceCounter = 0;

	int pieceX = nLevelWidth / 2; // spawns the pieceX in the middle of the level
	int pieceY = 0; // top of the level
	
	bool keyPressed[4];
	
	int speed = 20;
	int ticksPassed = 0;
	bool moveDown = false;

	vector<int> lines;

	int score = 0;


	//while not dead
	while (!gameOver) {

		//game timing
		this_thread::sleep_for(50ms);
		ticksPassed++;
		moveDown = (ticksPassed == speed); // true every second

		//input detection
		for (int i = 0; i < 4; i++) {							// \ RIGHT \ LEFT \ DOWN \ UP/ROTATE
			keyPressed[i] = (0x8000 & GetAsyncKeyState((unsigned char)("\x27\x25\x28\x26"[i]))) != 0;
		}

		//applying input
		if ( keyPressed[0] && doesPieceFit(currentPiece, currentPieceRotation, pieceX + 1, pieceY) ) {
				pieceX += 1;
		}
		if (keyPressed[1] && doesPieceFit(currentPiece, currentPieceRotation, pieceX - 1, pieceY)) {
				pieceX -= 1;
		}
		if (keyPressed[2] && doesPieceFit(currentPiece, currentPieceRotation, pieceX, pieceY + 1)) {
				pieceY += 1;
		}

		if (keyPressed[3]) {
			if (!rotating && doesPieceFit(currentPiece, currentPieceRotation + 1, pieceX, pieceY)) {
				currentPieceRotation += 1;
				rotating = true;
			}
		}
		else {
			rotating = false;
		}

		// LOGIC

		if (moveDown) {
			if (doesPieceFit(currentPiece, currentPieceRotation, pieceX, pieceY + 1)) {
				pieceY++;
			}
			else {
				//look at piece
				for (int x = 0; x < 4; x++) {
					for (int y = 0; y < 4; y++) {
						if (tetrisPieces[currentPiece][Rotate(x, y, currentPieceRotation)] != L'.')
							pfield[(pieceY + y) * nLevelWidth + (pieceX + x)] = currentPiece + 1;
					}
				}

				pieceCounter++;
				if (pieceCounter % 10 == 0) {
					if (speed >= 10) {
						speed--;
					}
				}

				//check for complete lines
				for (int y = 0; y < 4; y++) {
					if (pieceY + y < nLevelHeight - 1) {

						bool line = true;
						for (int x = 1; x < nLevelWidth - 1; x++) {
							line &= (pfield[(pieceY + y) * nLevelWidth + x]) != 0;
						}

						if (line) {
							for (int x = 1; x < nLevelWidth - 1; x++) {
								pfield[(pieceY + y) * nLevelWidth + x] = 8;
							}
						lines.push_back(pieceY + y); // stores the line that  the line is on
						}
					}
				}

				score += 25;
				if (!lines.empty()) { // gives more points if more lines are done at once
					score += (1 << lines.size()) * 100;
				}

				//change piece and reset position
				pieceX = nLevelWidth / 2;
				pieceY = 0;
				currentPieceRotation = 0;
				currentPiece = nextPiece;
				nextPiece = rand() % 7;

				//gameover trigger
				if (!doesPieceFit(currentPiece, currentPieceRotation, pieceX, pieceY)) {
					gameOver = true;
				}
			}
			ticksPassed = 0;
		}

		// Draw Field
		for (int x = 0; x < nLevelWidth; x++) {
			for (int y = 0; y < nLevelHeight; y++) {
				screen[(y + 2) * nScreenWidth + (x + 2)] = L" ░░░░░░░▓█"[pfield[y * nLevelWidth + x]];
			}
		}

		//draw next piece box
		for (int x = 0; x < 8; x++) {
			for (int y = 0; y < 8; y++) {
				screen[(y + nLevelHeight/2 - 3) * nScreenWidth + (x + nLevelWidth + 8)] = L" ░█"[nextPfield[y * 8 + x]];
			}
		}

		//draw a piece inside next piece box
		for (int x = 0; x < 4; x++) {
			for (int y = 0; y < 4; y++) {
				if (tetrisPieces[nextPiece][Rotate(x, y, 0)] != L'.')
					screen[(y + nLevelHeight / 2 - 1) * nScreenWidth + (x + nLevelWidth + 10)] = 9618;
			}
		}

		//draw piece
		for (int x = 0; x < 4; x++) {
			for (int y = 0; y < 4; y++) {
				if (tetrisPieces[currentPiece][Rotate(x, y, currentPieceRotation)] != L'.')
					screen[(pieceY + y + 2) * nScreenWidth + (pieceX + x + 2)] = 9618;
			}
		}

		//display score
		swprintf(&screen[2 * nScreenWidth + nLevelWidth + 6], 16, L"Score: %8d", score);
		swprintf(&screen[4 * nScreenWidth + nLevelWidth + 6], 16, L"NEXT PIECE: ");


		//snimates completeed lines
		if (!lines.empty()) {
			WriteConsoleOutputCharacter(hConsole, screen, nScreenWidth * nScreenHeight, { 0,0 }, &dwBytesWritten);
			this_thread::sleep_for(400ms);

			for (auto& v : lines) {
				for (int x = 1; x < nLevelWidth - 1; x++) {
					for (int y = v; y > 0; y--) {
						pfield[y * nLevelWidth + x] = pfield[(y - 1) * nLevelWidth + x];
					}
					pfield[x] = 0;
				}
			}
			lines.clear();
		}


		WriteConsoleOutputCharacter(hConsole, screen, nScreenWidth * nScreenHeight, { 0,0 }, &dwBytesWritten);
	}

	//gameOver
	CloseHandle(hConsole);
	cout << "Game Over! Score: " << score << endl;
	system("pause");
	return 0;
}
