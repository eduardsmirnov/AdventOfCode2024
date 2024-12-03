using System.Reflection;

namespace AdventOfCode2024.Days;

public class Day2 : IDay {
	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input2.txt"));
		
		var safeReports = 0;

		if (iteration == 1) {
			safeReports = Iteration1(lines);
		}

		if (iteration == 2) {
			safeReports = Iteration2(lines);
		}

		Console.WriteLine($"Number of safe reports is: {safeReports}");
	}

	private int Iteration1(string[] lines) {
		var safeReports = 0;

		foreach (var line in lines) {
			var parts = line.Split(" ");

			var list1 = parts.Select(x => int.Parse(x)).ToList();

			var isSafeReport = Day2_IsSafeReport(list1);

			if (isSafeReport) {
				safeReports++;
				//Console.WriteLine(string.Join(" ", parts));
			}
		}

		return safeReports;
	}

	private int Iteration2(string[] lines) {
		var safeReports = 0;

		foreach (var line in lines) {
			var parts = line.Split(" ");

			var isSafeReport = false;

			var list1 = parts.Select(x => int.Parse(x)).ToList();

			isSafeReport = Day2_IsSafeReport(list1);

			if (isSafeReport == false) {
				for (var idx = 0; idx < list1.Count; idx++) {
					var tempList = list1.Select(x => x).ToList();
					tempList.RemoveAt(idx);

					isSafeReport = Day2_IsSafeReport(tempList);
					if (isSafeReport) break;
				}
			}

			if (isSafeReport) {
				safeReports++;
				//Console.WriteLine(string.Join(" ", parts));
			}
		}

		return safeReports;
	}

	private bool Day2_IsSafeReport(List<int> list1) {
		var isSafeReport = true;

		bool? isIncreasing = null;

		for (var idx = 1; idx < list1.Count(); idx++) {
			if (list1[idx] == list1[idx - 1]) {
				isSafeReport = false;
				break;
			}

			if (isIncreasing == null) {
				isIncreasing = list1[idx] > list1[idx - 1];
			}

			if (isIncreasing == true && (list1[idx] < list1[idx - 1])) {
				isSafeReport = false;
				break;
			}

			if (isIncreasing == false && (list1[idx] > list1[idx - 1])) {
				isSafeReport = false;
				break;
			}

			var diff = Math.Abs(list1[idx] - list1[idx - 1]);
			if (diff < 1 || diff > 3) {
				isSafeReport = false;
				break;
			}
		}

		return isSafeReport;
	}
}
