using AdventOfCode2024;

var day = 4;
var iteration = 2;

if (args.Length > 0) {
	day = int.Parse(args[0]);
}

if (args.Length > 1) {
	iteration = int.Parse(args[1]);
}

Console.WriteLine("==========================================");
Console.WriteLine("ADVENT OF CODE 2024");
Console.WriteLine($"Day: {day}");
Console.WriteLine($"Iteraion: {iteration}");
Console.WriteLine("==========================================");

await DayFactory.CreateInstance(day).RunAsync(iteration);