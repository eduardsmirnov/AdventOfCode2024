using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/7
public class Day7: IDay {
	private List<(long, List<long>)> _data = new List<(long, List<long>)> ();

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input7.txt"));
		long sum = 0;

		foreach (var line in lines) {
			var parts = line.Split(":");
			var result = long.Parse(parts[0]);
			var numbers = parts[1].Trim().Split(" ").Select(x => long.Parse(x)).ToList();

			_data.Add((result, numbers));			
		}

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

        foreach (var item in _data)
        {
			var results = new List<long> { item.Item2.First() };
			for (var idx = 1; idx < item.Item2.Count; idx++) {
				var results2 = new List<long>();
				foreach (var r in results) {
					results2.Add(r * item.Item2[idx]);
					results2.Add(r + item.Item2[idx]);
				}
				results = results2;
			}
			
			if (results.Any(x => x == item.Item1)) {
				sum += item.Item1;
			}
		}

		return sum;
	}

	private long Iteration2() {
		long sum = 0;

		foreach (var item in _data) {
			var results = new List<long> { item.Item2.First() };
			for (var idx = 1; idx < item.Item2.Count; idx++) {
				var results2 = new List<long>();
				foreach (var r in results) {
					results2.Add(r * item.Item2[idx]);
					results2.Add(r + item.Item2[idx]);

					results2.Add(long.Parse($"{r}{item.Item2[idx]}"));
				}
				results = results2;
			}

			if (results.Any(x => x == item.Item1)) {
				sum += item.Item1;
			}
		}

		return sum;
	}
}
