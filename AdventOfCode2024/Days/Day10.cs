using AdventOfCode2024.Models;
using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/10
public class Day10: IDay {
	private Grid _grid;

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input10.txt"));

		_grid = new Grid(lines);

		long sum = 0;

		if (iteration == 1) {
			sum = Iteration1();
		}

		if (iteration == 2) {
			sum = Iteration2();
		}

		Console.WriteLine($"Sum is {sum}");
	}

	private long Iteration1() {
		long sum = 0;

		var cell = _grid.Cell(0, 0);
		while (cell != null) {
			if (cell.Value == '0') {
				var tempsum = 0;

				var celltemp = _grid.Cell(0, 0);
				while (celltemp != null) {
					if (celltemp.Value == '9' && celltemp.IsVisited) {
						celltemp.IsVisited = false;
					}

					celltemp = _grid.NextCell(celltemp);
				}

				var pathsCount = FindPaths(cell, -1);

				celltemp = _grid.Cell(0, 0);
				while (celltemp != null) {
					if (celltemp.Value == '9' && celltemp.IsVisited) {
						tempsum++;
					}

					celltemp = _grid.NextCell(celltemp);
				}

				Console.WriteLine($"Cell {cell.Row}, {cell.Col} has score {tempsum}");

				sum += tempsum;
			}

			cell = _grid.NextCell(cell);
		}

		return sum;
	}

	private long Iteration2() {
		long sum = 0;

		var cell = _grid.Cell(0, 0);
		while (cell != null) {
			if (cell.Value == '0') {
				sum += FindPaths(cell, -1);
			}

			cell = _grid.NextCell(cell);
		}

		return sum;
	}

	private int FindPaths(Cell cell, int prevValue) {
		var result = 0;

		var cellValue = int.Parse(cell.Value.ToString());

		if (cellValue == (prevValue + 1)) {
			if (cellValue == 9) {
				cell.IsVisited = true;
				return 1;
			}

			var upCell = _grid.UpCell(cell);
			if (upCell != null) {

				result += FindPaths(upCell, cellValue);
			}

			var downCell = _grid.DownCell(cell);
			if (downCell != null) {
				result += FindPaths(downCell, cellValue);
			}

			var leftCell = _grid.LeftCell(cell);
			if (leftCell != null) {
				result += FindPaths(leftCell, cellValue);
			}

			var rightCell = _grid.RightCell(cell);
			if (rightCell != null) {
				result += FindPaths(rightCell, cellValue);
			}
		}
		else {
			return 0;
		}

		return result;
	}
}
