﻿namespace PvPet.Data.Entities;

public class ShopItem : ItemBase
{
    public int Price { get; set; }

    public Guid PetId { get; set; }

    public Pet Pet { get; set; } = null!;
}
