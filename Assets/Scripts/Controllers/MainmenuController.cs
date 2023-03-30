using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
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

        /// <summary>
        /// ���������� ������
        /// </summary>
        [SerializeField] private TMP_Text PointsLabel;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private SwitchSceneService _scene;
        
        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        private void Awake()
        {
            UsernameLabel.text = _userService.UserInfo.UserName;
            PointsLabel.text = $"����: {_userService.UserInfo.Points}";
        }

        /// <summary>
        /// ������ ����
        /// </summary>
        public void StartGameButtonClick()
        {
            _loadingScreen.Show();
            _scene.ShowGameScene();
        }

        /// <summary>
        /// ���������� �������
        /// </summary>
        public void ViewHistoryButtonClick()
        {
            _loadingScreen.Show();
            _scene.ShowHistoryScene();
        }
    }
}