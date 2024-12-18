using pentasharp.Models.Enums;

namespace pentasharp.Models.DTOs
{
    /// <summary>
    /// Represents a standardized API response structure.
    /// </summary>
    /// <typeparam name="T">The type of data returned in the response.</typeparam>
    public class StandardApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the API operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Provides a descriptive message about the API operation result.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Represents the response code indicating the status of the operation.
        /// </summary>
        public ResponseCodes Code { get; set; }

        /// <summary>
        /// Contains the data returned by the API operation, if applicable.
        /// </summary>
        public T Data { get; set; }
    }
}