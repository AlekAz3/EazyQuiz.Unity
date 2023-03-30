using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Auth
{
    /// <summary>
    /// Всплывающее окно ошибки
    /// </summary>
    public class ErrorScreen : MonoBehaviour
    {
        /// <summary>
        /// <see cref="GameObject"/> окна
        /// </summary>
        [SerializeField] public GameObject ErrorLabel;

        /// <summary>
        /// Текст окна
        /// </summary>
        [SerializeField] private TMP_Text ErrorText;

        /// <summary>
        /// Скрыть окно
        /// </summary>
        public void Hide()
        {
            ErrorLabel.SetActive(false);
        }

        /// <summary>
        /// Показать окно с текстом ошибки
        /// </summary>
        /// <param name="error">Текст ошибки</param>
        public void Activate(string error)
        {
            ErrorLabel.SetActive(true);
            ErrorText.text = error;
        }
    }
}