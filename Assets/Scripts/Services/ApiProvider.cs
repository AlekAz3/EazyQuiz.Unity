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
using System.Net;
using Unity.VisualScripting.Antlr3.Runtime;

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
            return null;
        }

        private async Task<string> GetUserSalt(string username)
        {
            var response = await _client.GetAsync($"{BaseAdress}/api/Auth/{username}");
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

            return !(response.StatusCode == HttpStatusCode.NotFound);
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
                RequestUri = new Uri($"{BaseAdress}/api/Questions/PostUserAnswer"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };
            request.Headers.TryAddWithoutValidation("Accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");


            await _client.SendAsync(request);
        }

        public async Task<InputCountDTO<UserAnswerHistory>> GetHistory(Guid userId, AnswersGetHistoryCommand command, string token)
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
    }
}