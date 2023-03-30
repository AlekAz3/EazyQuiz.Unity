using UnityEngine;

namespace EazyQuiz.Unity.Elements.Common
{
    /// <summary>
    /// Окно загрузки
    /// </summary>
    public class LoadingScreen : MonoBehaviour
    {
        /// <summary>
        /// <see cref="GameObject"/> окна
        /// </summary>
        [SerializeField] private GameObject screen;

        /// <summary>
        /// Показать окно
        /// </summary>
        public void Show()
        {
            screen.SetActive(true);
        }

        /// <summary>
        /// Скрыть окно
        /// </summary>
        public void Hide()
        {
            screen.SetActive(false);
        }
    }
}