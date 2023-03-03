using EazyQuiz.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EazyQuiz.Unity
{
    public class UserService
    {
        private ApiProvider _apiProvider;

        public UserResponse UserInfo { get; private set; }

        public UserService(ApiProvider apiProvider)
        {
            _apiProvider = apiProvider;
        }

        public async Task Authtenticate(string login, string password)
        {
            UserInfo = await _apiProvider.Authtenticate(login, password);
        }

        internal Task SendUserAnswer(int userAnswer)
        {
            throw new NotImplementedException();
        }
    }
}
