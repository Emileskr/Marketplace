﻿
namespace Domain.Models.DTOs;

public class JsonPlaceholderResult<T> where T : class
{
    public bool IsSuccessful { get; set; }

    public string? ErrorMessage { get; set; }

    public T? Data { get; set; }
}
