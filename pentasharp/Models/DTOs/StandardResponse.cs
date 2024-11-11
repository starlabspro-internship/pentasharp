using pentasharp.Models.Enums;

namespace pentasharp.Models.DTOs
{
    /// <summary>
    /// Represents a standard response structure for API responses.
    /// </summary>
    public class StandardResponse
    {
        /// <summary>
        /// The status of the API response, indicating success, failure, or other status.
        /// </summary>
        public ApiStatusEnum Status { get; set; }

        /// <summary>
        /// A unique identifier for the response, typically used for tracking or logging.
        /// </summary>
        public string UUID { get; set; }

        /// <summary>
        /// A message providing additional information about the response.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardResponse"/> class with specified status, UUID, and message.
        /// </summary>
        /// <param name="status">The status of the API response.</param>
        /// <param name="uuid">The unique identifier for the response.</param>
        /// <param name="message">The message associated with the response.</param>
        public StandardResponse(ApiStatusEnum status, string uuid, string message)
        {
            Status = status;
            UUID = uuid;
            Message = message;
        }
    }
}