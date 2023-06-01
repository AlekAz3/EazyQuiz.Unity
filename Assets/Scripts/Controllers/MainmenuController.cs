using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// Панель главного меню
    /// </summary>
    public class MainmenuController : MonoBehaviour
    {
        /// <inheritdoc cref="LoadingScreen"/>
        [SerializeField] private LoadingScreen _loadingScreen;

        /// <summary>
        /// Ник игрока
        /// </summary>
        [SerializeField] private TMP_Text UsernameLabel;


        [SerializeField] private InformationScreen _error;

        /// <summary>
        /// Количество баллов
        /// </summary>
        [SerializeField] private TMP_Text PointsLabel;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;
        
        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        private void Update()
        {
            if (_userService.UserInfo != null)
            {
                UsernameLabel.text = $"Приветствуем тебя: {_userService.UserInfo.UserName}\nТвой счёт: {_userService.UserInfo.Points}";
            }
        }

        /// <summary>
        /// Начать игру
        /// </summary>
        public void StartGameButtonClick()
        {
            _scene.ShowGameScene();
        }

        /// <summary>
        /// Посмотреть историю
        /// </summary>
        public void ViewHistoryButtonClick()
        {
            _scene.ShowHistoryScene();
        }

        /// <summary>
        /// Панель добавления истории 
        /// </summary>
        public void ViewAddUserQuestionButtonClick()
        {
            _scene.ShowAddUserQuestionScene();
        }

        /// <summary>
        /// Таблица лидеров
        /// </summary>
        public void ViewLeaderboardScene()
        {
            _scene.ShowLeaderboardScene();
        }

        public void NotImplementButton()
        {
            _error.ShowError("В разработке");
        }

        public void ViewFeedbackScene()
        {
            _scene.ShowFeedbackScene();
        }

        public void ViewSettingScene()
        {
            _scene.ShowSettingScene();
        }
    }
}