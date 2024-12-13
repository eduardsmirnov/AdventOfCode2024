using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/13

public class Game {
	public long Ax {  get; set; }
	public long Ay { get; set; }
	public long Bx { get; set; }
	public long By { get; set; }
	public long PrizeX { get; set; }
	public long PrizeY { get; set; }
}

public class Day13 : IDay {
	private const int tokensA = 3;
	private const int tokensB = 1;

	private List<Game> _games = new List<Game>();

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var lines = await File.ReadAllLinesAsync(Path.Combine(directoryName, "Input", "input13.txt"));

		for (var idx = 0; idx < lines.Length; idx += 4) {
			var game = new Game();
			var parts = lines[idx].Split(",");
			game.Ax = int.Parse(parts[0].Replace("Button A: X+", "").Trim());
			game.Ay = int.Parse(parts[1].Replace(" Y+", "").Trim());
			parts = lines[idx+1].Split(",");
			game.Bx = int.Parse(parts[0].Replace("Button B: X+", "").Trim());
			game.By = int.Parse(parts[1].Replace(" Y+", "").Trim());
			parts = lines[idx+2].Split(",");
			game.PrizeX = int.Parse(parts[0].Replace("Prize: X=", "").Trim());
			game.PrizeY = int.Parse(parts[1].Replace(" Y=", "").Trim());

			_games.Add(game);
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

	// Easy solution, just brute force to find smallest possible number of tokens.
	// Unfortunately, did not work for big numbers :(
	private long Iteration1() {
		long sum = 0;

		foreach (var game in _games) {
			var minTokens = int.MaxValue;
			for (var a = 0; a < 100; a++) {
				for (var b = 0; b < 100; b++) {
					var x = game.Ax * a + game.Bx * b;
					var y = game.Ay * a + game.By * b;

					if (x == game.PrizeX && y == game.PrizeY) {
						var tokens = a * tokensA + b * tokensB;
						//Console.WriteLine($"a: {a}, b: {b}, price: {a * tokensA + b * tokensB}");
						if (tokens < minTokens) minTokens = tokens;
					}
				}
			}
			if (minTokens < int.MaxValue) {
				sum += minTokens;
			}
		}

		return sum;
	}

	// Thank you guys who knows linear algebra and matrixes calculations! Used one of your proposed solutions here.
	// https://git.sr.ht/~murr/advent-of-code/tree/master/item/2024/13/p2.c
	// a*ax + b*bx = px
	// a*ay + b*by = py
	// b = (py - a*ay) / by
	// a = (px*by - bx*py) / (ax*by - bx*ay)
	private long Iteration2() {
		long sum = 0;

		var gameIdx = 0;
		foreach (var game in _games) {
			var priceX = game.PrizeX + 10000000000000;
			var priceY = game.PrizeY + 10000000000000;

			var da = Math.DivRem(priceX * game.By - priceY * game.Bx, game.Ax * game.By - game.Ay * game.Bx, out var reminderA);
			var db = Math.DivRem(priceY * game.Ax - priceX * game.Ay, game.Ax * game.By - game.Ay * game.Bx, out var reminderB);
			
			if (reminderA == 0 && reminderB == 0) {
				//Console.WriteLine($"game: {gameIdx}");
				sum += 3 * da + db;
			}

			gameIdx++;
		}

		return sum;
	}
}
	