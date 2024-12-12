using AdventOfCode2024.Models;
using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/12
public class Day12 : IDay {
	private Grid _grid;

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input12.txt"));

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
		return Calculate(false);
	}

	private long Iteration2() {
		return Calculate(true);
	}

	private long Calculate(bool sides) {
		var sum = 0;

		var _regions = new Dictionary<(char, Index), (int area, int perimeter, HashSet<Cell> cells)>();

		// region-direction-row-col
		var fences = new HashSet<string>();

		var cell = _grid.Cell(0, 0);

		while (cell != null) {

			var set = CollectSameRegionCells(cell, new HashSet<Cell>());

			var regionIdx = 0;

			while (true) {
				if (!_regions.TryGetValue((cell.Value, regionIdx), out var region)) {
					region = new(0, 0, new HashSet<Cell>());
					_regions.Add((cell.Value, regionIdx), region);
				}

				var isSameRegion = region.cells.Count == 0 || region.cells.Contains(cell);

				if (isSameRegion) {
					if (region.cells.Count == 0) {
						region.cells = CollectSameRegionCells(cell, new HashSet<Cell>());
					}

					region.area++;  // new cell, area+1

					if (sides) {
						// non-region neighbor, add a perimeter only ones per side
						if (_grid.UpCell(cell)?.Value != cell.Value) 
						{
							fences.Add($"{cell.Value}-{regionIdx}-{Direction.Up}-{cell.Row}-{cell.Col}");

							var tempcell = _grid.LeftCell(cell);
							if (!fences.Contains($"{cell.Value}-{regionIdx}-{Direction.Up}-{tempcell?.Row}-{tempcell?.Col}")) {
								region.perimeter++;
							}							
						}  

						if (_grid.DownCell(cell)?.Value != cell.Value) {
							fences.Add($"{cell.Value}-{regionIdx}-{Direction.Down}-{cell.Row}-{cell.Col}");

							var tempcell = _grid.LeftCell(cell);
							if (!fences.Contains($"{cell.Value}-{regionIdx}-{Direction.Down}-{tempcell?.Row}-{tempcell?.Col}")) {
								region.perimeter++;
							}
						}

						if (_grid.LeftCell(cell)?.Value != cell.Value) {
							fences.Add($"{cell.Value}-{regionIdx}-{Direction.Left}-{cell.Row}-{cell.Col}");

							var tempcell = _grid.UpCell(cell);
							if (!fences.Contains($"{cell.Value}-{regionIdx}-{Direction.Left}-{tempcell?.Row}-{tempcell?.Col}")) {
								region.perimeter++;
							}
						}
						if (_grid.RightCell(cell)?.Value != cell.Value) {
							fences.Add($"{cell.Value}-{regionIdx}-{Direction.Right}-{cell.Row}-{cell.Col}");

							var tempcell = _grid.UpCell(cell);
							if (!fences.Contains($"{cell.Value}-{regionIdx}-{Direction.Right}-{tempcell?.Row}-{tempcell?.Col}")) {
								region.perimeter++;
							}
						}
					}
					else {
						if (_grid.UpCell(cell)?.Value != cell.Value) { region.perimeter++; }  // non-region neighbor, add a perimeter
						if (_grid.DownCell(cell)?.Value != cell.Value) { region.perimeter++; }
						if (_grid.LeftCell(cell)?.Value != cell.Value) { region.perimeter++; }
						if (_grid.RightCell(cell)?.Value != cell.Value) { region.perimeter++; }
					}

					_regions[(cell.Value, regionIdx)] = region;

					break;
				}

				regionIdx++;
			}

			cell = _grid.NextCell(cell);
		}

		sum = _regions.Values.Select(x => x.area * x.perimeter).Sum();

		return sum;
	}

	private HashSet<Cell> CollectSameRegionCells(Cell cell, HashSet<Cell> set) {
		set.Add(cell);

		var upCell = _grid.UpCell(cell);
		if (upCell?.Value == cell.Value && !set.Contains(upCell)) {
			set = CollectSameRegionCells(upCell, set);
		}

		var downCell = _grid.DownCell(cell);
		if (downCell?.Value == cell.Value && !set.Contains(downCell)) {
			set = CollectSameRegionCells(downCell, set);
		}

		var leftCell = _grid.LeftCell(cell);
		if (leftCell?.Value == cell.Value && !set.Contains(leftCell)) {
			set = CollectSameRegionCells(leftCell, set);
		}

		var rightCell = _grid.RightCell(cell);
		if (rightCell?.Value == cell.Value && !set.Contains(rightCell)) {
			set = CollectSameRegionCells(rightCell, set);
		}

		return set;
	}
}
