using UnityEngine;

namespace EazyQuiz.Unity.Elements.Common
{
    /// <summary>
    /// ���� ��������
    /// </summary>
    public class LoadingScreen : MonoBehaviour
    {
        /// <summary>
        /// <see cref="GameObject"/> ����
        /// </summary>
        [SerializeField] private GameObject screen;

        /// <summary>
        /// �������� ����
        /// </summary>
        public void Show()
        {
            screen.SetActive(true);
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