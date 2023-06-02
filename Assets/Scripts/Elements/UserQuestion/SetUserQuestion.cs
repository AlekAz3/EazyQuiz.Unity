using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.UserQuestion
{
    public class SetUserQuestion : MonoBehaviour
    {
        /// <summary>
        /// Текст вопроса
        /// </summary>
        [SerializeField] private TMP_Text question;

        /// <summary>
        /// Дата обновления
        /// </summary>
        [SerializeField] private TMP_Text date;

        /// <summary>
        /// Статус
        /// </summary>
        [SerializeField] private TMP_Text status;

        /// <summary>
        /// Запись ответа в карточку
        /// </summary>
        /// <param name="history">Исторический вопрос пользователя в <see cref="QuestionByUserResponse"/></param>
        public void ItemView(QuestionByUserResponse history)
        {
            question.text = history.QuestionText;
            date.text = history.LastUpdate.ToString("dd.MM.yyyy HH:mm");
            status.text = history.Status;
        }
    }
}
