using System.Reflection;

namespace AdventOfCode2024.Days;

// https://adventofcode.com/2024/day/9
public class Day9: IDay {
	private List<int?> _data = new List<int?>();
	private List<int> _emptyPlaces = new List<int>();
	private List<Slot> _slots = new List<Slot>();

	public async Task RunAsync(int iteration) {
		var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		var content = await File.ReadAllTextAsync(Path.Combine(directoryName, "Input", "input9.txt"));

		var fileIndex = 0;
		for (var idx = 0; idx < content.Length; idx++) {
			var num = int.Parse(content[idx].ToString());
			if (idx % 2 == 0) {
				_slots.Add(new Slot { InitiallyEmpty = false, Index = idx, Size = num, EmptySize = 0, Items = Enumerable.Repeat<int?>(fileIndex, num).ToList() });

				for (var i = 0; i < num; i++) {
					_data.Add(fileIndex);
				}
				fileIndex++;
			}
			else {
				_slots.Add(new Slot { InitiallyEmpty = true, Index = idx, Size = num, EmptySize = num, Items = Enumerable.Repeat<int?>(null, num).ToList() });

				for (var i = 0; i < num; i++) {
					_data.Add(null);
					_emptyPlaces.Add(_data.Count - 1);
				}
			}
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

	// TODO: change Iteration1 code to work with Slots collection?
	private long Iteration1() {
		long sum = 0;		

		// go through empty spaces, fill in with the last available file content
		var emptyPlacesIdx = 0;
		var numbersIdx = _data.Count - 1;

		while (true) {
			var currentIdx = _emptyPlaces[emptyPlacesIdx];

			if (_data[numbersIdx] == null) {
				numbersIdx--;
				continue;
			}

			if (currentIdx > numbersIdx || currentIdx > _data.Count || numbersIdx < 0) {
				break;
			}

			_data[currentIdx] = _data[numbersIdx];
			_data[numbersIdx] = null;
			emptyPlacesIdx++;
			numbersIdx--;
		}

		for (var idx = 0; idx < _data.Count; idx++) {
			sum += (idx * _data[idx] ?? 0);
		}

		return sum;
	}

	private long Iteration2() {
		long sum = 0;

		// start from the last file, try to fit in the first available slot from the beginnning
		var emptySlots = _slots.Where(x => x.InitiallyEmpty == true).ToList();
		var numericSlots = _slots.Where(x => x.InitiallyEmpty == false).ToList();

		for (var idx=numericSlots.Count-1; idx >= 0; idx--) {
			var numericSlot = numericSlots[idx];

			var emptySlot = emptySlots.FirstOrDefault(x => x.EmptySize >= numericSlot.Size);
			if (emptySlot == null || emptySlot.Index > numericSlot.Index) {
				continue;
			}

			for (var j=0; j < numericSlot.Items.Count; j++) {
				emptySlot.Items[emptySlot.Items.Count - emptySlot.EmptySize + j] = numericSlot.Items[j];
				numericSlot.Items[j] = null;
			}
			emptySlot.EmptySize -= numericSlot.Items.Count;
			;
		}

		//PrintData();

		var itemIdx = 0;
		foreach (var slot in _slots) {
			foreach (var item in slot.Items) {
				sum += (itemIdx * item ?? 0);
				itemIdx++;
			}
		}

		return sum;
	}

	private void PrintData() {
		foreach (var slot in _slots) {
			foreach (var item in slot.Items) {
				Console.Write(item?.ToString() ?? ".");
			}
		}
		Console.WriteLine();
	}
}

public class Slot {
	public int Index { get; set; }
	public bool InitiallyEmpty {  get; set; }
	public int Size { get; set; }
	public int EmptySize { get; set; }
	public List<int?> Items { get; set; } = new List<int?>();
}
