// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Net;

namespace Cencora.Common.Api;

/// <summary>
/// Provides utility for HTTP-related operations.
/// </summary>
public static class HttpUtils
{
    /// <summary>
    /// Determines whether the specified status code is a valid HTTP status code.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidHttpStatusCode(int statusCode)
    {
        return statusCode is >= 100 and < 600;
    }

    /// <summary>
    /// Determines whether the specified status code indicates a successful response.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code indicates success; otherwise, <c>false</c>.</returns>
    public static bool IsSuccessStatusCode(int statusCode)
    {
        return statusCode is >= 200 and < 300;
    }

    /// <summary>
    /// Determines whether the specified status code indicates a redirection.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code indicates redirection; otherwise, <c>false</c>.</returns>
    public static bool IsRedirectStatusCode(int statusCode)
    {
        return statusCode is >= 300 and < 400;
    }

    /// <summary>
    /// Determines whether the specified status code indicates a client error.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code indicates a client error; otherwise, <c>false</c>.</returns>
    public static bool IsClientErrorStatusCode(int statusCode)
    {
        return statusCode is >= 400 and < 500;
    }

    /// <summary>
    /// Determines whether the specified status code indicates a server error.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code indicates a server error; otherwise, <c>false</c>.</returns>
    public static bool IsServerErrorStatusCode(int statusCode)
    {
        return statusCode is >= 500 and < 600;
    }

    /// <summary>
    /// Determines whether the specified status code indicates an informational response.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code indicates an informational response; otherwise, <c>false</c>.</returns>
    public static bool IsInformationalStatusCode(int statusCode)
    {
        return statusCode is >= 100 and < 200;
    }

    /// <summary>
    /// Determines whether the specified status code indicates "Not Found" (404).
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code is 404; otherwise, <c>false</c>.</returns>
    public static bool IsNotFound(int statusCode)
    {
        return statusCode == 404;
    }

    /// <summary>
    /// Determines whether the specified status code indicates "Unauthorized" (401).
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code is 401; otherwise, <c>false</c>.</returns>
    public static bool IsUnauthorized(int statusCode)
    {
        return statusCode == 401;
    }

    /// <summary>
    /// Determines whether the specified status code indicates "Forbidden" (403).
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code is 403; otherwise, <c>false</c>.</returns>
    public static bool IsForbidden(int statusCode)
    {
        return statusCode == 403;
    }

    /// <summary>
    /// Determines whether the specified status code indicates "Internal Server Error" (500).
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns><c>true</c> if the status code is 500; otherwise, <c>false</c>.</returns>
    public static bool IsInternalServerError(int statusCode)
    {
        return statusCode == 500;
    }

    /// <summary>
    /// Converts the specified HTTP status code to an instance of <see cref="HttpStatusCode"/>.
    /// </summary>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <returns>An instance of <see cref="HttpStatusCode"/> representing the specified status code.</returns>
    public static HttpStatusCode GetHttpStatusCode(int statusCode)
    {
        if (!IsValidHttpStatusCode(statusCode))
        {
            throw new ArgumentException("Invalid status code.", nameof(statusCode));
        }
            
        return (HttpStatusCode)statusCode;
    }

    /// <summary>
    /// Converts an <see cref="HttpStatusCode"/> to an integer representation.
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> to convert.</param>
    /// <returns>An integer representation of the <see cref="HttpStatusCode"/>.</returns>
    public static int HttpStatusCodeToInt(HttpStatusCode statusCode)
    {
        return (int)statusCode;
    }
}