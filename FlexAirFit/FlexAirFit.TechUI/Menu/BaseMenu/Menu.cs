using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.BaseMenu;

public class Menu
{
    List<MenuLabel> _labels = [];

    public void AddLabel(MenuLabel label)
    {
        _labels.Add(label);
    }

    public async Task<int> Execute(Context context)
    {
        int iitem = 1;
        foreach (var l in _labels)
        {
            Console.Write($"[{iitem++}] ");
            l.Print();
        }
        Console.WriteLine("[0] Выход");
        Console.Write("Ввод: ");
        if (!int.TryParse(Console.ReadLine(), out int no))
        {
            Console.WriteLine("[!] Error");
            return -1;
        }

        if (no == 0)
        {
            return 0;
        }
        if (0 > no || no > _labels.Count)
        {
            Console.WriteLine("[!] Команды с таким номером нет");
            return -1;
        }
        await _labels[no - 1].Execute(context);
        return no;
    }
}