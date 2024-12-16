using pentasharp.Models.DTOs;
using pentasharp.Models.Enums;

namespace pentasharp.Models.Utilities
{
    public static class ResponseFactory
    {
        public static StandardApiResponse<T> CreateResponse<T>(ResponseCodes code, string message, T data)
        {
            return new StandardApiResponse<T>
            {
                Success = code == ResponseCodes.Success,
                Message = message,
                Code = code,
                Data = data
            };
        }
    }
}