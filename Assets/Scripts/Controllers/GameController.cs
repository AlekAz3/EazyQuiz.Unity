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

    private QuestionWithAnswers question;

    [Inject] private QuestionsService _questionsService;
    [Inject] private UserService _userService;


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
        question = await _questionsService.NextQuestion();
        SetQuestion();
        _timer.StartTimer(5);
    }

    /// <summary>
    /// Запись текста вопросов и ответов в интерфейс
    /// </summary>
    public void SetQuestion()
    {
        QuestiolLabel.text = question.Text;
        var answers = question.Answers
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
        _timer.StopTimer();
        if (answer.IsCorrect)
        {
            _gameOverScreen.Show("Ответ верный");
        }
        else
        {
            _gameOverScreen.Show("Ответ не верный");
        }

        await _userService.SendUserAnswer(answer, question.QuestionId);
    }

    /// <summary>
    /// Следующий вопрос
    /// </summary>
    public async void NextQuestion()
    {
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
