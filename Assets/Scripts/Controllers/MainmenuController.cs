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
        /// <summary>
        /// ��� ������
        /// </summary>
        [SerializeField] private TMP_Text helloLabel;


        [SerializeField] private InformationScreen error;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;
        
        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        private void Awake()
        {
            helloLabel.text = $"������������ ����: {_userService.UserInfo.UserName}\n" +
                                  $"���� ����: {_userService.UserInfo.Points}\n" +
                                  $"���� ������ ���������: {_userService.UserInfo.MaxCombo}";
            
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
            error.ShowError("� ����������");
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