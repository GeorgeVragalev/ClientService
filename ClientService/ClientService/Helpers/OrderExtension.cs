using ClientService.Models;

namespace ClientService.Helpers;

public static class OrderExtension
{
    private static IList<int> ratings = new List<int>();

    public static double GetOrderRating(this Order order)
    {
        var waitingTime = (DateTime.Now - order.PickUpTime);
        var timeElapsed = SetTimeElapsed(waitingTime);
        var rating = GetRating(timeElapsed, order.MaxWait);

        ratings.Add(rating);
        PrintConsole.Write($"Received rating {rating} from orderId {order.Id} {timeElapsed} | {order.MaxWait}", ConsoleColor.DarkGreen);

        return ratings.Average();
    }

    private static int SetTimeElapsed(TimeSpan time)
    {
        var timeElapsed = 0;

        switch (Settings.Settings.TimeUnit)
        {
            case 1:
                timeElapsed = (int) time.TotalMilliseconds;
                break;
            case 1000:
                timeElapsed = (int) time.TotalSeconds;
                break;
            case 60000:
                timeElapsed = (int) time.TotalMinutes;
                break;
        }

        return timeElapsed/4;
    }

    private static int GetRating(int timeElapsed, int maxWait)
    {
        var rating = 1;
        if (timeElapsed < maxWait)
        {
            rating = 5;
        }
        else if (timeElapsed < maxWait * 1.1)
        {
            rating = 4;
        }
        else if (timeElapsed < maxWait * 1.2)
        {
            rating = 3;
        }
        else if (timeElapsed < maxWait * 1.3)
        {
            rating = 2;
        }

        return rating;
    }
}