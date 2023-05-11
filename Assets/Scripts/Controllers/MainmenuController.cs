using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;
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

        private Banner banner;


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
            UsernameLabel.text =$"Приветствуем тебя {_userService.UserInfo.UserName}";
            PointsLabel.text = $"Счёт: {_userService.UserInfo.Points}";
            RequestBanner();
        }

        /// <summary>
        /// Начать игру
        /// </summary>
        public void StartGameButtonClick()
        {
            _scene.ShowGameScene();
        }

        private void RequestBanner()
        {
            string adUnitId = "kek";

            banner = new Banner(adUnitId, AdSize.BANNER_320x50, AdPosition.BottomCenter);

            AdRequest request = new AdRequest.Builder().Build();

            banner.LoadAd(request);

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
    }
}