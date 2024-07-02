using EasyTestMaker.Constants;
using EasyTestMaker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EasyTestMaker.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService()
        {
            _httpClient = new HttpClient();
        }
       
        // This method is used to authenticate the user by sending a POST request to the specified API endpoint.
        // It takes a User object as input and returns a boolean value indicating whether the login was successful or not.
        // It uses the HttpClient class to make the HTTP request.
        // If the response status code is successful, it stores the user's ID and username in the Storage class and returns true.
        // Otherwise, it returns false.
        public async Task<bool> Login(User user)
        {
            var response = await _httpClient.PostAsJsonAsync<User>($"http://localhost:7129/api/Authentication", user);
            var json = await response.Content.ReadAsStringAsync();
            var responseUser = JsonConvert.DeserializeObject<User>(json);

            if (response.IsSuccessStatusCode)
            {
                Const.UserId = responseUser.Id;
                Const.Username = responseUser.Username;
                Const.IsLector = responseUser.IsLector;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CreateUser(User user)
        {
            var response = await _httpClient.PutAsJsonAsync<User>($"http://localhost:7129/api/Authentication", user);
            var json = await response.Content.ReadAsStringAsync();
            var responseUser = JsonConvert.DeserializeObject<User>(json);

            if (response.IsSuccessStatusCode)
            {
               
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
