using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyQuiz.Models.DTO;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;
using EazyQuiz.Cryptography;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using UnityEditor.PackageManager;
using Unity.VisualScripting;
using Newtonsoft.Json;

namespace EazyQuiz.Unity
{
    public class ApiProvider
    {
        private static readonly string BaseAdress = "https://localhost:7273";
        private readonly HttpClient _client;

        public ApiProvider()
        {
            _client = new HttpClient();

        }

        public async Task<UserResponse> Authtenticate(string username, string password)
        {
            string userSalt = await GetUserSalt(username);
            Debug.Log($"Get Salt and password \n {userSalt}");

            var passwordHash = PasswordHash.HashWithCurrentSalt(password, userSalt);
            Debug.Log($"Get Salt and password {passwordHash} \n {userSalt}");

            string json = JsonConvert.SerializeObject(new UserAuth(username, new UserPassword(passwordHash, userSalt)));

            Debug.Log($"Serialize {json}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{BaseAdress}/api/Auth/GetUserByPassword"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json),
            };

            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();
            Debug.Log(responseBody);
            return JsonConvert.DeserializeObject<UserResponse>(responseBody);
        }

        private async Task<string> GetUserSalt(string username)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAdress}/api/Auth/GetUserSalt?userName={username}"),
            };
            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();
            
            return responseBody;
        }
    }
}