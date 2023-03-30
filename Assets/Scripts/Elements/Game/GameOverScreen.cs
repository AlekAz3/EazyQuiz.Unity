using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Game
{
    /// <summary>
    /// Элемент завершения игры
    /// </summary>
    public class GameOverScreen : MonoBehaviour
    {
        /// <summary>
        /// <see cref="GameObject"/> окна
        /// </summary>
        [SerializeField] private GameObject screen;

        /// <summary>
        /// Текст который надо вывести
        /// </summary>
        [SerializeField] private TMP_Text text;

        /// <summary>
        /// Вывести окно с кастомным текстом
        /// </summary>
        /// <param name="inpitText">Текст</param>
        public void Show(string inpitText)
        {
            screen.SetActive(true);
            text.text = inpitText;
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