using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/4
public class Day4: IDay {
	private int _rows;
	private int _cols;

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input4.txt"));

		_rows = lines.Length;
		_cols = lines[0].Length;

		char[,] matrix = new char[_rows, _cols];

		var sum = 0;

		for (var row=0; row < _rows; row++) {
			for (var col=0; col < _cols; col++) {
				matrix[row,col] = (char)lines[row][col];
			}			
		}		

		if (iteration == 1) {
			sum = Iteration1(matrix);
		}

		if (iteration == 2) {
			sum = Iteration2(matrix);
		}

		Console.WriteLine($"Sum is {sum}");
	}

	private int Iteration1(char[,] matrix) {
		var sum = 0;

		// XMAS - 4 symbols
		for (var row = 0; row < _rows; row++) {
			for (var col = 0; col < _cols; col++) {
				if (matrix[row, col] == 'X') {
					sum += XmasCheckRight(matrix, row, col) ? 1 : 0;
					sum += XmasCheckLeft(matrix, row, col) ? 1 : 0;
					sum += XmasCheckUp(matrix, row, col) ? 1 : 0;
					sum += XmasCheckDown(matrix, row, col) ? 1 : 0;
					sum += XmasCheckDownRight(matrix, row, col) ? 1 : 0;
					sum += XmasCheckDownLeft(matrix, row, col) ? 1 : 0;
					sum += XmasCheckUpRight(matrix, row, col) ? 1 : 0;
					sum += XmasCheckUpLeft(matrix, row, col) ? 1 : 0;
				}
			}
		}

		return sum;
	}

	private int Iteration2(char[,] matrix) {
		var sum = 0;

		// X-MAS puzzle - 3 symbols crossed as X
		for (var row = 0; row < _rows; row++) {
			for (var col = 0; col < _cols; col++) {
				if (matrix[row, col] == 'A') {
					if (row - 1 < 0) continue;
					if (row + 1 > _rows - 1) continue;
					if (col - 1 < 0) continue;
					if (col + 1 > _cols - 1) continue;

					if ((X_masCheckDiagonal1(matrix, row, col) || X_masCheckDiagonal1Flip(matrix, row, col))
						&&
						(X_masCheckDiagonal2(matrix, row, col) || X_masCheckDiagonal2Flip(matrix, row, col))) {
						sum += 1;
					}
				}
			}
		}

		return sum;
	}

	#region XMAS
	private bool XmasCheckRight(char[,] matrix, int row, int col) {
		if (col + 3 > _cols-1) return false;  
		if (matrix[row,col+1] != 'M') return false;
		if (matrix[row, col + 2] != 'A') return false;
		if (matrix[row, col + 3] != 'S') return false;

		return true;
	}

	private bool XmasCheckLeft(char[,] matrix, int row, int col) {
		if (col - 3 < 0) return false;
		if (matrix[row, col - 1] != 'M') return false;
		if (matrix[row, col - 2] != 'A') return false;
		if (matrix[row, col - 3] != 'S') return false;

		return true;
	}

	private bool XmasCheckDown(char[,] matrix, int row, int col) {
		if (row + 3 > _rows - 1) return false;
		if (matrix[row + 1, col] != 'M') return false;
		if (matrix[row + 2, col] != 'A') return false;
		if (matrix[row + 3, col] != 'S') return false;

		return true;
	}

	private bool XmasCheckUp(char[,] matrix, int row, int col) {
		if (row - 3 < 0) return false;
		if (matrix[row - 1, col] != 'M') return false;
		if (matrix[row - 2, col] != 'A') return false;
		if (matrix[row - 3, col] != 'S') return false;

		return true;
	}

	private bool XmasCheckDownRight(char[,] matrix, int row, int col) {
		if (col + 3 > _cols - 1) return false;
		if (row + 3 > _rows - 1) return false;
		if (matrix[row + 1, col + 1] != 'M') return false;
		if (matrix[row + 2, col + 2] != 'A') return false;
		if (matrix[row + 3, col + 3] != 'S') return false;

		return true;
	}

	private bool XmasCheckDownLeft(char[,] matrix, int row, int col) {
		if (col - 3 < 0) return false;
		if (row + 3 > _rows - 1) return false;
		if (matrix[row + 1, col - 1] != 'M') return false;
		if (matrix[row + 2, col - 2] != 'A') return false;
		if (matrix[row + 3, col - 3] != 'S') return false;

		return true;
	}

	private bool XmasCheckUpRight(char[,] matrix, int row, int col) {
		if (col + 3 > _cols - 1) return false;
		if (row - 3 < 0) return false;
		if (matrix[row - 1, col + 1] != 'M') return false;
		if (matrix[row - 2, col + 2] != 'A') return false;
		if (matrix[row - 3, col + 3] != 'S') return false;

		return true;
	}

	private bool XmasCheckUpLeft(char[,] matrix, int row, int col) {
		if (col - 3 < 0) return false;
		if (row - 3 < 0) return false;
		if (matrix[row - 1, col - 1] != 'M') return false;
		if (matrix[row - 2, col - 2] != 'A') return false;
		if (matrix[row - 3, col - 3] != 'S') return false;

		return true;
	}
	#endregion

	#region X-MAS
	private bool X_masCheckDiagonal1(char[,] matrix, int row, int col) {
		if (matrix[row - 1, col - 1] != 'M') return false;
		if (matrix[row + 1, col + 1] != 'S') return false;

		return true;
	}

	private bool X_masCheckDiagonal1Flip(char[,] matrix, int row, int col) {
		if (matrix[row - 1, col - 1] != 'S') return false;
		if (matrix[row + 1, col + 1] != 'M') return false;

		return true;
	}

	private bool X_masCheckDiagonal2(char[,] matrix, int row, int col) {
		if (matrix[row - 1, col + 1] != 'M') return false;
		if (matrix[row + 1, col - 1] != 'S') return false;

		return true;
	}

	private bool X_masCheckDiagonal2Flip(char[,] matrix, int row, int col) {
		if (matrix[row - 1, col + 1] != 'S') return false;
		if (matrix[row + 1, col - 1] != 'M') return false;

		return true;
	}

	#endregion
}
