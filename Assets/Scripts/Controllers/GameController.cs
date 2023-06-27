using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Elements.Game;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// Контроллер ответов на вопросы
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// Коллекция кнопок для ответа
        /// </summary>
        [SerializeField] private List<Button> buttons;

        /// <summary>
        /// Текст вопроса
        /// </summary>
        [SerializeField] private TMP_Text questionLabel;

        /// <summary>
        /// Экран завершения ответа на вопрос
        /// </summary>
        [SerializeField] private GameOverScreen gameOverScreen;

        [SerializeField] private GameObject settingsGame;

        /// <summary>
        /// Таймер
        /// </summary>
        [SerializeField] private Timer timer;

        [SerializeField] private TMP_Dropdown chooseTheme;

        [SerializeField] private LoadingScreen loadingScreen;

        /// <summary>
        /// Сервис вопросов
        /// </summary>
        [Inject] private QuestionsService _questionsService;

        /// <summary>
        /// Сервис пользователя
        /// </summary>
        [Inject] private readonly UserService _userService;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;

        /// <summary>
        /// Вопрос который на данный момент на экране
        /// </summary>
        private QuestionWithAnswers _question;
        private List<ThemeResponse> _themes;
        private int _timerTime;
        private int _combo;
        
        [SerializeField] private TMP_InputField timerInput;
        [SerializeField] private InformationScreen information;
        
        [SerializeField] private InterstitialAd ad;


        public GameController()
        {
            _timerTime = 10;
        }

        private async void Awake()
         {
            timerInput.text = _timerTime.ToString();
            _themes = (await _questionsService.GetThemes()).ToList();
            chooseTheme.AddOptions(_themes.Select(x => x.Name).ToList());
            Debug.Log(_themes);
        }

        /// <summary>
        /// Следующий вопрос
        /// </summary>
        private async Task NewQuestion()
        {
            _question = await _questionsService.NextQuestion();
            SetQuestion();
            timer.StartTimer(_timerTime);
        }

        /// <summary>
        /// Запись текста вопросов и ответов в интерфейс
        /// </summary>
        public void SetQuestion()
        {
            questionLabel.text = _question.Text;
            var answers = _question.Answers
                .ToList()
                .Shuffle();
            for (var i = 0; i < 4; i++)
            {
                buttons[i].GetComponent<UserAnswerClick>().WriteAnswer(answers[i]);
            }
        }

        public async void StartGame()
        {
            var timerValue = Convert.ToInt32(timerInput.text);

            if (timerValue is >= 50 or <= 0)
            {
                information.ShowError("Неверное значение таймера");
                return;
            }
            
            _timerTime = timerValue;

            _questionsService.ThemeId = _themes.Where(x => x.Name == chooseTheme.captionText.text)
                .Select(x => x.Id)
                .FirstOrDefault();

            loadingScreen.Show();
            await NewQuestion();
            settingsGame.SetActive(false);
            loadingScreen.Hide();
        }

        /// <summary>
        /// Проверка ответа игрока
        /// </summary>
        public async Task CheckUserAnswer(AnswerDTO answer)
        {
            timer.StopTimer();
            if (answer.IsCorrect)
            {
                _combo++;
                gameOverScreen.Show($"Ответ: верный,\nВаш текущий результат {_combo}\n\nЕсли покинете игру вы потеряете текущий результат");
            }
            else
            {
                ad.ShowAd();
                gameOverScreen.Show($"Ответ: не верный\n\nВерный ответ: {_question.Answers.Single(x => x.IsCorrect).AnswerText}\n\nВаш результат:{_combo}");
                ad.LoadAd();
            }

            var points = CalculatePoints(answer.IsCorrect);
            
            await _userService.SendUserAnswer(answer, _question.QuestionId, _combo, points);
            
            if (!answer.IsCorrect)
            {
                _combo = 0;
            }
        }

        private int CalculatePoints(bool answerIsCorrect)
        {
            if (!answerIsCorrect)
            {
                return 0;
            }

            var timerPoints = _timerTime switch
            {
                <= 5 => 3,
                > 5 and <= 20 => 2,
                > 20 and < 40 => 1,
                _ => 0
            };

            var points = _combo + timerPoints;

            return points;
        }

        /// <summary>
        /// Следующий вопрос
        /// </summary>
        public async void NextQuestion()
        {
            await NewQuestion();
            gameOverScreen.Hide();
        }

        /// <summary>
        /// Если время ответа на вопрос вышло
        /// </summary>
        public void TimeOver()
        {
            gameOverScreen.Show($"Время вышло\n\nВерный ответ: {_question.Answers.Single(x => x.IsCorrect).AnswerText}\n" +
                                $"Ваш результат:{_combo}\n" +
                                $"Ваш прогресс сбросился");
            
            _combo = 0;
        }
        /// <summary>
        /// Выход в главное меню
        /// </summary>
        public void ExitButtonClick()
        {
            _scene.ShowMainMenuScene();
        }
    }
}