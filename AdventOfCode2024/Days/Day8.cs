using AdventOfCode2024.Models;
using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/8
public class Day8: IDay {
	private Dictionary<char, List<Cell>> _cellsDictionary;

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input8.txt"));

		var grid = new Grid(lines);

		_cellsDictionary = new Dictionary<char, List<Cell>>();

		for (var row = 0; row < grid._rows; row++) {
			for (var col = 0; col < grid._cols; col++) {
				var cell = grid._matrix[row, col];
				var c = cell.Value;
				if (c == '.') continue;

				if (_cellsDictionary.ContainsKey(c)) {
					_cellsDictionary[c].Add(cell);
				}
				else {
					_cellsDictionary.Add(c, new List<Cell> { cell });
				}
			}
		}

		long sum = 0;

		if (iteration == 1) {
			sum = Iteration1(grid);
		}

		if (iteration == 2) {
			sum = Iteration2(grid);
		}

		Console.WriteLine($"Sum is {sum}");
	}

	private long Iteration1(Grid grid) {
		var antiNodes = new List<Cell>();

		foreach (var key in _cellsDictionary.Keys) {
			for (var idx=0; idx< _cellsDictionary[key].Count; idx++) {
				for (var j = 0; j < _cellsDictionary[key].Count; j++) {
					if (j == idx) continue;

					var cell = _cellsDictionary[key][idx];
					var cell2 = _cellsDictionary[key][j];

					var tempRow = cell.Row - cell2.Row;
					var tempCol = cell.Col - cell2.Col;

					var antiCellRow = cell.Row + tempRow;
					var antiCellCol = cell.Col + tempCol;
					if (antiCellCol < 0 || antiCellCol > grid._cols - 1 || antiCellRow < 0 || antiCellRow > grid._rows - 1) {
						break;
					}

					Cell? antiCell = grid._matrix[cell.Row + tempRow, cell.Col + tempCol];

					if (!antiNodes.Any(an => an.Row == antiCell.Row && an.Col == antiCell.Col)) {
						Console.WriteLine($"{antiCell.Row}, {antiCell.Col}");

						antiNodes.Add(antiCell);
					}
				}
			}
		}

		//grid.PrintMap();

		return antiNodes.Count();
	}

	private long Iteration2(Grid grid) {
		var antiNodes = new List<Cell>();


		foreach (var key in _cellsDictionary.Keys) {
			for (var idx = 0; idx < _cellsDictionary[key].Count; idx++) {
				for (var j = 0; j < _cellsDictionary[key].Count; j++) {
					if (j == idx) continue;

					var cell = _cellsDictionary[key][idx];
					var cell2 = _cellsDictionary[key][j];


					for (var step = -52; step < 52; step++) {
						var tempRow = step * (cell.Row - cell2.Row);
						var tempCol = step * (cell.Col - cell2.Col);

						var antiCellRow = cell.Row + tempRow;
						var antiCellCol = cell.Col + tempCol;
						if (antiCellCol < 0 || antiCellCol > grid._cols-1 || antiCellRow < 0 || antiCellRow > grid._rows-1) {
							continue;
						}

						Cell? antiCell = grid._matrix[cell.Row + tempRow, cell.Col + tempCol];

						if (!antiNodes.Any(an => an.Row == antiCell.Row && an.Col == antiCell.Col)) {
							Console.WriteLine($"{antiCell.Row}, {antiCell.Col}");

							antiNodes.Add(antiCell);
						}
					}
				}
			}
		}

		return antiNodes.Count();
	}
}
