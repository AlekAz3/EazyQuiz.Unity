using UnityEngine;
using EazyQuiz.Models.DTO;
using System;
using System.Threading.Tasks;
using EazyQuiz.Cryptography;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using EazyQuiz.Extensions;
using Zenject;
using System.Collections.Generic;

namespace EazyQuiz.Unity
{
    public class ApiProvider
    {
        private static readonly string BaseAdress = "http://localhost:5274";
        private readonly HttpClient _client;


        public ApiProvider()
        {
            _client = new HttpClient();
        }

        public async Task<UserResponse> Authtenticate(string username, string password)
        {

            string userSalt = await GetUserSalt(username);
            Debug.Log($"Get Salt and password \n {userSalt}");

            if (userSalt.IsNullOrEmpty())
            {
                return null;
            }

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

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                return JsonConvert.DeserializeObject<UserResponse>(responseBody);
            }
            return null;
        }

        private async Task<string> GetUserSalt(string username)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAdress}/api/Auth/GetUserSalt?userName={username}"),
            };
            var response = await _client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                return "";
            }

        }


        /// <summary>
        /// Регистрация нового игрока
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="username">Ник</param>
        /// <param name="age">Возраст</param>
        /// <param name="gender">Пол</param>
        /// <param name="country">Страна</param>
        internal async Task Registrate(string password, string username, int age, string gender, string country)
        {
            var user = new UserRegister()
            {
                Username = username,
                Age = age,
                Gender = gender,
                Country = country,
                Password = PasswordHash.Hash(password)
            };

            string json = JsonConvert.SerializeObject(user);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{BaseAdress}/api/Auth/RegisterNewPlayer"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json),
            };

            await _client.SendAsync(request);
        }

        /// <summary>
        /// Проверка на существующей ник 
        /// </summary>
        /// <param name="userName">Ник</param>
        /// <returns>true - если ник НЕ уникален</returns>
        /// <exception cref="ArgumentNullException">Нулл</exception>
        public async Task<bool> CheckUsername(string userName)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAdress}/api/Auth/CheckUniqueUsername?userName={userName}"),
            };

            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseBody == null)
            {
                throw new ArgumentNullException(paramName: nameof(userName));
            }

            if (responseBody == "true")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получить вопрос и ответы с сервера
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<QuestionWithAnswers> GetQuestion(string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAdress}/api/Questions/GetQuestion"),
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<QuestionWithAnswers>(responseBody);
        }


        /// <summary>
        /// Получить вопрос и ответы с сервера
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<QuestionWithAnswers>> GetQuestions(string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAdress}/api/Questions/GetQuestions"),
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await _client.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<QuestionWithAnswers>>(responseBody);
        }

        /// <summary>
        /// Отправить ответ игрока на сервер 
        /// </summary>
        /// <param name="answer">ответ в виде <see cref="UserAnswer"/></param>
        /// <param name="token">JWT токен</param>
        /// <returns></returns>
        public async Task SendUserAnswer(UserAnswer answer, string token)
        {
            string json = JsonConvert.SerializeObject(answer);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{BaseAdress}/api/Questions/PostUserAnswer"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");


            await _client.SendAsync(request);
        }
    }
}