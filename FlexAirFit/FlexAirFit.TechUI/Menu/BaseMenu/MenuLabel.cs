using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.BaseMenu;

public class MenuLabel(string announce, List<Command> commands)
{
    string _announce = announce;
    List<Command> _commands = commands;

    public void Print()
    {
        Console.WriteLine(_announce);
        if (_commands.Count <= 1)
        {
            return;
        }
        for (int i = 0; i < _commands.Count; ++i)
        {
            if (i == _commands.Count - 1)
            {
                Console.WriteLine($" └─ {_commands[i].Description()}");
            }
            else
            {
                Console.WriteLine($" ├─ {_commands[i].Description()}");
            }
        }
    }

    public async Task Execute(Context context)
    {
        if (_commands.Count == 1)
        {
            await _commands.First().Execute(context);
            return;
        }

        Console.WriteLine($"============ {_announce} ============");
        int iitem = 1;
        foreach (var c in _commands)
        {
            Console.WriteLine($"{iitem++}. {c.Description()}");
        }
        Console.Write("Ввод: ");
        if (!int.TryParse(Console.ReadLine(), out int no))
        {
            Console.WriteLine("[!] Error");
            return;
        }
        if (0 >= no || no > _commands.Count)
        {
            Console.WriteLine("[!] Error");
            return;
        }
        await _commands[no - 1].Execute(context);
    }
}