// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.Common.Api;

namespace Cencora.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for converting exceptions to ApiResponse objects.
    /// </summary>
    public static class ApiResponseExtensions
    {
        /// <summary>
        /// Converts the specified exception to an ApiResponse object with a default status code of 500.
        /// </summary>
        /// <param name="exception">The exception to convert.</param>
        /// <returns>An ApiResponse object representing the exception.</returns>
        public static ApiResponse ToApiResponse(this Exception exception)
        {
            return ApiResponse.Error(500, exception.Message);
        }

        /// <summary>
        /// Converts the specified exception to an ApiResponse object with the specified status code.
        /// </summary>
        /// <param name="exception">The exception to convert.</param>
        /// <param name="statusCode">The status code to use in the ApiResponse object.</param>
        /// <returns>An ApiResponse object representing the exception.</returns>
        public static ApiResponse ToApiResponse(this Exception exception, int statusCode)
        {
            return ApiResponse.Error(statusCode, exception.Message);
        }

        /// <summary>
        /// Converts the specified exception to an ApiResponse object with a default status code of 500.
        /// </summary>
        /// <typeparam name="T">The type of the data in the ApiResponse object.</typeparam>
        /// <param name="exception">The exception to convert.</param>
        /// <returns>An ApiResponse object representing the exception.</returns>
        public static ApiResponse<T> ToApiResponse<T>(this Exception exception)
        {
            return ApiResponse<T>.Error(500, exception.Message);
        }

        /// <summary>
        /// Converts the specified exception to an ApiResponse object with the specified status code.
        /// </summary>
        /// <typeparam name="T">The type of the data in the ApiResponse object.</typeparam>
        /// <param name="exception">The exception to convert.</param>
        /// <param name="statusCode">The status code to use in the ApiResponse object.</param>
        /// <returns>An ApiResponse object representing the exception.</returns>
        public static ApiResponse<T> ToApiResponse<T>(this Exception exception, int statusCode)
        {
            return ApiResponse<T>.Error(statusCode, exception.Message);
        }
    }
}