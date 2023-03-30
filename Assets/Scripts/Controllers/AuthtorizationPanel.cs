using UnityEngine;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// ���������� ������ �������������� � �����������
    /// </summary>
    public class AuthtorizationPanel : MonoBehaviour
    {
        /// <summary>
        /// ������ ��������������
        /// </summary>
        [SerializeField] private GameObject LoginGO;

        /// <summary>
        /// ������ �����������
        /// </summary>
        [SerializeField] private GameObject RegisterGO;

        /// <summary>
        /// ������� "����"
        /// </summary>
        [SerializeField] private GameObject LoginLabel;

        /// <summary>
        /// ������� "�����������"
        /// </summary>
        [SerializeField] private GameObject RegisterLabel;

        private void Awake()
        {
            Screen.fullScreen = false;
        }

        /// <summary>
        /// ������������ ����� � �����������
        /// </summary>
        public void Switch()
        {
            LoginGO.SetActive(!LoginGO.activeSelf);
            LoginLabel.SetActive(!LoginLabel.activeSelf);

            RegisterGO.SetActive(!RegisterGO.activeSelf);
            RegisterLabel.SetActive(!RegisterLabel.activeSelf);
        }
    }
}