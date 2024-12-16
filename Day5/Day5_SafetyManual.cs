namespace AdventOfCode2024.Day5;

public static class Day5_SafetyManual
{
  public static void Run()
  {
    var file = File.OpenRead(Path.Combine("Day5", "Input.txt"));
    var reader = new StreamReader(file);

    var rules = new Dictionary<int, List<int>>();
    var updates = new List<Dictionary<int, int>>();
    while (!reader.EndOfStream)
    {
      var kind = MatchKind(reader.ReadLine());
      if (kind is { Update: not null })
        updates.Add(kind.Update.Pages
          .Select((page, index) => (page, index))
          .ToDictionary(tuple => tuple.page, tuple => tuple.index));

      if (kind is not {Rule: not null})
        continue;

      if (!rules.ContainsKey(kind.Rule.Number))
        rules.Add(kind.Rule.Number, []);

      rules[kind.Rule.Number].Add(kind.Rule.ShouldBeBeforeNumber);
    }

    var validUpdates = new List<int[]>();
    foreach (var updateDictionary in updates)
    {
      var isValid = true;
      foreach (var (pageNumber, pageIndex) in updateDictionary)
      {
        var pageRules = rules.GetValueOrDefault(pageNumber, []);
        if (pageRules.Any(x => updateDictionary.GetValueOrDefault(x, int.MaxValue) < pageIndex))
        {
          isValid = false;
          break;
        }
      }

      if (isValid)
        validUpdates.Add(CopyToArray(updateDictionary));
    }

    var result = validUpdates.Sum(x => x.ElementAt((x.Length / 2)));

    Console.WriteLine($"Rules count: {rules.Count}");
    Console.WriteLine($"Updates count: {updates.Count}");
    Console.WriteLine($"Invalid updates count: {validUpdates.Count}");
    Console.WriteLine($"Result sum is: {result}");
    Console.ReadLine();
  }

  public static int[] CopyToArray(Dictionary<int, int> dictionary)
  {
    var array = new int[dictionary.Count];
    foreach (var (page, index) in dictionary)
      array[index] = page;

    return array;
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