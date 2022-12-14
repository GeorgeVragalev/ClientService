namespace ClientService.Models;
public class Order : BaseEntity
{
    public int ClientId { get; set; }
    public int RestaurantId { get; set; }
    public int Priority { get; set; }
    public int MaxWait { get; set; }
    public DateTime PickUpTime { get; set; }
    public IList<int> Foods { get; set; }

    public Order() { }
}