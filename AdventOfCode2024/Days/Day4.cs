using System.Reflection;

namespace AdventOfCode2024.Days;

public class Day4: IDay {
	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var content = await File.ReadAllTextAsync(Path.Combine(directoryName, "Input", "input4.txt"));
		
		//var sum = 0;

		//if (iteration == 1) {
		//	sum = Iteration1(content);
		//}

		//if (iteration == 2) {
		//	sum = Iteration2(content);
		//}

		//Console.WriteLine($"Sum is {sum}");
	}

	private int Iteration1(string content) {
		var sum = 0;

		return sum;
	}

	private int Iteration2(string content) {
		var sum = 0;

		return sum;
	}
}
