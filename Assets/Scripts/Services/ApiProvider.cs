using UnityEngine;
using EazyQuiz.Models.DTO;
using System;
using System.Threading.Tasks;
using EazyQuiz.Cryptography;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace EazyQuiz.Unity.Services
{
    /// <summary>
    /// ������ � ��� EazyQuiz
    /// </summary>
    public class ApiProvider
    {
        /// <summary>
        /// IP ����� �������
        /// </summary>
        //private static readonly string BaseAdress = "http://10.61.140.42:5274";
        //private static readonly string BaseAdress = "http://192.168.1.90:5274";
        private static readonly string BaseAdress = "http://eazyquiz-ru.1gb.ru";
        /// <inheritdoc cref="HttpClient"/>
        private readonly HttpClient _client;


        public ApiProvider()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// �������������� ������������
        /// </summary>
        /// <param name="username">���</param>
        /// <param name="password">������</param>
        /// <returns>������ � ������������ � <see cref="UserResponse"/></returns>
        public async Task<UserResponse> Authtenticate(string username, string password)
        {
            string userSalt = await GetUserSalt(username);
            Debug.Log($"Get Salt and password \n {userSalt}");

            if (userSalt == "")
            {
                return new UserResponse() { Id = Guid.Empty };
            }

            var passwordHash = PasswordHash.HashWithCurrentSalt(password, userSalt);
            Debug.Log($"Get Salt and password {passwordHash} \n {userSalt}");

            var user = new UserAuth()
            {
                Username = username,
                PasswordHash = passwordHash
            };

            var response = await _client.GetAsync($"{BaseAdress}/api/Auth?Username={user.Username}&PasswordHash={user.PasswordHash}");

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
            return new UserResponse() { Id = Guid.Empty }; ;
        }

        /// <summary>
        /// �������� � ������� ���� ������������
        /// </summary>
        /// <param name="username">���</param>
        /// <returns>������ ����</returns>
        private async Task<string> GetUserSalt(string username)
        {
            var response = await _client.GetAsync($"{BaseAdress}/api/Auth/{username}");
            
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return "";
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;

        }

        /// <summary>
        /// ����������� ������ ������
        /// </summary>
        /// <param name="password">������</param>
        /// <param name="username">���</param>
        /// <param name="age">�������</param>
        /// <param name="gender">���</param>
        /// <param name="country">������</param>
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
                RequestUri = new Uri($"{BaseAdress}/api/Auth"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json),
            };

            await _client.SendAsync(request);
        }

        /// <summary>
        /// �������� �� ������������ ��� 
        /// </summary>
        /// <param name="userName">���</param>
        /// <returns>true - ���� ��� �� ��������</returns>
        /// <exception cref="ArgumentNullException">����</exception>
        public async Task<bool> CheckUsername(string userName)
        {
            var response = await _client.GetAsync($"{BaseAdress}/api/Auth/{userName}");

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// �������� ������ � ������ � �������
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
        /// �������� ������ � ������ � �������
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<QuestionWithAnswers>> GetQuestions(Guid? themeId, string token)
        {
            var url = "";
            if (themeId == Guid.Empty)
            {
                url = $"{BaseAdress}/api/Questions";
            }
            else
            {
                url = $"{BaseAdress}/api/Questions?ThemeId={themeId}";
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
        /// ��������� ����� ������ �� ������ 
        /// </summary>
        /// <param name="answer">����� � ���� <see cref="UserAnswer"/></param>
        /// <param name="token">JWT �����</param>
        /// <returns></returns>
        public async Task SendUserAnswer(UserAnswer answer, string token)
        {
            string json = JsonConvert.SerializeObject(answer);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{BaseAdress}/api/Questions"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");


            await _client.SendAsync(request);
        }

        /// <summary>
        /// �������� ��������� �������
        /// </summary>
        /// <param name="userId">�� ������������</param>
        /// <param name="command">��������� ���������</param>
        /// <param name="token">JWT �����</param>
        /// <returns>��������� ������� �������������</returns>
        public async Task<InputCountDTO<UserAnswerHistory>> GetHistory(Guid userId, GetHistoryCommand command, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAdress}/api/History?userId={userId}&PageNumber={command.PageNumber}&PageSize={command.PageSize}"),
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
                RequestUri = new Uri($"{BaseAdress}/api/AddUserQuestion"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");


            await _client.SendAsync(request);
        }

        public async Task<InputCountDTO<QuestionByUserResponse>> GetCurrentUserQuestions(Guid userId, GetHistoryCommand command, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{BaseAdress}/api/AddUserQuestion?userId={userId}&PageNumber={command.PageNumber}&PageSize={command.PageSize}"),
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
                RequestUri = new Uri($"{BaseAdress}/api/Themes"),
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await _client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ThemeResponse>>(responseBody);
        }
    }
}