using CloneGame;
using System.Text;

var clones = Clone.CloneNames
    .Select(name => new Clone(name, Random.Shared.Next(1, Clone.MAX_ID))).ToList();
int selectedName = 0;
int selectedId = 0;
var startTime = DateTime.Now;
var lastAddTime = DateTime.Now;

Console.CursorVisible = false;
var buffer = new StringBuilder();

while (clones.Count > 0)
{
    buffer.Clear();

    buffer.Append("Name:\t");
    for (int i = 0; i < Clone.CloneNames.Count; i++)
    {
        var name = Clone.CloneNames[i];
        buffer.Append(i == selectedName ? name.ToUpper() : name).Append('\t');
    }

    buffer.Append("\nID:\t");
    for (int i = 0; i <= Clone.MAX_ID; i++)
    {
        buffer.Append(i == selectedId ? $">{i}<" : $"{i}").Append('\t');
    }

    buffer.AppendFormat($@"
Target Clones: left/right changes target name, up/down changes target ID, Enter to fire
Clones: {clones.Count}              Time:{(DateTime.Now - startTime).TotalSeconds:0.0}s
");

    int count = 0;
    foreach (var clone in clones)
    {
        buffer.Append(clone);
        count++;
        buffer.Append(count % 5 == 0 ? Environment.NewLine : "\t");
    }

    Console.SetCursorPosition(0, 0);
    Console.Write(buffer);

    int currentLine = Console.CursorTop;
    int totalLines = Console.WindowHeight;
    for (int i = currentLine; i < totalLines - 1; i++)
    {
        Console.Write(new string(' ', Console.WindowWidth));
    }
    Console.SetCursorPosition(0, currentLine);

    if (Console.KeyAvailable)
    {
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.LeftArrow:
                selectedName = selectedName == 0 ? Clone.CloneNames.Count - 1 : selectedName - 1;
                break;
            case ConsoleKey.RightArrow:
                selectedName = selectedName == Clone.CloneNames.Count - 1 ? 0 : selectedName + 1;
                break;
            case ConsoleKey.UpArrow:
                selectedId = selectedId == Clone.MAX_ID ? 0 : selectedId + 1;
                break;
            case ConsoleKey.DownArrow:
                selectedId = selectedId == 1 ? Clone.MAX_ID : selectedId - 1;
                break;
            case ConsoleKey.Enter:
                var target = new Clone(Clone.CloneNames[selectedName], selectedId);
                clones.Remove(target);
                break;
        }
    }

    if ((DateTime.Now - lastAddTime).TotalSeconds > Clone.ADD_SECONDS && clones.Count < 99)
    {
        var newCloneSelection = clones[Random.Shared.Next(clones.Count)];
        clones.Add(newCloneSelection with { Id = Random.Shared.Next(1, Clone.MAX_ID + 1) });
        lastAddTime = DateTime.Now;
    }

    Thread.Sleep(Clone.FRAME_DELAY_MILLISECONDS);
}

Console.CursorVisible = true;
Console.WriteLine($"Completed in {(DateTime.Now - startTime).TotalSeconds:0.0} seconds");