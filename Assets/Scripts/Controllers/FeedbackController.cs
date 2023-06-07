using EazyQuiz.Extensions;
using EazyQuiz.Models.DTO;
using EazyQuiz.Unity.Elements.Common;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace EazyQuiz.Unity.Controllers
{
    public class FeedbackController : MonoBehaviour
    {
        [Inject] private readonly SwitchSceneService _scene;
        [Inject] private readonly ApiProvider _provider;
        [Inject] private readonly UserService _user;

        [SerializeField] private TMP_InputField feedbackText;
        [SerializeField] private TMP_InputField Email;
        [SerializeField] private InformationScreen information;

        public void Exit()
        {
            _scene.ShowMainMenuScene();
        }
        
        public async void SendPlayerFeedback()
        {
            if (feedbackText.text.IsNullOrEmpty())
            {
                information.ShowError("Поле Текст пустое");
                return;
            }
            var a = new FeedbackRequest() { Text = feedbackText.text, Email = Email.text };
            await _provider.SendFeedback(a, _user.UserInfo.Token.Jwt);
            
            information.ShowInformation("Ваш вопрос или предложение было отправлено");
            
            feedbackText.text = string.Empty;
            Email.text = string.Empty;
        }

        public void SayThanks()
        {
            Application.OpenURL("https://pay.mysbertips.ru/82048041");
        }
    }
}
