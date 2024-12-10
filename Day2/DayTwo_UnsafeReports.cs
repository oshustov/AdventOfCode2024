namespace AdventOfCode2024.Day2;

public static class DayTwo_UnsafeReports
{
  public static void Run()
  {
    using var file = File.OpenRead(Path.Combine("Day2", "Input.txt"));
    using var reader = new StreamReader(file);

    var safeCount = 0;
    while (!reader.EndOfStream)
    {
      var levels = GetLevels(reader.ReadLine());
      if (AreSafe(levels))
      {
        safeCount++;
        Console.WriteLine(string.Join(", ", levels));
      }
    }

    Console.WriteLine(safeCount);
    Console.ReadLine();
  }

  private static bool AreSafe(long[] levels)
  {
    var leftNum = 0;
    var rightNum = 1;

    var levelGrowSign = 0L; 
    for (; rightNum < levels.Length; leftNum++, rightNum++)
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