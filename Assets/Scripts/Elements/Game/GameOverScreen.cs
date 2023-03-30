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
        /// <param name="inpitText">�����</param>
        public void Show(string inpitText)
        {
            screen.SetActive(true);
            text.text = inpitText;
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