namespace AdventOfCode2024.Day5.V2;

public static class Day5_SafetyManual_V2
{
  public static void Run()
  {
    var file = File.OpenRead(Path.Combine("Day5", "Input.txt"));
    var reader = new StreamReader(file);

    var rules = new HashSet<(int, int)>();
    var updates = new List<int[]>();
    while (!reader.EndOfStream)
    {
      var kind = MatchKind(reader.ReadLine());
      if (kind is { Update: not null })
        updates.Add(kind.Update.Pages);

      if (kind is not {Rule: not null})
        continue;

      rules.Add((kind.Rule.Number, kind.Rule.ShouldBeBeforeNumber));
    }

    var validUpdates = new List<int[]>();
    foreach (var update in updates)
    {
      var isValid = true;
      var firstNum = 0;
      var secondNum = 1;
      for (; secondNum < update.Length; firstNum++, secondNum++)
      {
        var invalidOrderRule = (update[secondNum], (update[firstNum]));
        if (!rules.Contains(invalidOrderRule))
          continue;

        isValid = false;
        break;
      }

      if (isValid)
        validUpdates.Add(update);
    }

    var result = validUpdates.Sum(x => x.ElementAt((x.Length / 2)));

    Console.WriteLine($"Rules count: {rules.Count}");
    Console.WriteLine($"Updates count: {updates.Count}");
    Console.WriteLine($"Valid updates count: {validUpdates.Count}");
    Console.WriteLine($"Result sum is: {result}");
    Console.ReadLine();
  }

  public static (Rule? Rule, Update? Update) MatchKind(string self) =>
    self switch
    {
      not null when self.Contains('|') => (Rule.From(self.Split("|").ToArray()), default),
      not null when self.Contains(',') => (default, Update.From(self.Split(","))),
      _ => default
    }; 
}

public record Rule(int Number, int ShouldBeBeforeNumber)
{
  public static Rule From(string[] parts)
  {
    if (parts.Length < 2)
      throw new ArgumentException();

    return new Rule(int.Parse(parts[0]), int.Parse(parts[1]));
  }
}

public record Update(int[] Pages)
{
  public static Update From(string[] parts)
  {
    return new Update(parts.Select(int.Parse).ToArray());
  }
}