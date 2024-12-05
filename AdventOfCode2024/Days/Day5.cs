using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/5
public class Day5: IDay {
	private Dictionary<int, HashSet<int>> _pagesWithDependencies = new Dictionary<int, HashSet<int>>();
	private List<List<int>> _data = new List<List<int>>();

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input5.txt"));
		var sum = 0;

		var loadingData = false;
		foreach (var line in lines) {
			if (string.IsNullOrEmpty(line)) {
				loadingData = true;
				continue;
			}

			if (loadingData) {
				HandleData(line);
			}
			else {
				HandleRule(line);
			}
		}

		if (iteration == 1) {
			sum = Iteration1();
		}

		if (iteration == 2) {
			sum = Iteration2();
		}

		Console.WriteLine($"Sum is {sum}");
	}

	private int Iteration1() {
		var sum = 0;

		foreach (var d in _data) {
			var isValid = true;
			for (var idx = 0; idx < d.Count(); idx++) {
				var dependencies = _pagesWithDependencies[d[idx]];

				if (!d.Skip(idx + 1).All(x => dependencies.Contains(x))) {
					isValid = false;
					break;
				}
			}

			if (isValid) {
				var middleIndex = d.Count() / 2;
				sum += d[middleIndex];
			}
		}

		return sum;
	}

	private int Iteration2() {
		var sum = 0;

		foreach (var d in _data) {
			var isValid = true;
			for (var idx = 0; idx < d.Count(); idx++) {
				var dependencies = _pagesWithDependencies[d[idx]];

				if (!d.Skip(idx + 1).All(x => dependencies.Contains(x))) {
					isValid = false;
					break;
				}
			}

			if (!isValid) {
				var pagesOrdered = d;

				foreach (var page in _pagesWithDependencies.Keys) {
					if (!pagesOrdered.Contains(page)) { continue; }

					var dependencies = _pagesWithDependencies[page];

					foreach (var dependentPage in dependencies) {
						if (!pagesOrdered.Contains(dependentPage)) { continue; }

						if (pagesOrdered.IndexOf(page) > pagesOrdered.IndexOf(dependentPage)) {							
							Move(pagesOrdered, pagesOrdered.IndexOf(page), pagesOrdered.IndexOf(dependentPage));
						}
					}
				}

				var middleIndex = pagesOrdered.Count() / 2;

				sum += pagesOrdered[middleIndex];
			}
		}

		return sum;
	}

	private void HandleRule(string line) {
		var parts = line.Split("|");

		var firstPage = int.Parse(parts[0]);
		var nextPage = int.Parse(parts[1]);

		if (_pagesWithDependencies.ContainsKey(firstPage)) {
			_pagesWithDependencies[firstPage].Add(nextPage);
		}
		else {
			_pagesWithDependencies.Add(firstPage, new HashSet<int> { nextPage });
		}

		if (!_pagesWithDependencies.ContainsKey(nextPage)) {
			_pagesWithDependencies.Add(nextPage, new HashSet<int>());
		}
	}

	private void HandleData(string line) {
		var parts = line.Split(",").Select(x => int.Parse(x)).ToList();
		_data.Add(parts);
	}

	private void Move(List<int> collection, int oldIndex, int newIndex) {
		if (newIndex < 0) newIndex = 0;

		int item = collection[oldIndex];
		collection.RemoveAt(oldIndex);
		collection.Insert(newIndex, item);
	}
}
