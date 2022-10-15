namespace ClientService.Helpers;

public static class IdGenerator
{
    private static Mutex _mutex = new();
    private static int _orderId = 0;
    private static int _groupOrderId = 0;
    private static int _clientId = 0;

    public static int GenerateOrderId()
    {
        _mutex.WaitOne();
        _orderId++;
        _mutex.ReleaseMutex();
        return _orderId;
    }

    public static int GenerateGroupOrderId()
    {
        _mutex.WaitOne();
        _groupOrderId++;
        _mutex.ReleaseMutex();
        return _groupOrderId;
    }

    public static int GenerateClientId()
    {
        _mutex.WaitOne();
        _clientId++;
        _mutex.ReleaseMutex();
        return _clientId;
    }
}