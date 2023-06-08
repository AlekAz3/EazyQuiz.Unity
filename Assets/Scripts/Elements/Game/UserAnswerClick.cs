using EazyQuiz.Unity.Controllers;
using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Game
{
    /// <summary>
    /// Элемент ответа на вопрос
    /// </summary>
    public class UserAnswerClick : MonoBehaviour
    {
        /// <summary>
        /// Текст ответа в кнопке
        /// </summary>
        [SerializeField] private TMP_Text buttonText;

        /// <summary>
        /// GameController
        /// </summary>
        [SerializeField] private GameController gameController;

        /// <summary>
        /// Ответ назначенный на кнопку
        /// </summary>
        private AnswerDTO _answer;

        /// <summary>
        /// Записать ответ на кнопку
        /// </summary>
        /// <param name="answer">Ответ в <see cref="AnswerDTO"/></param>
        public void WriteAnswer(AnswerDTO answer)
        {
            _answer = answer;
            buttonText.text = answer.AnswerText;
        }

        /// <summary>
        /// Записать ответ 
        /// </summary>
        public async void SendAnswer()
        {
            await gameController.CheckUserAnswer(_answer);
        }
    }
}