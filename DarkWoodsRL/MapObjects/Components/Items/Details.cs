using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components.Items;

public class Details : RogueLikeComponentBase<RogueLikeEntity>
{
    public string Name;
    public string Type;
    public string Description;
    protected Details(string name, string type, string description) : base(false, false, false, false)
    {
        Name = name;
        Type = type;
        Description = description;
    }
}