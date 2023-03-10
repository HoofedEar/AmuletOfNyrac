using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace DarkWoodsRL.MapObjects.Components.Items;

public class DetailsComponent : RogueLikeComponentBase<RogueLikeEntity>
{
    public string Type;
    public string[] Description;

    public DetailsComponent(string type, string[] description) : base(false, false, false, false)
    {
        Type = type;
        Description = description;
    }
}