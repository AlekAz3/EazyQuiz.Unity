using UnityEngine;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// ���������� ������ �������������� � �����������
    /// </summary>
    public class AuthorizationPanel : MonoBehaviour
    {
        /// <summary>
        /// ������ ��������������
        /// </summary>
        [SerializeField] private GameObject loginGo;

        /// <summary>
        /// ������ �����������
        /// </summary>
        [SerializeField] private GameObject registerGo;

        /// <summary>
        /// ������� "����"
        /// </summary>
        [SerializeField] private GameObject loginLabel;

        /// <summary>
        /// ������� "�����������"
        /// </summary>
        [SerializeField] private GameObject registerLabel;

        /// <summary>
        /// ������� "�����" �� ������
        /// </summary>
        [SerializeField] private GameObject loginButtonText;

        /// <summary>
        /// ������� "�����������" �� ������
        /// </summary>
        [SerializeField] private GameObject registerButtonText;

        private void Awake()
        {
            Screen.fullScreen = false;
        }

        /// <summary>
        /// ������������ ����� � �����������
        /// </summary>
        public void Switch()
        {
            loginGo.SetActive(!loginGo.activeSelf);
            loginLabel.SetActive(!loginLabel.activeSelf);

            registerGo.SetActive(!registerGo.activeSelf);
            registerLabel.SetActive(!registerLabel.activeSelf);

            loginButtonText.SetActive(!loginButtonText.activeSelf);
            registerButtonText.SetActive(!registerButtonText.activeSelf);
        }
    }
}