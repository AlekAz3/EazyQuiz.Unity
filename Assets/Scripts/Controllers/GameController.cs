using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Button> Buttons;
    [SerializeField] private GameObject gameovers;
    [SerializeField] private GameObject timer;
    [SerializeField] private TMP_Text QuestiolLabel;


    /// <summary>
    /// Пол вопросов
    /// </summary>
    private List<QuestionWithAnswers> questions = new List<QuestionWithAnswers>();

    /// <summary>
    /// Порядок вопроса 
    /// </summary>
    private int order = 0;

    [Inject] private UserService _userService;
    [Inject] private ApiProvider _apiProvider;
    private GameOverScreen _gameOverScreen;
    private Timer _timer;

    private async void Awake()
    {
        _gameOverScreen = gameovers.GetComponent<GameOverScreen>();
        _timer = timer.GetComponent<Timer>();
        await NewQuestion();
    }

    /// <summary>
    /// Следующий вопрос
    /// </summary>
    private async Task NewQuestion()
    {
        if (questions.Count() - order < 5)
        {
            await GetQuestions();
        }
        SetQuestion();
        _timer.StartTimer(5);
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

    /// <summary>
    /// Запись текста вопросов и ответов в интерфейс
    /// </summary>
    public void SetQuestion()
    {
        QuestiolLabel.text = questions[order].Text;
        var answers = questions[order].Answers
            .ToList()
            .Shuffle();

        for (int i = 0; i < 4; i++)
        {
            Buttons[i].GetComponent<UserAnswerClick>().WriteAnswer(answers[i]);
        }
    }




    /// <summary>
    /// Проверка ответа игрока
    /// </summary>
    public async Task CheckUserAnswer(Answer answer)
    {
        _timer.StopAllCoroutines();
        if (answer.IsCorrect)
        {
            _gameOverScreen.Show("Ответ верный");
        }
        else
        {
            _gameOverScreen.Show("Ответ верный");
        }

        await _userService.SendUserAnswer(answer, questions[order].QuestionId);
    }

    /// <summary>
    /// Следующий вопрос
    /// </summary>
    public async void NextQuestion()
    {
        order++;
        await NewQuestion();
        _gameOverScreen.Hide();
    }

    /// <summary>
    /// Выход в главное меню
    /// </summary>
    public void ExitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
