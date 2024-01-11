namespace PvPet.Business.DTOs;

public class ItemOnMapDto : ItemBaseDto
{
    public LocationDto? Location { get; set; }

    public int? SecondsLeft { get; set; }

    public static ItemOnMapDto New()
    {
        var item = ItemBaseDto.New<ItemOnMapDto>();
        item.Location = new LocationDto
        {
            Latitude = Random.Shared.NextDouble() * (45.771 - 45.729) + 45.729,
            Longitude = Random.Shared.NextDouble() * (21.261 - 21.191) + 21.261
        };
        item.SecondsLeft = Random.Shared.Next(300, 3600);

        return item;
    }
}
