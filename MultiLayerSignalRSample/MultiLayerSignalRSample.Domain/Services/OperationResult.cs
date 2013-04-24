namespace MultiLayerSignalRSample.Domain.Services
{
    public class OperationResult
    {
        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; private set; }
    }
}
