namespace MultiLayerSignalRSample.Domain.Services
{
    public class OperationResult<TEntity> : OperationResult
    {
        public OperationResult(bool isSuccess)
            : base(isSuccess) { }

        public TEntity Entity { get; set; }
    }
}
