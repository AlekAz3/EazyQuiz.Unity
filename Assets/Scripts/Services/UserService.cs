using EazyQuiz.Models.DTO;
using System;
using System.Threading.Tasks;

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

        private void AddPoint()
        {
            UserInfo.Points++;
        }
    }
}
