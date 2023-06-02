using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EazyQuiz.Unity.Elements.History
{
    /// <summary>
    /// ������ ������ ������������ � ��������
    /// </summary>
    public class SetUserAnswer : MonoBehaviour
    {
        /// <summary>
        /// ����� ������
        /// </summary>
        [SerializeField] private TMP_Text answer;

        /// <summary>
        /// ����� �������
        /// </summary>
        [SerializeField] private TMP_Text question;

        /// <summary>
        /// ���� ������
        /// </summary>
        [SerializeField] private TMP_Text date;

        /// <summary>
        /// ���
        /// </summary>
        [SerializeField] private Image background;

        /// <summary>
        /// ������ ������ � ��������
        /// </summary>
        /// <param name="history">������������ ����� ������������ � <see cref="UserAnswerHistory"/></param>
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