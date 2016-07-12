using ServiceStack;

namespace ServiceModel
{
    public class SendFile
    {
        public string Name { get; set; }
    }

    public class SendFileResponse : IReturn<SendFile>
    {
        public string Name { get; set; }
        public long FileSize { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}