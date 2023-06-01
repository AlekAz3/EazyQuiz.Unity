using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
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

        /// <summary>
        /// ���������� ������
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
                UsernameLabel.text = $"������������ ����: {_userService.UserInfo.UserName}\n���� ����: {_userService.UserInfo.Points}";
            }
        }

        /// <summary>
        /// ������ ����
        /// </summary>
        public void StartGameButtonClick()
        {
            _scene.ShowGameScene();
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