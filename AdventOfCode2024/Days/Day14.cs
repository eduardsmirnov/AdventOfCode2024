using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/14

public class Robot {
	public int PosX { get; set; }
	public int PosY { get; set; }
	public int VelocityX { get; set; }
	public int VelocityY { get; set; }
	public int Move { get; set; }
}

public enum Quadrant {
	TopLeft,
	TopRight,
	BottomLeft,
	BottomRight,
}

public class Day14 : IDay {
	private List<Robot> _robots = new List<Robot>();

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input14.txt"));

		for (var idx = 0; idx < lines.Length; idx++) {
			var parts = lines[idx].Split(' ');
			var position = parts[0].Replace("p=", "").Split(",");
			var velocity = parts[1].Replace("v=", "").Split(",");

			var robot = new Robot {
				PosX = int.Parse(position[0]),
				PosY = int.Parse(position[1]),
				VelocityX = int.Parse(velocity[0]),
				VelocityY = int.Parse(velocity[1]),
				Move = 0
			};

			_robots.Add(robot);
		}

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

		int rows = 103;
		int cols = 101;

		//int rows = 7;
		//int cols = 11;

		for (var idx = 0; idx < 100; idx++) {
			foreach (var robot in _robots) {			
				var newPosX = robot.PosX + robot.VelocityX;
				if (newPosX < 0) {
					newPosX = cols + newPosX;
				}
				if (newPosX >= cols) {
					newPosX = Math.Abs(cols - newPosX);
				}

				var newPosY = robot.PosY + robot.VelocityY;
				if (newPosY < 0) {
					newPosY = rows + newPosY;
				}
				if (newPosY >= rows) {
					newPosY = Math.Abs(rows - newPosY);
				}

				robot.PosX = newPosX;
				robot.PosY = newPosY;
				robot.Move++;
			}
		}

		var quadrants = new Dictionary<Quadrant, int> {
			{ Quadrant.TopLeft, 0 },
			{ Quadrant.TopRight, 0 },
			{ Quadrant.BottomLeft, 0 },
			{ Quadrant.BottomRight, 0 }
		};

		var middleX = cols / 2;
		var middleY = rows / 2;

		foreach (var robot in _robots) {
			if (robot.PosX < middleX && robot.PosY < middleY) quadrants[Quadrant.TopLeft]++;
			if (robot.PosX > middleX && robot.PosY < middleY) quadrants[Quadrant.TopRight]++;
			if (robot.PosX < middleX && robot.PosY > middleY) quadrants[Quadrant.BottomLeft]++;
			if (robot.PosX > middleX && robot.PosY > middleY) quadrants[Quadrant.BottomRight]++;
		}

		sum = quadrants[Quadrant.TopLeft] * quadrants[Quadrant.TopRight] * quadrants[Quadrant.BottomLeft] * quadrants[Quadrant.BottomRight];

		return sum;
	}

	private long Iteration2() {
		long sum = 0;

		int rows = 103;
		int cols = 101;

		//int rows = 7;
		//int cols = 11;

		for (var idx = 0; idx < 100000; idx++) {
			foreach (var robot in _robots) {
				var newPosX = robot.PosX + robot.VelocityX;
				if (newPosX < 0) {
					newPosX = cols + newPosX;
				}
				if (newPosX >= cols) {
					newPosX = Math.Abs(cols - newPosX);
				}

				var newPosY = robot.PosY + robot.VelocityY;
				if (newPosY < 0) {
					newPosY = rows + newPosY;
				}
				if (newPosY >= rows) {
					newPosY = Math.Abs(rows - newPosY);
				}

				robot.PosX = newPosX;
				robot.PosY = newPosY;
				robot.Move++;
			}

			if (IsChristmasTree()) {
				return idx+1;
			}
		}

		return -1;
	}

	// No idea what math should be behind it, found from reddit users that right patttern would be when all robots are visible and do not overlap,
	// so this condition is tested here and gave me a correct answer
	private bool IsChristmasTree() {
		var set = new HashSet<string>();

		foreach (var robot in _robots) {
			set.Add($"{robot.PosX}-{robot.PosY}");
		}

		return set.Count == _robots.Count;
	}
}
	