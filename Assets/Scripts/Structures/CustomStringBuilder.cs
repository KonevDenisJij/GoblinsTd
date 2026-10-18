using System.Collections.Generic;

public class CustomStringBuilder
{
    public Dictionary<int, string> Text => _text;
    private Dictionary<int, string> _text;

    public CustomStringBuilder()
    {
        _text = new Dictionary<int, string>();
    }

    public void AppendLine(string newLine, int index)
    {
        _text[index] = newLine + "\n";
    }

    public override string ToString()
    {
        string result = "";

        foreach(var str in _text)
        {
            result += str.Value;
        }

        return result;
    }
}
