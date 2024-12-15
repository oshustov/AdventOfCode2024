namespace AdventOfCode2024.Day5;

public static class Day5_SafetyManual
{
  public static void Run()
  {
    var file = File.OpenRead(Path.Combine("Day5", "Input.txt"));
    var reader = new StreamReader(file);

    var rules = new List<Rule>();
    var updates = new List<Update>();
    while (!reader.EndOfStream)
    {
      Action action = MatchKind(reader.ReadLine()) switch
      {
        { Rule: not null } x => () => rules.Add(x.Rule),
        { Update: not null } x => () => updates.Add(x.Update),
        _ => () => { }
      };
      action.Invoke();
    }

    foreach (var update in updates)
    {
      var 
    }
    
    Console.WriteLine($"Rules count: {rules.Count}");
    Console.WriteLine($"Updates count: {updates.Count}");
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

public class RulesTree
{
  public record Node(int Num, Node[] Next);
  
  public Node Root { get; set; }

  public int[] GetBeforeNumbers(int number)
  {
    if (Root.Num == number)
      return [];
    
    var current = Root;
    while (current.Next != null)
    {
      foreach (var nextNode in current.Next)
      {
        //todo
      }
    }
  }
}