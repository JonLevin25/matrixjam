namespace MatrixJam.Team17
{
    public interface IPlayerInventoryItem : IPlayerOwned
    {
        bool IsPassive { get; }
    }
}