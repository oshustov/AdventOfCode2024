using System.Data.Common;
using System.Text;

namespace AdventOfCode2024.Day4;

public static class Day4_XMAS
{
  private record Position(int Row, int Col)
  {
    public static Func<Position, Position> NextRight => x => x with { Col = x.Col + 1 };
    public static Func<Position, Position> NextDown => x => x with { Row = x.Row + 1 };
    public static Func<Position, Position> NextDiagonalUp => x => x with { Row = x.Row - 1, Col = x.Col + 1 };
    public static Func<Position, Position> NextDiagonalDown => x => x with { Row = x.Row + 1, Col = x.Col + 1 };
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
    {
      for (var colIndex = 0; colIndex < matrix[rowIndex].Count; colIndex++)
      {
        var letter = matrix[rowIndex][colIndex];
        if (letter != "X" && letter != "S")
          continue;

        result += calculator.CountWordsStartingAt(rowIndex, colIndex, matrix);
      }

      Console.WriteLine($"Result after {rowIndex} row: {result}");
    }

    Console.WriteLine($"{result}");
    Console.ReadLine();
  }

  public class XMasCalculator
  {
    private static Position[] CalcNextPositionsFrom(Position pos, Func<Position, Position> next, int count) =>
      --count <= 0
        ? [next(pos)]
        : [next(pos), .. CalcNextPositionsFrom(next(pos), next, count)];

    private IEnumerable<Position[]> LookupRight(Position pos)
    {
      yield return CalcNextPositionsFrom(pos, Position.NextRight, count: 3);
      yield return CalcNextPositionsFrom(pos, Position.NextDown, count: 3);
      yield return CalcNextPositionsFrom(pos, Position.NextDiagonalUp, count: 3);
      yield return CalcNextPositionsFrom(pos, Position.NextDiagonalDown, count: 3);
    }

    public int CountWordsStartingAt(int rowIndex, int colIndex, List<List<string>> matrix)
    {
      var result = 0;

      foreach (var nextPositions in LookupRight(new Position(rowIndex, colIndex)))
      {
        var letters = NextLettersFor(matrix[rowIndex][colIndex]);
        foreach (var (row, col) in nextPositions)
        {
          if (row >= matrix.Count || row < 0 || col >= matrix[0].Count || col < 0)
            break;

          var next = letters.Peek();
          if (matrix[row][col] != next)
            break;

          letters.Dequeue();
        }

        if (letters.Count == 0)
          result++;
      }

      return result;
    }

    private Queue<string> NextLettersFor(string letter) =>
      letter switch
      {
        "X" => new Queue<string>(["M", "A", "S"]),
        "S" => new Queue<string>(["A", "M", "X"]),
        _ => new Queue<string>()
      };
  }
}