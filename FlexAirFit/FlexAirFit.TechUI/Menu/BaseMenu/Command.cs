using FlexAirFit.TechUI.BaseMenu;

namespace FlexAirFit.TechUI.BaseMenu;

abstract public class Command
{
    abstract public Task Execute(Context context);
    abstract public string? Description();
}