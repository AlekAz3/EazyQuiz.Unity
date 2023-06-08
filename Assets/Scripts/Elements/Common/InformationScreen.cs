using EazyQuiz.Unity.Controllers;
using EazyQuiz.Unity.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

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
        [SerializeField] public GameObject informationLabel;

        /// <summary>
        /// Текст окна
        /// </summary>
        [SerializeField] private TMP_Text informationText;
        /// <summary>
        /// Текст окна
        /// </summary>
        [SerializeField] private TMP_Text informationHeader;
        
        /// <summary>
        /// Скрыть окно
        /// </summary>
        public void Hide()
        {
            informationLabel.SetActive(false);
        }

        /// <summary>
        /// Показать окно с текстом ошибки
        /// </summary>
        /// <param name="error">Текст ошибки</param>
        public void ShowError(string error)
        {
            informationLabel.SetActive(true);
            informationHeader.text = "Ошибка";
            informationText.text = error;
        }

        /// <summary>
        /// Показать окно с информацией 
        /// </summary>
        /// <param name="text">Текст</param>
        public void ShowInformation(string text)
        {
            informationLabel.SetActive(true);
            informationHeader.text = "Информация";
            informationText.text = text;
        }
    }
}