using Domain.Interfaces;
using Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Clients;

public class JsonPlaceholderClient : IJsonPlaceholderClient
{
    private HttpClient _httpClient;
    public JsonPlaceholderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<JsonPlaceholderResult<UserDto>> GetUserAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}");
        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadAsAsync<UserDto>();
            return new JsonPlaceholderResult<UserDto>
            {
                Data = user,
                IsSuccessful = true,
                ErrorMessage = ""
            };
        }
        else
        {
            return new JsonPlaceholderResult<UserDto>
            {
                IsSuccessful = false,
                ErrorMessage = response.StatusCode.ToString()
            };
        }

    }
}
