using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.UserQuestion
{
    public class SetUserQuestion : MonoBehaviour
    {
        /// <summary>
        /// ����� �������
        /// </summary>
        [SerializeField] private TMP_Text question;

        /// <summary>
        /// ���� ����������
        /// </summary>
        [SerializeField] private TMP_Text date;

        /// <summary>
        /// ������
        /// </summary>
        [SerializeField] private TMP_Text status;

        /// <summary>
        /// ������ ������ � ��������
        /// </summary>
        /// <param name="history">������������ ������ ������������ � <see cref="QuestionByUserResponse"/></param>
        public void ItemView(QuestionByUserResponse history)
        {
            question.text = history.QuestionText;
            date.text = history.LastUpdate.ToString("dd.MM.yyyy HH:mm");
            status.text = history.Status;
        }
    }
}
