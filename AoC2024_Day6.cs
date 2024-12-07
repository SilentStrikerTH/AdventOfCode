namespace AoC2024_Day6;

class Program {
	static void Main(string[] args) {
		
		// PART ONE:
		
		string input = File.ReadAllText("AoC2024_Day6_Input.txt");
		List<string> mapGrid = input.Split("\n").ToList();
		
		int initYPos = mapGrid.FindIndex(str => str.Contains('^'));
		int initXPos = mapGrid[initYPos].IndexOf('^', StringComparison.Ordinal);
		(int, int) positionYX = (initYPos, initXPos);
		char facingDirection = 'N';

		HashSet<(int, int)> visited = GenPathTiles(facingDirection, positionYX, mapGrid);

		Console.WriteLine($"Part One: {visited.Count}");
		
		
		// PART TWO:
		
		int possibleLoops = 0;
		foreach ((int, int) testPositionYX in visited) {
			List<string> testMapGrid = new List<string>(mapGrid);
			testMapGrid[testPositionYX.Item1] = testMapGrid[testPositionYX.Item1]
				.Remove(testPositionYX.Item2, 1)
				.Insert(testPositionYX.Item2, "#");
			if (GenPathTiles(facingDirection, positionYX, testMapGrid).Count == 0) {
				possibleLoops++;
			}
		}
		
		Console.WriteLine($"Part Two: {possibleLoops}");
		
	}

	static char TurnRight(char facingDirection) {
		switch (facingDirection) {
			case 'N': return 'E';
			case 'E': return 'S';
			case 'S': return 'W';
			case 'W': return 'N';
			default: return 'X';  // This shouldn't happen
		}
	}

	static HashSet<(int, int)> GenPathTiles(char facingDirection, (int, int) positionYX, List<string> mapGrid) {
		Dictionary<char, (int, int)> directionsYX = new Dictionary<char, (int, int)> {
			{ 'N', (-1, 0) },
			{ 'S', (1, 0) },
			{ 'E', (0, 1) },
			{ 'W', (0, -1) }
		};
		HashSet<(int, int)> visited = [positionYX];
		HashSet<(int, int, char)> collisions = [];  // Part two: Check if repeated collisions to detect path looping
		
		// While we are within the grid (check count - 1 otherwise the below if statement tries to access OOB)
		while (positionYX.Item1 < mapGrid.Count - 1 && positionYX.Item2 < mapGrid[0].Length - 1 && positionYX is { Item1: > 0, Item2: > 0 }) {
			// If there is a wall in front of us, turn right
			if (mapGrid[positionYX.Item1 + directionsYX[facingDirection].Item1][positionYX.Item2 + directionsYX[facingDirection].Item2] == '#') {
				(int, int, char) collision = (positionYX.Item1, positionYX.Item2, facingDirection);
				if (collisions.Contains(collision)) {
					// Console.WriteLine("Path loop detected!");
					visited = [];
					break;
				}
				collisions.Add(collision);  // Part two
				facingDirection = TurnRight(facingDirection);
			}
			// Move forward
			positionYX = (positionYX.Item1 + directionsYX[facingDirection].Item1, positionYX.Item2 + directionsYX[facingDirection].Item2);
			// Log new pos in visited
			visited.Add(positionYX);
		}
		
		return visited;
	}
}
