using TMPro;
using UnityEngine;

namespace EazyQuiz.Unity.Elements.Common
{
    /// <summary>
    /// Всплывающее окно ошибки
    /// </summary>
    public class InformationScreen : MonoBehaviour
    {
        /// <summary>
        /// <see cref="GameObject"/> окна
        /// </summary>
        [SerializeField] public GameObject InformationLabel;

        /// <summary>
        /// Текст окна
        /// </summary>
        [SerializeField] private TMP_Text InformationText;
        /// <summary>
        /// Текст окна
        /// </summary>
        [SerializeField] private TMP_Text InformationHeader;

        /// <summary>
        /// Скрыть окно
        /// </summary>
        public void Hide()
        {
            InformationLabel.SetActive(false);
        }

        /// <summary>
        /// Показать окно с текстом ошибки
        /// </summary>
        /// <param name="error">Текст ошибки</param>
        public void ShowError(string error)
        {
            InformationLabel.SetActive(true);
            InformationHeader.text = "Ошибка";
            InformationText.text = error;
        }

        /// <summary>
        /// Показать окно с благодарностью 
        /// </summary>
        /// <param name="text">Текст</param>
        public void ShowInformation(string text)
        {
            InformationLabel.SetActive(true);
            InformationHeader.text = "Спасибо";
            InformationText.text = text;
        }
    }
}