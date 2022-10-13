namespace ClientService.Models;

public class GroupOrder : BaseEntity
{
    public IList<Order> GroupedOrder { get; set; }
}