using EazyQuiz.Models.DTO;
using System;
using System.Threading.Tasks;

namespace EazyQuiz.Unity.Services
{
    /// <summary>
    /// Сервис работы с пользователем
    /// </summary>
    public class UserService
    {
        /// <inheritdoc cref="ApiProvider"/>
        private ApiProvider _apiProvider;

        /// <inheritdoc cref="UserResponse"/>
        public UserResponse UserInfo { get; private set; }

        public UserService()
        {
            _apiProvider = new ApiProvider();
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public async Task Authtenticate(string login, string password)
        {
            UserInfo = await _apiProvider.Authtenticate(login, password);

        }

        /// <summary>
        /// Отправить ответ пользователя
        /// </summary>
        /// <param name="answer">Ответ</param>
        /// <param name="questionId">Ид вопроса</param>
        internal async Task SendUserAnswer(Answer answer, Guid questionId)
        {
            var userAnswer = new UserAnswer()
            {
                UserId = UserInfo.Id,
                QuestionId = questionId,
                AnswerId = answer.AnswerId
            };

            if (answer.IsCorrect)
            {
                AddPoint();
            }

            await _apiProvider.SendUserAnswer(userAnswer, UserInfo.Token);
        }

        /// <summary>
        /// Добавить балл
        /// </summary>
        private void AddPoint()
        {
            UserInfo.Points++;
        }
    }
}
