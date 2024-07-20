// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Net;
using System.Text.Json.Serialization;

namespace Cencora.Common.Api;

/// <summary>
/// Represents an API response without a payload.
/// </summary>
[JsonConverter(typeof(ApiResponseConverter))]
public readonly struct ApiResponse : IEquatable<ApiResponse>
{
    /// <summary>
    /// Gets the status code of the response.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Gets the error message of the response.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Determines whether the response is successful.
    /// </summary>
    public bool IsSuccess => HttpUtils.IsSuccessStatusCode(StatusCode);

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse"/> struct.
    /// </summary>
    /// <param name="statusCode">The status code of the response.</param>
    /// <param name="errorMessage">The error message of the response.</param>
    /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
    private ApiResponse(int statusCode, string? errorMessage)
    {
        if (!HttpUtils.IsValidHttpStatusCode(statusCode))
        {
            throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code is not a valid HTTP status code.");
        }

        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponse"/> struct that represents a successful response.
    /// </summary>
    /// <param name="statusCode">The status code of the response. Default to 200</param>
    /// <returns>A new instance of the <see cref="ApiResponse"/> struct that represents a successful response.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The status code does not indicate success.</exception>
    public static ApiResponse Success(int statusCode = 200)
    {
        if (!HttpUtils.IsSuccessStatusCode(statusCode))
        {
            throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code does not indicate success.");
        }

        return new ApiResponse(statusCode, null);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponse"/> struct that represents a successful response.
    /// </summary>
    /// <param name="statusCode">The status code of the response.</param>
    /// <returns>A new instance of the <see cref="ApiResponse"/> struct that represents a successful response.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The status code does not indicate success.</exception>
    public static ApiResponse Success(HttpStatusCode statusCode)
    {
        return Success(HttpUtils.HttpStatusCodeToInt(statusCode));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponse"/> struct that represents an error response.
    /// </summary>
    /// <param name="statusCode">The status code of the response.</param>
    /// <param name="errorMessage">The error message of the response.</param>
    /// <returns>A new instance of the <see cref="ApiResponse"/> struct that represents an error response.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The status code indicates success.</exception>
    public static ApiResponse Error(int statusCode, string? errorMessage)
    {
        if (HttpUtils.IsSuccessStatusCode(statusCode))
        {
            throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code indicates success.");
        }

        return new ApiResponse(statusCode, errorMessage);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponse"/> struct that represents an error response.
    /// </summary>
    /// <param name="statusCode">The status code of the response.</param>
    /// <param name="errorMessage">The error message of the response.</param>
    /// <returns>A new instance of the <see cref="ApiResponse"/> struct that represents an error response.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The status code indicates success.</exception>
    public static ApiResponse Error(HttpStatusCode statusCode, string? errorMessage)
    {
        return Error(HttpUtils.HttpStatusCodeToInt(statusCode), errorMessage);
    }

    /// <summary>
    /// Matches the response against the specified actions.
    /// </summary>
    /// <param name="success">The action to execute if the response is successful.</param>
    /// <param name="error">The action to execute if the response is an error.</param>
    /// <exception cref="ArgumentNullException">The success action is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">The error action is <c>null</c>.</exception>
    public void Match(Action<int> success, Action<int, string?> error)
    {
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(error);

        if (IsSuccess)
        {
            success(StatusCode);
        }
        else
        {
            error(StatusCode, ErrorMessage ?? string.Empty);
        }
    }

    /// <summary>
    /// Matches the response against the specified functions.
    /// </summary>
    /// <typeparam name="TR">The return type of the functions.</typeparam>
    /// <param name="success">The function to execute if the response is successful.</param>
    /// <param name="error">The function to execute if the response is an error.</param>
    /// <returns>The result of the executed function.</returns>
    /// <exception cref="ArgumentNullException">The success function is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">The error function is <c>null</c>.</exception>
    public TR Match<TR>(Func<int, TR> success, Func<int, string?, TR> error)
    {
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(error);

        return IsSuccess ? success(StatusCode) : error(StatusCode, ErrorMessage);
    }

    /// <inheritdoc/>
    public bool Equals(ApiResponse other)
    {
        return StatusCode == other.StatusCode && ErrorMessage == other.ErrorMessage;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ApiResponse other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(StatusCode, ErrorMessage);
    }

    public static bool operator ==(ApiResponse left, ApiResponse right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ApiResponse left, ApiResponse right)
    {
        return !left.Equals(right);
    }
}

/// <summary>
/// Represents an API response with a payload.
/// </summary>
[JsonConverter(typeof(PayloadApiResponseConverterFactory))]
public readonly struct ApiResponse<T> : IEquatable<ApiResponse<T>>
{
    /// <summary>
    /// The payload of the response.
    /// </summary>
    private readonly T? _payload;

    /// <summary>
    /// Gets the status code of the response.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Determines whether the response is successful.
    /// </summary>
    public bool IsSuccess => HttpUtils.IsSuccessStatusCode(StatusCode) && _payload != null;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> struct.
    /// </summary>
    /// <param name="payload">The payload of the response.</param>
    /// <param name="statusCode">The status code of the response.</param>
    /// <param name="errorMessage">The error message of the response.</param>
    /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
    private ApiResponse(T? payload, int statusCode, string? errorMessage)
    {
        if (!HttpUtils.IsValidHttpStatusCode(statusCode))
        {
            throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code is not a valid HTTP status code.");
        }

        StatusCode = statusCode;
        ErrorMessage = errorMessage;
        _payload = payload;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponse{T}"/> struct that represents a successful response.
    /// </summary>
    /// <param name="payload">The payload of the response.</param>
    /// <param name="statusCode">The status code of the response.</param>
    /// <returns>A new instance of the <see cref="ApiResponse{T}"/> struct that represents a successful response.</returns>
    /// <exception cref="ArgumentNullException">The payload is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The status code does not indicate success.</exception>
    public static ApiResponse<T> Success(T payload, int statusCode = 200)
    {
        if (payload == null)
        {
            throw new ArgumentNullException(nameof(payload));
        }

        if (!HttpUtils.IsSuccessStatusCode(statusCode))
        {
            throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code does not indicate success.");
        }

        return new ApiResponse<T>(payload, statusCode, null);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponse{T}"/> struct that represents an error response.
    /// </summary>
    /// <param name="statusCode">The status code of the response.</param>
    /// <param name="errorMessage">The error message of the response.</param>
    /// <returns>A new instance of the <see cref="ApiResponse{T}"/> struct that represents an error response.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The status code indicates success.</exception>
    public static ApiResponse<T> Error(int statusCode, string? errorMessage)
    {
        if (HttpUtils.IsSuccessStatusCode(statusCode))
        {
            throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code indicates success.");
        }

        return new ApiResponse<T>(default, statusCode, errorMessage);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ApiResponse{T}"/> struct that represents an error response.
    /// </summary>
    /// <param name="statusCode">The status code of the response.</param>
    /// <param name="errorMessage">The error message of the response.</param>
    /// <returns>A new instance of the <see cref="ApiResponse{T}"/> struct that represents an error response.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The status code indicates success.</exception>
    public static ApiResponse<T> Error(HttpStatusCode statusCode, string? errorMessage)
    {
        return Error(HttpUtils.HttpStatusCodeToInt(statusCode), errorMessage);
    }

    /// <summary>
    /// Gets the payload of the response.
    /// </summary>
    /// <returns>The payload of the response.</returns>
    /// <exception cref="ArgumentNullException">The payload is <c>null</c>.</exception>
    public T Value
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_payload);
            return _payload;
        }
    }

    /// <summary>
    /// Gets the error message of the response.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Matches the response against the specified actions.
    /// </summary>
    /// <param name="success">The action to execute if the response is successful.</param>
    /// <param name="error">The action to execute if the response is an error.</param>
    /// <exception cref="ArgumentNullException">The success action is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">The error action is <c>null</c>.</exception>
    public void Match(Action<T, int> success, Action<int, string?> error)
    {
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(error);

        if (IsSuccess)
        {
            if (_payload == null)
            {
                throw new InvalidOperationException("The payload is null.");
            }
            success(_payload, StatusCode);
        }
        else
        {
            error(StatusCode, ErrorMessage);
        }
    }

    /// <summary>
    /// Matches the response against the specified functions.
    /// </summary>
    /// <typeparam name="TR">The return type of the functions.</typeparam>
    /// <param name="success">The function to execute if the response is successful.</param>
    /// <param name="error">The function to execute if the response is an error.</param>
    /// <returns>The result of the executed function.</returns>
    /// <exception cref="ArgumentNullException">The success function is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">The error function is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The payload is <c>null</c>.</exception>
    public TR Match<TR>(Func<T, int, TR> success, Func<int, string?, TR> error)
    {
        ArgumentNullException.ThrowIfNull(success);
        ArgumentNullException.ThrowIfNull(error);

        if (!IsSuccess) return error(StatusCode, ErrorMessage);
        // Should never happen, because IsSuccess checks for _payload != null
        // But we need to check it here to satisfy the compiler and to be sure.
        if (_payload == null)
        {
            throw new InvalidOperationException("The payload is null.");
        }
        return success(_payload, StatusCode);

    }

    /// <summary>
    /// Converts the payload of the current <see cref="ApiResponse{T}"/> to a new type using the specified payload converter function.
    /// </summary>
    /// <typeparam name="TR">The type to convert the payload to.</typeparam>
    /// <param name="payloadConverter">The function that converts the payload to the new type.</param>
    /// <returns>An <see cref="ApiResponse{R}"/> with the converted payload.</returns>
    /// <exception cref="InvalidOperationException">The payload converter is <c>null</c>. Only if the Response is success and the converter is null.</exception>
    public ApiResponse<TR> Into<TR>(Func<T, TR> payloadConverter)
    {
        ArgumentNullException.ThrowIfNull(payloadConverter);

        return Match(
            success: (payload, statusCode) => ApiResponse<TR>.Success(payloadConverter(payload), statusCode),
            error: ApiResponse<TR>.Error
        );
    }

    /// <summary>
    /// Unwraps the payload of the current <see cref="ApiResponse{T}"/>.
    /// </summary>
    /// <returns>The payload of the current <see cref="ApiResponse{T}"/>.</returns>
    /// <exception cref="InvalidOperationException">The payload is <c>null</c>.</exception>
    public T Unwrap()
    {
        if (_payload == null)
        {
            throw new InvalidOperationException("The payload is null.");
        }
        return _payload;
    }

    /// <inheritdoc/>
    public bool Equals(ApiResponse<T> other)
    {
        return StatusCode == other.StatusCode
               && ErrorMessage == other.ErrorMessage
               && EqualityComparer<T>.Default.Equals(_payload, other._payload);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ApiResponse<T> other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(StatusCode, ErrorMessage, _payload);
    }

    public static bool operator ==(ApiResponse<T> left, ApiResponse<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ApiResponse<T> left, ApiResponse<T> right)
    {
        return !left.Equals(right);
    }
}