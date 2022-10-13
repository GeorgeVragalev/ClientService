namespace ClientService.Models;

public class RestaurantData : BaseEntity
{
    public IList<Food> Menu { get; set; }

    public RestaurantData(IList<Food> menu)
    {
        Menu = menu;
    }
}