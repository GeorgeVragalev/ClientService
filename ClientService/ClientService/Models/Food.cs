namespace ClientService.Models;

public class Food : BaseEntity
{
    public string Name { get; set; }
    public int PreparationTime { get; set; }
}