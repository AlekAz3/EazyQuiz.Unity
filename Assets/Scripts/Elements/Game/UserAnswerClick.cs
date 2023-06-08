using EazyQuiz.Unity.Controllers;
using EazyQuiz.Models.DTO;
using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Game
{
    /// <summary>
    /// ������� ������ �� ������
    /// </summary>
    public class UserAnswerClick : MonoBehaviour
    {
        /// <summary>
        /// ����� ������ � ������
        /// </summary>
        [SerializeField] private TMP_Text buttonText;

        /// <summary>
        /// GameController
        /// </summary>
        [SerializeField] private GameController gameController;

        /// <summary>
        /// ����� ����������� �� ������
        /// </summary>
        private AnswerDTO _answer;

        /// <summary>
        /// �������� ����� �� ������
        /// </summary>
        /// <param name="answer">����� � <see cref="AnswerDTO"/></param>
        public void WriteAnswer(AnswerDTO answer)
        {
            _answer = answer;
            buttonText.text = answer.AnswerText;
        }

        /// <summary>
        /// �������� ����� 
        /// </summary>
        public async void SendAnswer()
        {
            await gameController.CheckUserAnswer(_answer);
        }
    }
}