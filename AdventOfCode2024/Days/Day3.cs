using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/3
public class Day3: IDay {
	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var content = await File.ReadAllTextAsync(Path.Combine(directoryName, "Input", "input3.txt"));
		var sum = 0;

		if (iteration == 1) {
			sum = Iteration1(content);
		}

		if (iteration == 2) {
			sum = Iteration2(content);
		}

		Console.WriteLine($"Sum is {sum}");
	}

	private int Iteration1(string content) {
		var sum = 0;

		var regex = @"mul\([0-9]*,[0-9]*\)";

		var matches = Regex.Matches(content, regex).ToList();

		foreach (var m in matches) {
			var value = m.Value.Replace(@"mul(", "").Replace(")", "");
			var parts = value.Split(",");

			sum += int.Parse(parts[0]) * int.Parse(parts[1]);
		}

		return sum;
	}

	private int Iteration2(string content) {
		var sum = 0;

		var regex = @"(mul\([0-9]*,[0-9]*\))|(don't\(\))|(do\(\))";

		var matches = Regex.Matches(content, regex).ToList();

		var enabled = true;

		foreach (var m in matches) {
			if (m.Value == "don't()") {
				enabled = false;
				continue;
			}

			if (m.Value == "do()") {
				enabled = true;
				continue;
			}
			
			if (!enabled) {
				continue;
			}

			var value = m.Value.Replace(@"mul(", "").Replace(")", "");
			var parts = value.Split(",");

			sum += int.Parse(parts[0]) * int.Parse(parts[1]);
		}

		return sum;
	}
}
