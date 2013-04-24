using System;

namespace MultiLayerSignalRSample.API.Model.RequestModels
{
    public class ChatMessageRequestModel
    {
        public string Message { get; set; }
        public DateTimeOffset ReceivedOn { get; set; }
    }
}
