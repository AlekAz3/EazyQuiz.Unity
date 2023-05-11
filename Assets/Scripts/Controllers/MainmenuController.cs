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
    /// ������ �������� ����
    /// </summary>
    public class MainmenuController : MonoBehaviour
    {
        /// <inheritdoc cref="LoadingScreen"/>
        [SerializeField] private LoadingScreen _loadingScreen;

        /// <summary>
        /// ��� ������
        /// </summary>
        [SerializeField] private TMP_Text UsernameLabel;

        [SerializeField] private InformationScreen _error;

        private Banner banner;


        /// <summary>
        /// ���������� ������
        /// </summary>
        [SerializeField] private TMP_Text PointsLabel;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;
        
        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        private void Awake()
        {
            UsernameLabel.text =$"������������ ���� {_userService.UserInfo.UserName}";
            PointsLabel.text = $"����: {_userService.UserInfo.Points}";
            RequestBanner();
        }

        /// <summary>
        /// ������ ����
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
        /// ���������� �������
        /// </summary>
        public void ViewHistoryButtonClick()
        {
            _scene.ShowHistoryScene();
        }

        /// <summary>
        /// ������ ���������� ������� 
        /// </summary>
        public void ViewAddUserQuestionButtonClick()
        {
            _scene.ShowAddUserQuestionScene();
        }

        /// <summary>
        /// ������� �������
        /// </summary>
        public void ViewLeaderboardScene()
        {
            _scene.ShowLeaderboardScene();
        }

        public void NotImplementButton()
        {
            _error.ShowError("� ����������");
        }
    }
}