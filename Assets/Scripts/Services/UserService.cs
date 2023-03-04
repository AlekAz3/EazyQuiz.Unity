using EazyQuiz.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace EazyQuiz.Unity
{
    public class UserService
    {
        private ApiProvider _apiProvider;

        public UserResponse UserInfo { get; private set; }

        public UserService()
        {
            _apiProvider = new ApiProvider();
        }

        public async Task Authtenticate(string login, string password)
        {
            UserInfo = await _apiProvider.Authtenticate(login, password);
        }

        internal async Task SendUserAnswer(int userAnswerId, int questionId)
        {
            var a = new UserAnswer()
            {
                IdUser = UserInfo.Id,
                IdQuestion = questionId,
                IdAnswer = userAnswerId
            };

            await _apiProvider.SendUserAnswer(a, UserInfo.Token);
        }

        internal void AddPoint()
        {
            UserInfo.Points++;
        }
    }
}
