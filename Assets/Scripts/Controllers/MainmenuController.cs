using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
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

        [SerializeField] private ErrorScreen _error;

        /// <summary>
        /// Количество баллов
        /// </summary>
        [SerializeField] private TMP_Text PointsLabel;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;
        
        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        private void Awake()
        {
            UsernameLabel.text = _userService.UserInfo.UserName;
            PointsLabel.text = $"Очки: {_userService.UserInfo.Points}";
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

        public void NotImplementButton()
        {
            _error.Activate("В разработке");
        }
    }
}