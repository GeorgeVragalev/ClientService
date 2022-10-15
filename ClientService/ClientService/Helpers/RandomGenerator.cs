namespace ClientService.Helpers;

public static class RandomGenerator
{
    public static int NumberGenerator(int max)
    {
        return Random.Shared.Next(1, max);
    }
    
    public static int IndexGenerator(int max)
    {
        return Random.Shared.Next(0, max);
    }
}