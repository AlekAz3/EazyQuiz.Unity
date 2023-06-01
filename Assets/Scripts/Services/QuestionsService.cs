using EazyQuiz.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Experimental.GlobalIllumination;

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
        private List<QuestionWithAnswers> questions = new List<QuestionWithAnswers>();

        /// <summary>
        /// ������� ������� 
        /// </summary>
        private int order = -1;

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
            order++;
            if (questions.Count - order < 5)
            {
                await GetQuestions();
            }
            return questions[order];
        }

        /// <summary>
        /// ���������� �������� � ������� 
        /// </summary>
        public async Task GetQuestions()
        {
            if (order > 25)
            {
                order = 0;
                questions.Clear();
            }
            var ques = await _apiProvider.GetQuestions(ThemeId,_userService.UserInfo.Token.Jwt);

            questions.AddRange(ques);
        }

        public async Task<IReadOnlyCollection<ThemeResponse>> GetThemes()
        {
            return await _apiProvider.GetThemes(_userService.UserInfo.Token.Jwt);
        }
    }
}