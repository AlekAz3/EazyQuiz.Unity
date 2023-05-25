using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Elements.Game;
using EazyQuiz.Unity.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// ���������� ������� �� �������
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// ��������� ������ ��� ������
        /// </summary>
        [SerializeField] private List<Button> Buttons;

        /// <summary>
        /// ����� �������
        /// </summary>
        [SerializeField] private TMP_Text QuestiolLabel;

        /// <summary>
        /// ����� ���������� ������ �� ������
        /// </summary>
        [SerializeField] private GameOverScreen _gameOverScreen;

        [SerializeField] private GameObject settingsGame;

        /// <summary>
        /// ������
        /// </summary>
        [SerializeField] private Timer _timer;

        [SerializeField] private TMP_Dropdown _chooseTheme;

        [SerializeField] private LoadingScreen _loadingScreen;

        /// <summary>
        /// ������ ��������
        /// </summary>
        [Inject] private QuestionsService _questionsService;

        /// <summary>
        /// ������ ������������
        /// </summary>
        [Inject] private readonly UserService _userService;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;

        /// <summary>
        /// ������ ������� �� ������ ������ �� ������
        /// </summary>
        private QuestionWithAnswers question;
        private List<ThemeResponse> themes;

        private async void Awake()
         {
            themes = (await _questionsService.GetThemes()).ToList();
            _chooseTheme.AddOptions(themes.Select(x => x.Name).ToList());
            Debug.Log(themes);
        }

        /// <summary>
        /// ��������� ������
        /// </summary>
        private async Task NewQuestion()
        {
            question = await _questionsService.NextQuestion();
            SetQuestion();
            _timer.StartTimer(10);
        }

        /// <summary>
        /// ������ ������ �������� � ������� � ���������
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

        public async void StartGame()
        {
            _questionsService.ThemeId = themes.Where(x => x.Name == _chooseTheme.captionText.text).Select(x => x.Id).FirstOrDefault();
            _loadingScreen.Show();
            await NewQuestion();
            settingsGame.SetActive(false);
            _loadingScreen.Hide();
        }

        /// <summary>
        /// �������� ������ ������
        /// </summary>
        public async Task CheckUserAnswer(AnswerDTO answer)
        {
            _timer.StopTimer();
            if (answer.IsCorrect)
            {
                _gameOverScreen.Show("�����: ������");
            }
            else
            {
                _gameOverScreen.Show($"�����: �� ������\n\n������ �����: {question.Answers.Where(x => x.IsCorrect).Single().AnswerText}");
            }

            await _userService.SendUserAnswer(answer, question.QuestionId);
        }

        /// <summary>
        /// ��������� ������
        /// </summary>
        public async void NextQuestion()
        {
            await NewQuestion();
            _gameOverScreen.Hide();
        }

        /// <summary>
        /// ����� � ������� ����
        /// </summary>
        public void ExitButtonClick()
        {
            _scene.ShowMainMenuScene();
        }
    }
}