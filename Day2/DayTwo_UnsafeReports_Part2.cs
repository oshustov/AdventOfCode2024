namespace AdventOfCode2024.Day2;

public static class DayTwo_UnsafeReports_Part2
{
  public static void Run()
  {
    using var file = File.OpenRead(Path.Combine("Day2", "Input.txt"));
    using var reader = new StreamReader(file);

    var safeCount = 0;
    var notSafeLevels = new List<List<long>>();

    while (!reader.EndOfStream)
    {
      var levels = GetLevels(reader.ReadLine());
      if (AreSafe(levels.ToList()))
      {
        safeCount++;
        Console.WriteLine(string.Join(", ", levels));
      }
      else
      {
        notSafeLevels.Add(levels.ToList());
      }
    }

    foreach (List<long> notSafeLevel in notSafeLevels)
    {
      foreach (var variant in Variants(notSafeLevel))
      {
        if (AreSafe(variant))
        {
          safeCount++;
          break;
        }
      }
    }

    Console.WriteLine(safeCount);
    Console.ReadLine();
  }

  private static bool AreSafe(List<long> levels)
  {
    var leftNum = 0;
    var rightNum = 1;

    var levelGrowSign = 0L;
    for (; rightNum < levels.Count; leftNum++, rightNum++)
    {
      var difference = levels[leftNum] - levels[rightNum];
      var withinRange = Math.Abs(difference) >= 1 && Math.Abs(difference) <= 3;
      if (difference == 0 || !withinRange)
        return false;

      var adjacentLevelsSign = difference / Math.Abs(difference);

      if (levelGrowSign == 0)
      {
        levelGrowSign = adjacentLevelsSign;
        continue;
      }

      if (levelGrowSign != adjacentLevelsSign)
        return false;
    }

    return true;
  }

  private static IEnumerable<List<long>> Variants(List<long> source)
  {
    for (var i = 0; i < source.Count; i++)
    {
      var variant = new List<long>(source);
      variant.RemoveAt(i);
      yield return variant;
    }
  }

  private static long[] GetLevels(string line)
  {
    if (string.IsNullOrEmpty(line))
      return [];

    return line.Split(' ')
      .Where(x => !string.IsNullOrWhiteSpace(x))
      .Select(long.Parse)
      .ToArray();
  }
}