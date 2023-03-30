using UnityEngine;

namespace EazyQuiz.Unity.Controllers
{
    /// <summary>
    /// Контроллер панели Аутентификации и Регистрации
    /// </summary>
    public class AuthtorizationPanel : MonoBehaviour
    {
        /// <summary>
        /// Панель Аутентификации
        /// </summary>
        [SerializeField] private GameObject LoginGO;

        /// <summary>
        /// Панель Регистрации
        /// </summary>
        [SerializeField] private GameObject RegisterGO;

        /// <summary>
        /// Надпись "Вход"
        /// </summary>
        [SerializeField] private GameObject LoginLabel;

        /// <summary>
        /// Надпись "Регистрация"
        /// </summary>
        [SerializeField] private GameObject RegisterLabel;

        private void Awake()
        {
            Screen.fullScreen = false;
        }

        /// <summary>
        /// Переключение входа и регистрации
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