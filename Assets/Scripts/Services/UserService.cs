using EazyQuiz.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EazyQuiz.Unity
{
    internal class UserService
    {
        private readonly ApiProvider _apiProvider;

        public UserResponse UserInfo { get; private set; }

        public UserService(ApiProvider apiProvider)
        {
            _apiProvider = apiProvider;
        }

        public async Task Authtenticate(string login, string password)
        {
            UserInfo = await _apiProvider.Authtenticate(login, password);
        }
    }
}
