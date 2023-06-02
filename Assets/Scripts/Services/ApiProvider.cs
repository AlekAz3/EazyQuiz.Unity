using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EazyQuiz.Cryptography;
using EazyQuiz.Models.DTO;
using Newtonsoft.Json;
using UnityEngine;

namespace EazyQuiz.Unity.Services
{
    /// <summary>
    /// Работа с АПИ EazyQuiz
    /// </summary>
    public class ApiProvider
    {
        /// <summary>
        /// IP адрес сервера
        /// </summary>
        private static readonly string BaseAddress = "http://10.61.140.42:5274";
        //private static readonly string BaseAddress = "http://192.168.1.90:5274";
        //private static readonly string BaseAddress = "https://eazyquiz.ru";
        
        /// <inheritdoc cref="HttpClient"/>
        private readonly HttpClient _client;

        public ApiProvider()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <param name="username">Ник</param>
        /// <param name="password">Пароль</param>
        /// <returns>Данные о пользователе в <see cref="UserResponse"/></returns>
        public async Task<UserResponse> Authenticate(string username, string password)
        {
            string userSalt = await GetUserSalt(username);

            if (userSalt == "")
            {
                return null;
            }

            var passwordHash = PasswordHash.HashWithCurrentSalt(password, userSalt);

            var user = new UserAuth()
            {
                Username = username,
                PasswordHash = passwordHash
            };

            var response = await _client.GetAsync($"{BaseAddress}/api/Auth?Username={user.Username}&PasswordHash={user.PasswordHash}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log(responseBody);
                return JsonConvert.DeserializeObject<UserResponse>(responseBody);
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Debug.Log("NotFound");
            }
            return null;
        }

        /// <summary>
        /// Получить с сервера соль пользователя
        /// </summary>
        /// <param name="username">Ник</param>
        /// <returns>Строку соль</returns>
        private async Task<string> GetUserSalt(string username)
        {
            var response = await _client.GetAsync($"{BaseAddress}/api/Auth/{username}");
            
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return "";
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        /// <summary>
        /// Регистрация нового игрока
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="username">Ник</param>
        /// <param name="country">Страна</param>
        internal async Task Registration(string password, string username, string country)
        {
            var user = new UserRegister()
            {
                Username = username,
                Country = country,
                Password = PasswordHash.Hash(password),
                Role = "Player"
            };

            string json = JsonConvert.SerializeObject(user);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{BaseAddress}/api/Auth"),
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
            var response = await _client.GetAsync($"{BaseAddress}/api/Auth/{userName}");

            return response.StatusCode == HttpStatusCode.OK;
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
                RequestUri = new Uri($"{BaseAddress}/api/Questions/GetQuestion"),
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
        public async Task<List<QuestionWithAnswers>> GetQuestions(Guid? themeId, string token)
        {
            var url = string.Empty;
            if (themeId == Guid.Empty)
            {
                url = $"{BaseAddress}/api/Questions";
            }
            else
            {
                url = $"{BaseAddress}/api/Questions?ThemeId={themeId}";
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
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
                RequestUri = new Uri($"{BaseAddress}/api/Questions"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");


            await _client.SendAsync(request);
        }

        /// <summary>
        /// Получить коллекцию истории
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="command">Параметры пагинации</param>
        /// <param name="token">JWT токен</param>
        /// <returns>Коллекцию ответов пользователей</returns>
        public async Task<InputCountDTO<UserAnswerHistory>> GetHistory(GetHistoryCommand command, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAddress}/api/History?PageNumber={command.PageNumber}&PageSize={command.PageSize}"),
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<InputCountDTO<UserAnswerHistory>>(responseBody);
        }

        public async Task SendUserQuestion(AddQuestionByUser addQuestion, string token)
        {
            string json = JsonConvert.SerializeObject(addQuestion);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{BaseAddress}/api/AddUserQuestion"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");


            await _client.SendAsync(request);
        }

        public async Task<InputCountDTO<QuestionByUserResponse>> GetCurrentUserQuestions(GetHistoryCommand command, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAddress}/api/AddUserQuestion?PageNumber={command.PageNumber}&PageSize={command.PageSize}"),
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<InputCountDTO<QuestionByUserResponse>>(responseBody);
        }

        public async Task<IReadOnlyCollection<ThemeResponse>> GetThemes(string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAddress}/api/Themes"),
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ThemeResponse>>(responseBody);
        }

        internal async Task<int> GetUserPosition(string country, string token)
        {
            string url = "Leaderboard/user";
            if (country is not null)
            {
                url = $"Leaderboard/user?Country={country}";
            }
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAddress}/api/{url}"),
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<int>(responseBody);
        }

        internal async Task<IReadOnlyCollection<PublicUserInfo>> GetLeaderboard(string country, string token)
        {
            string url = "Leaderboard?Count=5";
            if (country is not null)
            {
                url = $"Leaderboard?Count=5&Country={country}";
            }
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAddress}/api/{url}"),
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IReadOnlyCollection<PublicUserInfo>>(responseBody);
        }

        public async Task SendFeedback(FeedbackRequest feedback, string token)
        {
            string json = JsonConvert.SerializeObject(feedback);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{BaseAddress}/api/Feedback"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            await _client.SendAsync(request);
        }

        internal async Task<Token> RefreshToken(string refreshToken)
        {
            string json = JsonConvert.SerializeObject(refreshToken);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{BaseAddress}/api/Auth/token"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");

            var response = await _client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Token>(responseBody);
            }
            return null;
        }
    }
}