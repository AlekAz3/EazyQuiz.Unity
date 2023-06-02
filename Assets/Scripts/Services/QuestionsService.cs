using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EazyQuiz.Models.DTO;

namespace EazyQuiz.Unity.Services
{
    /// <summary>
    /// ������ �� ������� ��������
    /// </summary>
    public class QuestionsService
    {
        /// <summary>
        /// ��� ��������
        /// </summary>
        private readonly List<QuestionWithAnswers> _questions = new List<QuestionWithAnswers>();

        /// <summary>
        /// ������� ������� 
        /// </summary>
        private int _order = -1;

        /// <inheritdoc cref="UserService"/>
        private readonly UserService _userService;

        /// <inheritdoc cref="ApiProvider"/>
        private readonly ApiProvider _apiProvider;

        public Guid? ThemeId { get; set; }  

        public QuestionsService(UserService userService, ApiProvider apiProvider)
        {
            _userService = userService;
            _apiProvider = apiProvider;
        }

        /// <summary>
        /// ��������� ������
        /// </summary>
        /// <returns>������ � �������� � <see cref="QuestionWithAnswers"/></returns>
        public async Task<QuestionWithAnswers> NextQuestion()
        {
            _order++;
            if (_questions.Count - _order < 5)
            {
                await GetQuestions();
            }
            return _questions[_order];
        }

        /// <summary>
        /// ���������� �������� � ������� 
        /// </summary>
        public async Task GetQuestions()
        {
            if (_order > 25)
            {
                _order = 0;
                _questions.Clear();
            }
            var ques = await _apiProvider.GetQuestions(ThemeId,_userService.UserInfo.Token.Jwt);

            _questions.AddRange(ques);
        }

        public async Task<IReadOnlyCollection<ThemeResponse>> GetThemes()
        {
            return await _apiProvider.GetThemes(_userService.UserInfo.Token.Jwt);
        }
    }
}