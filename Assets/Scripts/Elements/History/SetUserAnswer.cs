using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EazyQuiz.Unity.Elements.History
{
    /// <summary>
    /// Запись ответа пользователя в карточку
    /// </summary>
    public class SetUserAnswer : MonoBehaviour
    {
        /// <summary>
        /// Текст ответа
        /// </summary>
        [SerializeField] private TMP_Text answer;

        /// <summary>
        /// Текст вопроса
        /// </summary>
        [SerializeField] private TMP_Text question;

        /// <summary>
        /// Дата ответа
        /// </summary>
        [SerializeField] private TMP_Text date;

        /// <summary>
        /// Фон
        /// </summary>
        [SerializeField] private Image background;

        /// <summary>
        /// Запись ответа в карточку
        /// </summary>
        /// <param name="history">Исторический ответ пользователя в <see cref="UserAnswerHistory"/></param>
        public void ItemView(UserAnswerHistory history)
        {
            answer.text = history.AnswerText;
            question.text = history.QuestionText;
            date.text = history.AnswerTime.ToString("dd.MM.yyyy HH:mm");
            background.color = history.IsCorrect switch
            {
                false => Color.red,
                true => Color.green
            };
        }
    }
}