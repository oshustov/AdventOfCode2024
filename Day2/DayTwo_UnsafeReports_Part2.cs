namespace AdventOfCode2024.Day2;

public static class DayTwo_UnsafeReports_Part2
{
  public static void Run()
  {
    using var file = File.OpenRead(Path.Combine("Day2", "Input.txt"));
    using var reader = new StreamReader(file);

    var safeCount = 0;
    while (!reader.EndOfStream)
    {
      var sequence = reader.ReadLine()!.AsSequence();
      if (IsSafe(sequence) || VariantsOf(sequence).Any(IsSafe))
        safeCount++;
    }

    Console.WriteLine(safeCount);
    Console.ReadLine();
  }

  private static bool IsSafe(List<long> levels)
  {
    var leftIndex = 0;
    var rightIndex = 1;
    var sign = 0L;

    for (; rightIndex < levels.Count; leftIndex++, rightIndex++)
    {
      var difference = levels[leftIndex] - levels[rightIndex];
      var withinRange = Math.Abs(difference) >= 1 && Math.Abs(difference) <= 3;
      if (difference == 0 || !withinRange)
        return false;

      var adjacentLevelsSign = Math.Sign(difference);

      if (sign == 0)
      {
        sign = adjacentLevelsSign;
        continue;
      }

      if (sign != adjacentLevelsSign)
        return false;
    }

    return true;
  }

  private static IEnumerable<List<long>> VariantsOf(List<long> source)
  {
    for (var i = 0; i < source.Count; i++)
    {
      var variant = new List<long>(source);
      variant.RemoveAt(i);
      yield return variant;
    }
  }
}

public static class Extensions
{
  public static List<long> AsSequence(this string self) =>
    string.IsNullOrWhiteSpace(self)
      ? []
      : self.Split(' ')
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(long.Parse)
        .ToList();
}