using System.Text;

namespace AdventOfCode2024.Day3;

public static class Day3_MulInstructions
{
  public static void Run()
  {
    using var file = File.Open(Path.Combine("Day3", "Input.txt"), FileMode.Open);
    using var reader = new StreamReader(file);

    var match = new MulOperation();
    var doDontModifier = new DoDontModifier();

    var total = 0;
    while (!reader.EndOfStream)
    {
      var input = new StringBuilder();
      var symbol = (char) reader.Read();
      input.Append(symbol);

      while (char.IsNumber(symbol) && char.IsNumber((char)reader.Peek()))
        input.Append((char)reader.Read());

      var stringInput = input.ToString();

      doDontModifier.CaptureInput(stringInput);

      var result = match.TryComplete(stringInput);
      if (result is not null && doDontModifier.IsMulEnabled)
      {
        total += MulOperationResult.Calculate(result);
        match.Reset();
      }
    }

    Console.WriteLine(total);
    Console.ReadLine();
  }

  public class MulOperation
  {
    private readonly Func<string, bool>[] _matchers;
    private int _index;

    private readonly StringBuilder _buffer;

    public MulOperation()
    {
      _index = 0;
      _buffer = new StringBuilder();

      _matchers =
      [
        x => x == "m",
        x => x == "u",
        x => x == "l",
        x => x == "(",
        x => long.TryParse(x, out _),
        x => x == ",",
        x => long.TryParse(x, out _),
        x => x == ")"
      ];
    }

    public string? TryComplete(string input)
    {
      var hasMatch = _matchers[_index];
      if (hasMatch(input))
      {
        _buffer.Append(input);

        if (_index == _matchers.Length - 1)
        {
          var result = _buffer.ToString();
          Reset();
          return result;
        }

        _index++;
        return null;
      }

      Reset();
      return null;
    }

    public void Reset()
    {
      _index = 0;
      _buffer.Clear();
    }
  }

  public class DoDontModifier
  {
    public bool IsMulEnabled { get; private set; } = true;

    private string _doBuffer;
    private string _dontBuffer;

    private Func<string, string, string, bool> IsPartOf => 
      (target, input, source) => target.StartsWith(source + input);

    public void CaptureInput(string input)
    {
      if (IsPartOf("do()", input, _doBuffer))
      {
        _doBuffer += input;
        if (_doBuffer == "do()")
        {
          IsMulEnabled = true;
          _doBuffer = string.Empty;
        }
      }
      else
      {
        _doBuffer = string.Empty;
      }

      if (IsPartOf("don't()", input, _dontBuffer))
      {
        _dontBuffer += input;
        if (_dontBuffer == "don't()")
        {
          IsMulEnabled = false;
          _dontBuffer = string.Empty;
        }
      }
      else
      {
        _dontBuffer = string.Empty;
      }
    }
  }

  public static class MulOperationResult
  {
    public static int Calculate(string operation)
    {
      var operands = operation
        .Replace("mul(", "")
        .Replace(")", "")
        .Split(',')
        .Select(int.Parse)
        .ToArray();

      return operands[0] * operands[1];
    }
  }
}