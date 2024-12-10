namespace AdventOfCode2024.Day1;

public static class DayOne_DistanceBetweenLists
{
    public static void Run()
    {
        using var file = File.OpenRead(Path.Combine("Day1", "Input.txt"));

        var left = new List<int>(1000);
        var right = new List<int>(1000);

        using var reader = new StreamReader(file);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var pieces = line.Split(' ');
            if (pieces.Length < 1)
                continue;

            var toggle = false;
            foreach (var piece in pieces)
            {
                if (string.IsNullOrWhiteSpace(piece))
                    continue;

                if (!toggle)
                {
                    left.Add(int.Parse(piece));
                    toggle = true;
                }
                else
                {
                    right.Add(int.Parse(piece));
                    break;
                }
            }
        }

        if (left.Count != right.Count)
        {
            Console.WriteLine("The column lengths don't match.");
            return;
        }

        var zip = left
          .OrderBy(x => x)
          .Zip(right.OrderBy(y => y))
          .Sum(x => Math.Abs(x.First - x.Second));

        Console.WriteLine(zip);
        Console.ReadLine();
    }
}