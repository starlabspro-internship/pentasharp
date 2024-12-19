using pentasharp.Models.DTOs;
using pentasharp.Models.Enums;
using System;
using System.Collections.Generic;

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

        public static StandardApiResponse<T> SuccessResponse<T>(string message, T data = default)
        {
            return CreateResponse(ResponseCodes.Success, message, data);
        }

        public static StandardApiResponse<object> ErrorResponse(ResponseCodes code, string message)
        {
            return CreateResponse<object>(code, message, null);
        }

        public static StandardApiResponse<object> UnauthorizedResponse()
        {
            return ErrorResponse(ResponseCodes.Unauthorized, ResponseMessages.Unauthorized);
        }

        public static StandardApiResponse<object> ForbiddenResponse()
        {
            return ErrorResponse(ResponseCodes.Forbidden, ResponseMessages.Forbidden);
        }

        public static StandardApiResponse<object> ValidationErrorResponse(string message)
        {
            return ErrorResponse(ResponseCodes.InvalidData, string.IsNullOrEmpty(message) ? ResponseMessages.InvalidData : message);
        }

        public static StandardApiResponse<object> GeneralErrorResponse(ResponseCodes code)
        {
            string defaultMessage = code switch
            {
                ResponseCodes.InvalidData => ResponseMessages.InvalidData,
                ResponseCodes.Unauthorized => ResponseMessages.Unauthorized,
                ResponseCodes.Forbidden => ResponseMessages.Forbidden,
                ResponseCodes.NotFound => ResponseMessages.NotFound,
                ResponseCodes.Conflict => ResponseMessages.Conflict,
                ResponseCodes.InvalidOperation => ResponseMessages.InvalidOperation,
                _ => ResponseMessages.InternalServerError
            };

            return ErrorResponse(code, defaultMessage);
        }

        public static StandardApiResponse<object> NotFoundResponse(string message = null)
        {
            return ErrorResponse(ResponseCodes.NotFound, string.IsNullOrEmpty(message) ? ResponseMessages.NotFound : message);
        }

        public static StandardApiResponse<object> InvalidOperationResponse(string message = null)
        {
            return ErrorResponse(ResponseCodes.InvalidOperation, string.IsNullOrEmpty(message) ? ResponseMessages.InvalidOperation : message);
        }
    }
}