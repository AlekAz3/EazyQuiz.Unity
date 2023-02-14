using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyQuiz.Models.DTO;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;
using EazyQuiz.Cryptography;

namespace EazyQuiz.Unity
{
    public class ApiProvider 
    {
        private static readonly string BaseAdress = "https://localhost:7273";

        public ApiProvider()
        {

        }

        public async Task<UserResponse> Authtenticate(string username, string password)
        {
            var uwr1 = UnityWebRequest.Get($"{BaseAdress}/api/Auth/GetUserSalt?userName{username}");

            uwr1.SendWebRequest();

            while (!uwr1.isDone)
            {
                await Task.Yield();
            }

            string userSalt = string.Empty;

            if (uwr1.result is UnityWebRequest.Result.Success)
            {
                userSalt = uwr1.downloadHandler.text;
            }

            var passwordHash = PasswordHash.HashWithCurrentSalt(password, userSalt);

            var json = JsonUtility.ToJson(new UserAuth(username, new UserPassword(passwordHash, userSalt)));

            var uwr2 = UnityWebRequest.Post($"{BaseAdress}/api/Auth/GetUserSalt?userName{username}", json);
            while (!uwr2.isDone)
            {
                await Task.Yield();
            }

            if (uwr2.result is UnityWebRequest.Result.Success)
            {
                var user = JsonUtility.FromJson<UserResponse>(uwr2.downloadHandler.text);
                return user;
            }

            return new UserResponse(0, "", 0, "", 0, "", "");
        }
    }
}