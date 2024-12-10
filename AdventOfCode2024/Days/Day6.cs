using AdventOfCode2024.Models;
using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/6

public class Day6: IDay {
	private int _rows;
	private int _cols;

	private int _currentRow;
	private int _currentCol;

	private List<string> _obstaclesList = new List<string>();

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input6.txt"));

		var grid = new Grid(lines);
		var validIterations = grid.BuildPath();

		_rows = lines.Length;
		_cols = lines[0].Length;

		char[,] matrix = new char[_rows, _cols];

		var sum = 0;

		for (var row=0; row < _rows; row++) {
			for (var col=0; col < _cols; col++) {
				matrix[row, col] = (char)lines[row][col];

				if (lines[row][col] == '^') {
					matrix[row, col] = 'X';
					_currentRow = row;
					_currentCol = col;
				}
			}			
		}

		Console.WriteLine($"Start from position {_currentCol}, {_currentRow}");

		if (iteration == 1) {
			sum = Iteration1(grid);
		}

		if (iteration == 2) {
			sum = Iteration2(grid, matrix, lines, validIterations);
		}

		//grid.PrintMap();

		Console.WriteLine($"Sum is {sum}");
	}

	private int Iteration1(Grid grid) {
		return grid.VisitedCells();
	}

	private int Iteration2(Grid grid, char[,] matrix, string[]? lines, int validIterations) {
		// spent quite a lot of time, no better idea than brute force,
		// for every single cell in a path, try to set next cell as an obstacle and attempt to pass the whole path,
		// positive condition when we will step into the loop
		foreach (var p in grid._path) {
			var cell = grid._matrix[p.Item1, p.Item2];
			var direction = p.Item3;

			var tempGrid = new Grid(lines);

			Cell? nextCellInPath = null;

			try {
				if (direction == Direction.Up) {
					nextCellInPath = tempGrid._matrix[cell.Row - 1, cell.Col];
				}
				else if (direction == Direction.Down) {
					nextCellInPath = tempGrid._matrix[cell.Row + 1, cell.Col];
				}
				else if (direction == Direction.Left) {
					nextCellInPath = tempGrid._matrix[cell.Row, cell.Col - 1];
				}
				else if (direction == Direction.Right) {
					nextCellInPath = tempGrid._matrix[cell.Row, cell.Col + 1];
				}
			}
			catch (Exception e) {
				continue;
			}

			if (nextCellInPath?.IsObstacle == true) { continue; }

			nextCellInPath.Value = '#';
			nextCellInPath.IsObstacle = true;

			var tempIterations = tempGrid.BuildPath(validIterations * 4);
			if (tempIterations > validIterations * 3) {
				var x = $"{nextCellInPath.Row}-{nextCellInPath.Col}";
				if (_obstaclesList.Contains(x)) {
				}
				else {
					_obstaclesList.Add(x);
				}
			}
		}

		return  _obstaclesList.Count;
	}
}
