using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Game
{
    /// <summary>
    /// ������� ���������� ����
    /// </summary>
    public class GameOverScreen : MonoBehaviour
    {
        /// <summary>
        /// <see cref="GameObject"/> ����
        /// </summary>
        [SerializeField] private GameObject screen;

        /// <summary>
        /// ����� ������� ���� �������
        /// </summary>
        [SerializeField] private TMP_Text text;

        /// <summary>
        /// ������� ���� � ��������� �������
        /// </summary>
        /// <param name="inputText">�����</param>
        public void Show(string inputText)
        {
            screen.SetActive(true);
            text.text = inputText;
        }

        /// <summary>
        /// ������ ����
        /// </summary>
        public void Hide()
        {
            screen.SetActive(false);
        }
    }
}