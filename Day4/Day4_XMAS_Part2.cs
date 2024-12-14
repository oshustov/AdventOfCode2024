namespace AdventOfCode2024.Day4;

public static class Day4_XMAS_Part2
{
  private record Position(int Row, int Col)
  {
    public static Func<Position, Position> NextDiagonalUp => x => x with { Row = x.Row - 1, Col = x.Col + 1 };
    public static Func<Position, Position> NextDiagonalDown => x => x with { Row = x.Row + 1, Col = x.Col + 1 };

    public bool IsInside<T>(List<List<T>> matrix) => 
      Row >= 0 && Row < matrix.Count &&
      Col >= 0 && Col < matrix[0].Count;
  }

  public static void Run()
  {
    using var file = File.Open(Path.Combine("Day4", "Input.txt"), FileMode.Open);
    using var reader = new StreamReader(file);

    var matrix = new List<List<string>>();
    var row = 0;
    while (!reader.EndOfStream)
    {
      var line = reader.ReadLine().ToCharArray().Select(x => x.ToString());
      if (matrix.ElementAtOrDefault(row) == default)
        matrix.Add(new List<string>());

      matrix[row].AddRange(line);
      row++;
    }

    var calculator = new XMasCalculator();
    var result = 0;

    for (var rowIndex = 0; rowIndex < matrix.Count; rowIndex++)
    for (var colIndex = 0; colIndex < matrix[rowIndex].Count; colIndex++)
        if (calculator.HasXShapedWord(rowIndex, colIndex, matrix))
          result++;

    Console.WriteLine($"{result}");
    Console.ReadLine();
  }

  public class XMasCalculator
  {
    public bool HasXShapedWord(int rowIndex, int colIndex, List<List<string>> matrix) =>
      AreLettersMatch(
        NextPositionsFrom(new Position(rowIndex, colIndex), Position.NextDiagonalDown, count: 2),
        matrix[rowIndex][colIndex],
        matrix) &&
      AreLettersMatch(
        NextPositionsFrom(new Position(rowIndex + 2, colIndex), Position.NextDiagonalUp, count: 2),
        matrix[rowIndex + 2][colIndex],
        matrix);

    private static Position[] NextPositionsFrom(Position pos, Func<Position, Position> next, int count) =>
      --count <= 0
        ? [next(pos)]
        : [next(pos), .. NextPositionsFrom(next(pos), next, count)];

    private bool AreLettersMatch(Position[] positions, string startLetter, List<List<string>> matrix)
    {
      var nextLetters = NextLettersFor(startLetter);
      if (nextLetters is not { Count: > 0 })
        return false;

      foreach (var position in positions)
      {
        if (!position.IsInside(matrix))
          return false;

        if (matrix[position.Row][position.Col] != nextLetters.Dequeue())
          return false;
      }

      return true;
    }

    private Queue<string> NextLettersFor(string letter) =>
      letter switch
      {
        "M" => new Queue<string>(["A", "S"]),
        "S" => new Queue<string>(["A", "M"]),
        _ => new Queue<string>()
      };
  }
}