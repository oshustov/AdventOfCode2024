namespace AdventOfCode2024.Day1;

public static class DayOne_Part2_SimilarityScore
{
    public static void Run()
    {
        using var file = File.OpenRead("Input.txt");

        var leftList = new List<long>();
        var rightMap = new Dictionary<long, int>();

        using var reader = new StreamReader(file);
        while (!reader.EndOfStream)
        {
            var (left, right) = GetBothParts(reader.ReadLine());
            leftList.Add(left);

            if (!rightMap.TryAdd(right, 1))
                rightMap[right]++;
        }

        var result = leftList.Sum(x => x * rightMap.GetValueOrDefault(x, 0));
        Console.WriteLine(result);
        Console.ReadLine();
    }

    static (long Left, long Right) GetBothParts(string line)
    {
        if (string.IsNullOrEmpty(line))
            return default;

        var parts = line.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        return parts.Length > 1
          ? (long.Parse(parts[0]), long.Parse(parts[1]))
          : default;
    }
}