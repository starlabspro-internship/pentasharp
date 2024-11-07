using pentasharp.Models.Enums;

namespace pentasharp.Models.DTOs
{
    public class StandardResponse
    {
        public ApiStatusEnum Status { get; set; }
        public string UUID { get; set; }
        public string Message { get; set; }

        public StandardResponse(ApiStatusEnum status, string uuid, string message)
        {
            Status = status;
            UUID = uuid;
            Message = message;
        }
    }
}