using System;
using System.Threading.Tasks;
using EazyQuiz.Cryptography;
using EazyQuiz.Models.DTO;
using Zenject;

namespace EazyQuiz.Unity.Services
{
    /// <summary>
    /// Сервис работы с пользователем
    /// </summary>
    public class UserService
    {
        /// <inheritdoc cref="ApiProvider"/>
        private readonly ApiProvider _apiProvider;

        /// <inheritdoc cref="UserResponse"/>
        public UserResponse UserInfo { get; private set; }

        [Inject] private SaveUserService _saveUser;

        public UserService()
        {
            _apiProvider = new ApiProvider();
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public async Task Authenticate(string login, string password)
        {
            UserInfo = await _apiProvider.Authenticate(login, password);
            _saveUser.SaveUser(UserInfo);
        }

        public async Task<bool> SetUser(UserResponse user)
        {
            var newToken = await _apiProvider.RefreshToken(user.Token.RefrashToken);
            if (newToken == null) return false;
            
            UserInfo = user;
            UserInfo.Token = newToken;
            _saveUser.SaveUser(UserInfo);
            
            return true;
        }

        /// <summary>
        /// Отправить ответ пользователя
        /// </summary>
        /// <param name="answer">Ответ</param>
        /// <param name="questionId">Ид вопроса</param>
        internal async Task SendUserAnswer(AnswerDTO answer, Guid questionId)
        {
            var userAnswer = new UserAnswer()
            {
                QuestionId = questionId,
                AnswerId = answer.AnswerId
            };

            if (answer.IsCorrect)
            {
                AddPoint();
            }

            await _apiProvider.SendUserAnswer(userAnswer, UserInfo.Token.Jwt);
        }

        /// <summary>
        /// Добавить балл
        /// </summary>
        private void AddPoint()
        {
            UserInfo.Points++;
            _saveUser.SaveUser(UserInfo);
        }

        public async Task ChangeNickname(string nickname)
        {
            await _apiProvider.ChangeUsername(nickname, UserInfo.Token.Jwt);
            UserInfo.UserName = nickname;
            _saveUser.SaveUser(UserInfo);
        }

        public async Task<bool> CheckNickname(string nickname)
        {
            return await _apiProvider.CheckUsername(nickname);
        }

        public async Task ChangePassword(string password)
        {
            var passwordDto = PasswordHash.Hash(password);
            await _apiProvider.ChangePassword(passwordDto, UserInfo.Token.Jwt);
        }
    }
}
