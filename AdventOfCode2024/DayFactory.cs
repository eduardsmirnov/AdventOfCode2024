using AdventOfCode2024.Days;

namespace AdventOfCode2024;

public class DayFactory {
	public static IDay CreateInstance(int day) {
		var typename = $"AdventOfCode2024.Days.Day{day}";
		var t = Type.GetType(typename);
		if (t == null) {
			throw new ArgumentException($"Cannot find type {typename}");
		}

		var instance = (IDay?)Activator.CreateInstance(t);
		if (instance == null ) {
			throw new ArgumentException($"Cannot create instance of type {typename} or type is not inherited from interface IDay");
		}

		return instance;
	}
}
