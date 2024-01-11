namespace PvPet.Business.DTOs;

public abstract class ItemBaseDto
{
    private const int WeaponNumVariants = 13;
    private const int PotionNumVariants = 15;
    private const int FoodNumVariants = 16;
    private const int ArmorNumVariants = 15;

    public Guid Id { get; set; }

    public ItemTypeDto? Type { get; set; }

    public int? Pictocode { get; set; }

    public int? Attack { get; set; }

    public int? Armor { get; set; }

    public double? AttackSpeed { get; set; }

    public int? Crit { get; set; }

    public int? Hp { get; set; }

    public int? Food { get; set; }

    public int? Gold { get; set; }

    public static T New<T>() where T : ItemBaseDto
    {
        var isFromShop = typeof(T) == typeof(ShopItemDto);
        var item = Activator.CreateInstance<T>();

        var itemTypes = Enum.GetValues(typeof(ItemTypeDto)).Cast<ItemTypeDto>();
        if (isFromShop)
            itemTypes = itemTypes.Where(i => i != ItemTypeDto.Gold);
        else
            itemTypes = itemTypes.Where(i => i != ItemTypeDto.Food && i != ItemTypeDto.Potion);
        var type = itemTypes.ElementAt(Random.Shared.Next(itemTypes.Count()));

        item.Id = Guid.NewGuid();
        item.Type = type;

        switch (type)
        {
            case ItemTypeDto.Gold: item.Gold = Random.Shared.Next(10, 251); break;
            case ItemTypeDto.Armor: item.Armor = Random.Shared.Next(1, 21); item.Pictocode = Random.Shared.Next(ArmorNumVariants) + 1; break;
            case ItemTypeDto.Food: item.Food = Random.Shared.Next(10, 76); item.Pictocode = Random.Shared.Next(FoodNumVariants) + 1; break;
            case ItemTypeDto.Potion: item.Hp = Random.Shared.Next(10, 76); item.Pictocode = Random.Shared.Next(PotionNumVariants) + 1; break;
            case ItemTypeDto.Weapon:
                {
                    item.Attack = Random.Shared.Next(1, 31);
                    item.AttackSpeed = Random.Shared.Next(1, 21) / 10.0;
                    item.Crit = Random.Shared.Next(1, 11);
                    item.Pictocode = Random.Shared.Next(WeaponNumVariants) + 1;
                    break;
                }
        };

        return item;
    }
}

public enum ItemTypeDto
{
    Gold, Weapon, Armor, Food, Potion
}
