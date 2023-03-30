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

/// <summary>
/// Контроллер ответов на вопросы
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>
    /// Коллекция кнопок для ответа
    /// </summary>
    [SerializeField] private List<Button> Buttons;

    /// <summary>
    /// Текст вопроса
    /// </summary>
    [SerializeField] private TMP_Text QuestiolLabel;

    /// <summary>
    /// Экран завершения ответа на вопрос
    /// </summary>
    [SerializeField] private GameOverScreen _gameOverScreen;

    /// <summary>
    /// Таймер
    /// </summary>
    [SerializeField] private Timer _timer;

    /// <summary>
    /// Сервис вопросов
    /// </summary>
    [Inject] private QuestionsService _questionsService;

    /// <summary>
    /// Сервис пользователя
    /// </summary>
    [Inject] private UserService _userService;

    /// <summary>
    /// Вопрос который на данный момент на экране
    /// </summary>
    private QuestionWithAnswers question;

    private async void Awake()
    {
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
