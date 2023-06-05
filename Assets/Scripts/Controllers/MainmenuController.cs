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
        /// <summary>
        /// Ник игрока
        /// </summary>
        [SerializeField] private TMP_Text helloLabel;


        [SerializeField] private InformationScreen error;

        /// <inheritdoc cref="SwitchSceneService"/>
        [Inject] private readonly SwitchSceneService _scene;
        
        /// <inheritdoc cref="UserService"/>
        [Inject] private readonly UserService _userService;

        private void Awake()
        {
            helloLabel.text = $"Приветствуем тебя: {_userService.UserInfo.UserName}\n" +
                                  $"Твой счёт: {_userService.UserInfo.Points}\n" +
                                  $"Твой лучший результат: {_userService.UserInfo.MaxCombo}";
            
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
            error.ShowError("В разработке");
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