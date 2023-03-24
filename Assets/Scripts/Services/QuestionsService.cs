using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class QuestionsService
{
    /// <summary>
    /// Пол вопросов
    /// </summary>
    private List<QuestionWithAnswers> questions = new List<QuestionWithAnswers>();

    /// <summary>
    /// Порядок вопроса 
    /// </summary>
    private int order = -1;

    private readonly UserService _userService;
    private readonly ApiProvider _apiProvider;

    public QuestionsService(UserService userService, ApiProvider apiProvider)
    {
        _userService = userService;
        _apiProvider = apiProvider;
    }

    /// <summary>
    /// Следующий вопрос
    /// </summary>
    /// <returns>Вопрос с ответами в <see cref="QuestionWithAnswers"/></returns>
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
    /// Дополнение вопросов с сервера 
    /// </summary>
    public async Task GetQuestions()
    {
        if (order > 25)
        {
            order = 0;
            questions.Clear();
        }
        var ques = await _apiProvider.GetQuestions(_userService.UserInfo.Token);

        questions.AddRange(ques);
    }
}
