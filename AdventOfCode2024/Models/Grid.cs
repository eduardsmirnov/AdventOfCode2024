﻿namespace AdventOfCode2024.Models;

public class Cell {
	public int Row { get; set; }
	public int Col { get; set; }
	public char Value { get; set; }
	public Direction Direction { get; set; }
	public bool IsInitial { get; set; }
	public bool IsVisited { get; set; }
	public bool IsObstacle { get; set; }
	public bool IsRotation { get; set; }
	public bool IsExtraObstacle { get; set; }

	public bool IsNeighbourOf(Cell cell) {
		if (cell == null) return false;

		return (Row == cell.Row && Col == cell.Col + 1) ||
				(Row == cell.Row && Col == cell.Col - 1) ||
				(Row == cell.Row + 1 && Col == cell.Col) ||
				(Row == cell.Row - 1 && Col == cell.Col);
				//(Row == cell.Row - 1 && Col == cell.Col - 1) ||
				//(Row == cell.Row - 1 && Col == cell.Col + 1) ||
				//(Row == cell.Row + 1 && Col == cell.Col - 1) ||
				//(Row == cell.Row + 1 && Col == cell.Col + 1);
	}
}

public class Grid {
	public Cell[,] _matrix;
	public List<(int, int, Direction)> _path;
	public List<string> _obstaclesList = new List<string>();

	public int _rows { get; private set; }
	public int _cols { get; private set; }

	private int _initialRow;
	private int _initialCol;

	private Direction _currentDirection;

	public Grid(string[]? lines) {
		_rows = lines.Length;
		_cols = lines[0].Length;

		_matrix = new Cell[_rows, _cols];

		_path = new List<(int, int, Direction)>();

		for (var row = 0; row < _rows; row++) {
			for (var col = 0; col < _cols; col++) {
				var cell = new Cell { Row = row, Col = col, Value = (char)lines[row][col] };

				if (lines[row][col] == '^') {
					cell.IsInitial = true;
					cell.IsVisited = true;
					cell.Direction = Direction.Up;

					_initialRow = row;
					_initialCol = col;
					_currentDirection = Direction.Up;
				}

				if (lines[row][col] == '#') {
					cell.IsObstacle = true;
				}

				_matrix[row, col] = cell;
			}
		}
	}

	public Grid(int rows, int cols) {
		_rows = rows;
		_cols = cols;

		_matrix = new Cell[_rows, _cols];

		for (var row = 0; row < _rows; row++) {
			for (var col = 0; col < _cols; col++) {
				var cell = new Cell { Row = row, Col = col, Value = '.' };
				_matrix[row, col] = cell;
			}
		}
	}

	public int VisitedCells() {
		var sum = 0;
		for (var row = 0; row < _rows; row++) {
			for (var col = 0; col < _cols; col++) {
				if (_matrix[row, col].IsVisited) sum++;
			}
		}

		return sum;
	}

	public List<Cell> All() {
		var list = new List<Cell>();

		for (var row = 0; row < _rows; row++) {
			for (var col = 0; col < _cols; col++) {
				list.Add(_matrix[row, col]);
			}
		}

		return list; ;
	}

	public Cell? Cell(int row, int col) {
		if (row >= _rows || row < 0) { return null; }
		if (col >= _cols || col < 0) { return null; }

		return _matrix[row, col];
	}

	public Cell? NextCell(Cell cell) {
		var nextRow = cell.Row;

		var nextCol = cell.Col + 1;
		if (nextCol >= _cols) {
			nextCol = 0;
			nextRow++;
		}

		if (nextRow >= _rows) { return null; }		

		return _matrix[nextRow, nextCol];
	}

	public Cell? UpCell(Cell cell) {
		var nextRow = cell.Row - 1;
		var nextCol = cell.Col;

		if (nextRow >= _rows || nextRow < 0) { return null; }
		if (nextCol >= _cols || nextCol < 0) { return null; }

		return _matrix[nextRow, nextCol];
	}

	public Cell? DownCell(Cell cell) {
		var nextRow = cell.Row + 1;
		var nextCol = cell.Col;

		if (nextRow >= _rows || nextRow < 0) { return null; }
		if (nextCol >= _cols || nextCol < 0) { return null; }

		return _matrix[nextRow, nextCol];
	}

	public Cell? LeftCell(Cell cell) {
		var nextRow = cell.Row;
		var nextCol = cell.Col - 1;

		if (nextRow >= _rows || nextRow < 0) { return null; }
		if (nextCol >= _cols || nextCol < 0) { return null; }

		return _matrix[nextRow, nextCol];
	}

	public Cell? RightCell(Cell cell) {
		var nextRow = cell.Row;
		var nextCol = cell.Col + 1;

		if (nextRow >= _rows || nextRow < 0) { return null; }
		if (nextCol >= _cols || nextCol < 0) { return null; }

		return _matrix[nextRow, nextCol];
	}

	public int BuildPath(int? maxIterations = null) {
		var iterations = 0;

		var cell = _matrix[_initialRow, _initialCol];

		while (cell != null) {
			if (iterations > (maxIterations ?? iterations)) break;

			Cell? nextCell;

			var nextDirection = cell.Direction;
			var nextRow = 0;
			var nextCol = 0;
			var cellValue = '^';

			if (cell.Direction == Direction.Up) {
				nextDirection = Direction.Right;
				nextRow = cell.Row - 1;
				nextCol = cell.Col;
				cellValue = '^';
			}
			if (cell.Direction == Direction.Down) {
				nextDirection = Direction.Left;
				nextRow = cell.Row + 1;
				nextCol = cell.Col;
				cellValue = 'V';
			}
			if (cell.Direction == Direction.Left) {
				nextDirection = Direction.Up;
				nextRow = cell.Row;
				nextCol = cell.Col - 1;
				cellValue = '<';
			}
			if (cell.Direction == Direction.Right) {
				nextDirection = Direction.Down;
				nextRow = cell.Row;
				nextCol = cell.Col + 1;
				cellValue = '>';
			}

			try {
				nextCell = _matrix[nextRow, nextCol];
			}
			catch (Exception e) {
				break; // out of map
			}

			if (nextCell.IsObstacle) {
				cell.Value = cellValue;
				cell.Direction = nextDirection;
				cell.IsRotation = true;
				cell.IsVisited = true;
			}
			else {
				cell.Value = cellValue;
				nextCell.IsVisited = true;
				nextCell.Direction = cell.Direction;
				cell = nextCell;
			}

			if (maxIterations == null) {
				_path.Add(new(cell.Row, cell.Col, cell.Direction));
			}

			iterations++;
		}

		return iterations;
	}

	public void PrintMap() {
		//return;
		Console.Clear();
		Console.WriteLine("\x1b[3J");
		Console.WriteLine("====================================================");
		for (var row = 0; row < _rows; row++) {
			for (var col = 0; col < _cols; col++) {
				if (_matrix[row, col].IsExtraObstacle) {
					Console.Write("@");
				}
				else {
					Console.Write(_matrix[row, col].Value);
				}
			}
			Console.WriteLine();
		}
		Console.WriteLine("====================================================");
	}
}
