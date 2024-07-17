namespace TrackOrders.Services.Interfaces
{
    public interface IPasswordHashService
    {
        string Hash(string password);

        (bool verified, bool needsUpgrade) Check(string hash, string password);
    }
}
