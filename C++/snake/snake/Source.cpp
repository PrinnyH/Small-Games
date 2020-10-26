#include <iostream>
#include <windows.h>
#include <vector>

using namespace std;

enum direction {NONE ,UP, DOWN, LEFT, RIGHT };
direction dir = direction::NONE;

int length = 12, width = 24;
vector<int> player_x = {width/2};
vector<int> player_y = {length/2};
int playerlength = 1;
int score = 0;
vector<int> worm_x = { 3 };
vector<int> worm_y = { 3 };
bool gameOver = false;

void movePlayer(int yMAX, int xMAX){
	for (int i = player_x.size() - 1; i > 0; --i) {
		player_x[i] = player_x[i - 1];
		player_y[i] = player_y[i - 1];
	}
	//moves the players head around
	if (dir == direction::UP) {
		player_y[0] -= 1;
		if (player_y[0] == 0) {
			player_y[0] = yMAX - 1;
		}
	}
	else if (dir == direction::DOWN) {
		player_y[0] += 1;
		if (player_y[0] == yMAX) {
			player_y[0] = 1;
		}
	}
	else if (dir == direction::RIGHT) {
		player_x[0] += 1;
		if (player_x[0] == xMAX) {
			player_x[0] = 1;
		}
	}
	else if (dir == direction::LEFT) {
		player_x[0] -= 1;
		if (player_x[0] == 0) {
			player_x[0] = xMAX - 1;
		}
	}

	for (int i = 1; i < player_x.size(); i++) {
		if ((player_x[0] == player_x[i]) && (player_y[0] == player_y[i])) {
			gameOver = true;
		}
	}
	//checks if player is touching fuit;
	if (player_x[0] == worm_x[0] && player_y[0] == worm_y[0]) {
		score++;
		playerlength++;
		player_x.push_back(0);
		player_y.push_back(0);
		worm_x[0] = (rand() % (width-1)) + 1;
		worm_y[0] = (rand() % (length - 1)) + 1;
	}
}

//grabs inputs and changes the direction of player
void getInputs(int length, int height) {
	Sleep(100);
	bool keyPressed = false;
	if (!keyPressed) {
		keyPressed = true;
		if (GetAsyncKeyState(VK_UP)) {
			dir = direction::UP;
		}
		else if (GetAsyncKeyState(VK_DOWN)) {
			dir = direction::DOWN;
		}
		else if (GetAsyncKeyState(VK_RIGHT)) {
			dir = direction::RIGHT;
		}
		else if (GetAsyncKeyState(VK_LEFT)) {
			dir = direction::LEFT;
		}
	}
	movePlayer(length, height);
}


// simple function to test current enum setting
void checkEnum() {
	if (dir == direction::UP) {
		cout << "up";
	}
}

void draw(const int &length, const int& width) {
	//Sleep(500);
	system("cls"); //clears the console
	// WILL DRAW WALLS AND CHARACTER
	//draws walls
	for (int i = 0; i <= length; i++) {
		for (int j = 0; j <= width; j++) {
			bool bodyPrinted = false;
			if ( i == 0 || i == length) { // prints line for top and bottm
				cout << "#";
			}
			else if ( (j == 0 || j == width) && (i != 0 || i != length) ) { // prints side walls
				cout << "#";
			}
			else { // prints empty space between ~~ will also house the player drawing
				
				for (int bod = 0; bod < player_x.size(); ++bod) {
					if (player_x[bod] == j && player_y[bod] == i) {
						cout << "@";
						bodyPrinted = true;
					}
				}
				if (worm_x[0] == j && worm_y[0] == i) {
					cout << "~";
					bodyPrinted = true;
				}

				if (!bodyPrinted){
					cout << " ";
				}
			}
		}
		cout << endl;
	}
	cout << "Score: " << score;
	//cout << "\nPlayer X: " << player_x[0] << " Player Y: " << player_y[0];
	//cout << "\nWorm X: " << worm_x[0] << " Worm Y: " << worm_y[0];
}

int main() {
	//loops the main
	while (true) {
		//checkEnum();
		draw(length, width);
		if (GetAsyncKeyState(VK_ESCAPE) || gameOver) {	//exit
			dir = direction::NONE;
			cout << "\nGAME OVER";
			return 0;
		}
		getInputs(length, width); // grabs the inputs // also move the player
	}
}