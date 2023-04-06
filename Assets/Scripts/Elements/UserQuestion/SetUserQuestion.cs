using EazyQuiz.Models.DTO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.UserQuestion
{
    public class SetUserQuestion : MonoBehaviour
    {
        /// <summary>
        /// ����� �������
        /// </summary>
        [SerializeField] private TMP_Text Question;

        /// <summary>
        /// ���� ����������
        /// </summary>
        [SerializeField] private TMP_Text Date;

        /// <summary>
        /// ������
        /// </summary>
        [SerializeField] private TMP_Text Status;

        /// <summary>
        /// ������ ������ � ��������
        /// </summary>
        /// <param name="history">������������ ������ ������������ � <see cref="QuestionByUserResponse"/></param>
        public void ItemView(QuestionByUserResponse history)
        {
            Question.text = history.QuestionText;
            Date.text = history.LastUpdate.ToString("dd.MM.yyyy HH:mm");
            Status.text = history.Status;
        }
    }
}
