using UnityEngine;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// Контроллер панели Аутентификации и Регистрации
    /// </summary>
    public class AuthorizationPanel : MonoBehaviour
    {
        /// <summary>
        /// Панель Аутентификации
        /// </summary>
        [SerializeField] private GameObject loginGo;

        /// <summary>
        /// Панель Регистрации
        /// </summary>
        [SerializeField] private GameObject registerGo;

        /// <summary>
        /// Надпись "Вход"
        /// </summary>
        [SerializeField] private GameObject loginLabel;

        /// <summary>
        /// Надпись "Регистрация"
        /// </summary>
        [SerializeField] private GameObject registerLabel;

        /// <summary>
        /// Надпись "Логин" на кнопке
        /// </summary>
        [SerializeField] private GameObject loginButtonText;

        /// <summary>
        /// Надпись "Регистрация" на кнопке
        /// </summary>
        [SerializeField] private GameObject registerButtonText;

        private void Awake()
        {
            Screen.fullScreen = false;
        }

        /// <summary>
        /// Переключение входа и регистрации
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