using ServiceStack;

namespace ServiceModel
{
    public class UploadFile
    {
        public string Name { get; set; }
    }

    public class UploadFileResponse : IReturn<UploadFile>
    {
        public string Name { get; set; }
        public long FileSize { get; set; }
    }
}