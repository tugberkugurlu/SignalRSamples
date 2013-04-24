namespace MultiLayerSignalRSample.Domain.Entities.Core
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}
