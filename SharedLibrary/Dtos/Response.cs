﻿using System.Text.Json.Serialization;

namespace SharedLibrary.Dtos;

public class Response<T> where T : class
{
    public T? Data { get; private set; }
    public int StatusCode { get; private set; }
    public ErrorDto? Error { get; set; }

    [JsonIgnore]
    public bool IsSuccessfull { get; set; }

    public static Response<T> Success(T data, int statusCode)
    {
        return new Response<T>()
        {
            Data = data,
            StatusCode = statusCode,
            IsSuccessfull = true
        };
    }

    public static Response<T> Success(int statusCode)
    {
        return new Response<T>() { StatusCode = statusCode , IsSuccessfull = true};
    }

    public static Response<T> Fail(ErrorDto errors, int statusCode)
    {
        return new Response<T>() { Error = errors, StatusCode = statusCode, IsSuccessfull = false};
    }

    public static Response<T> Fail(int statusCode)
    {
        return new Response<T> { StatusCode = statusCode, IsSuccessfull = false};
    }

    public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
    {
        var errorDto = new ErrorDto(errorMessage, isShow);
        return new Response<T>()
        {
            Error = errorDto,
            StatusCode = statusCode,
            IsSuccessfull = false
        };
    }
}

