using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/11
public class Day11: IDay {
	private List<long> _stones = new List<long>();

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var content = await File.ReadAllTextAsync(Path.Combine(directoryName, "Input", "input11.txt"));

		_stones = content.Split(" ").Select(x => long.Parse(x)).ToList();

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
		return Process(25);
	}

	private long Iteration2() {
		return Process(75);
	}

	private long Process(int steps) {
		long sum = 0;

		/*
			Every stone is transformed or split N times.
			Every splitted stone is transformed or split N-1 times. etc

			Calculating all stones takes tooooo much time. Lets hash already calculated combinations of stone+step and re-use them.		
		 */

		foreach (var stone in _stones) {
			sum += HandleStone(stone, 0, steps);
		}

		return sum + _stones.Count();
	}


	private Dictionary<string, long> _dict = new Dictionary<string, long>();

	private long HandleStone(long stone, long sum, int remainingSteps) {
		if (_dict.ContainsKey($"{stone}_{remainingSteps}")) {
			return _dict[$"{stone}_{remainingSteps}"];
		}

		if (remainingSteps <= 0) return sum;

		if (stone == 0) {
			return HandleStone(1, sum, remainingSteps-1);
		}
		else if (stone.ToString().Length % 2 == 0) {
			var res1 = HandleStone(long.Parse(stone.ToString().Substring(0, stone.ToString().Length / 2)), 0, remainingSteps - 1);

			var res2 = HandleStone(long.Parse(stone.ToString().Substring(stone.ToString().Length / 2)), 0, remainingSteps - 1);

			_dict.TryAdd($"{stone}_{remainingSteps}", sum + res1 + res2 + 1);

			return sum + res1 + res2 + 1;  // cause we split here one stone into two
		}
		else {
			return HandleStone(stone * 2024, sum, remainingSteps - 1);
		}
	}
}
