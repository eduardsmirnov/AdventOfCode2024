using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/1
public class Day1 : IDay {
	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input1.txt"));

		if (iteration == 1) {
			Iteration1(lines);
		}

		if (iteration == 2) {
			Iteration2(lines);
		}
	}

	private void Iteration1(string[] lines) {
		var list1 = new List<int>();
		var list2 = new List<int>();

		foreach (var line in lines) {
			var parts = line.Split(" ");

			list1.Add(int.Parse(parts.First()));
			list2.Add(int.Parse(parts.Last()));
		}

		list1.Sort();
		list2.Sort();

		var distance = 0;
		for (var idx = 0; idx < list1.Count; idx++) {
			distance += Math.Abs(list2[idx] - list1[idx]);
		}

		Console.WriteLine($"Distance is: {distance}");
	}

	private void Iteration2(string[] lines) {
		var list1 = new List<int>();

		var dict = new Dictionary<int, int>();

		foreach (var line in lines) {
			var parts = line.Split(" ");

			list1.Add(int.Parse(parts.First()));

			var value2 = int.Parse(parts.Last());

			if (dict.TryGetValue(value2, out int dictValue)) {
				dict[value2] = dictValue + 1;
			}
			else {
				dict.Add(value2, 1);
			}
		}

		list1.Sort();

		long score = 0;
		for (var idx = 0; idx < list1.Count; idx++) {
			if (dict.TryGetValue(list1[idx], out int dictValue)) {
				score += list1[idx] * dictValue;
			}
		}

		Console.WriteLine($"Score is: {score}");
	}
}
